﻿#pragma once
#include "xx_object.h"
#include "xx_list.h"
namespace xx {
	struct BBuffer : List<uint8_t> {
		using BaseType = List<uint8_t>;

		// 读指针偏移量
		size_t offset = 0;

		// offset值写入修正
		size_t offsetRoot = 0;

		// 写入引用对象时用于理清引用关系的 指针 字典
		std::shared_ptr<std::unordered_map<void*, size_t>> ptrs;

		// 读出引用对象时用于理清引用关系的 下标 字典
		std::shared_ptr<std::unordered_map<size_t, std::shared_ptr<Object>>> idxs;



		BBuffer() : BaseType() {}
		BBuffer(BBuffer&& o) noexcept
			: BaseType(std::move(o))
			, offset(o.offset) {
			o.offset = 0;
		}
		inline BBuffer& operator=(BBuffer&& o) noexcept {
			this->BaseType::operator=(std::move(o));
			std::swap(offset, o.offset);
			// ptrs, idxs 因为是临时数据, 不需要处理
			return *this;
		}
		BBuffer(BBuffer const&) = delete;
		BBuffer& operator=(BBuffer const&) = delete;



		// unsafe: 直接篡改内存( 常见于公用借壳 reader bb )
		inline void Reset(uint8_t* const& buf = nullptr, size_t const& len = 0, size_t const& cap = 0, size_t const& offset = 0) noexcept {
			this->buf = buf;
			this->len = len;
			this->cap = cap;
			this->offset = offset;
		}


		// 定长写
		template<typename T, typename ENABLED = std::enable_if_t<std::is_pod_v<T>>>
		void WriteFixed(T const& v) {
			Reserve(len + sizeof(T));
			memcpy(buf + len, &v, sizeof(T));
			len += sizeof(T);
		}

		// 定长读
		template<typename T, typename ENABLED = std::enable_if_t<std::is_pod_v<T>>>
		int ReadFixed(T& v) {
			if (offset + sizeof(T) > len) return -1;
			memcpy(&v, buf + offset, sizeof(T));
			offset += sizeof(T);
			return 0;
		}


		// 变长写
		template<typename T, bool needReserve = true>
		inline void WriteVarIntger(T const& v) {
			using UT = std::make_unsigned_t<T>;
			UT u(v);
			if constexpr (std::is_signed_v<T>) {
				u = ZigZagEncode(v);
			}
			if constexpr (needReserve) {
				Reserve(len + sizeof(T) + 1);
			}
			while (u >= 1 << 7) {
				buf[len++] = uint8_t((u & 0x7fu) | 0x80u);
				u = UT(u >> 7);
			};
			buf[len++] = uint8_t(u);
		}

		// 变长读
		template<typename T>
		inline int ReadVarInteger(T& v) {
			using UT = std::make_unsigned_t<T>;
			UT u(0);
			for (size_t shift = 0; shift < sizeof(T) * 8; shift += 7) {
				if (offset == len) return -9;
				auto b = (UT)buf[offset++];
				u |= UT((b & 0x7Fu) << shift);
				if ((b & 0x80) == 0) {
					if constexpr (std::is_signed_v<T>) {
						v = ZigZagDecode(u);
					}
					else {
						v = u;
					}
					return 0;
				}
			}
			return -10;
		}


		// 同时写多个
		template<typename ...TS>
		void Write(TS const& ...vs) noexcept {
			std::initializer_list<int> n{ (BFuncs<TS>::Write(*this, vs), 0)... };
			(void)n;
		}

		template<typename T, typename ...TS>
		int ReadCore(T& v, TS&...vs) noexcept {
			if (auto r = BFuncs<T>::Read(*this, v)) return r;
			return ReadCore(vs...);
		}

		template<typename T>
		int ReadCore(T& v) noexcept {
			return BFuncs<T>::Read(*this, v);
		}

		// 同时读多个
		template<typename ...TS>
		int Read(TS&...vs) noexcept {
			return ReadCore(vs...);
		}


		// 读 string 同时检查长度限制
		template<size_t limit>
		int ReadLimit(std::string& out) {
			size_t siz = 0;
			if (auto r = Read(siz)) return r;
			if (limit && siz > limit) return -1;
			if (offset + siz > len) return -2;
			out.assign((char*)buf + offset, siz);
			offset += siz;
			return 0;
		}

		// 读 BBuffer 同时检查长度限制
		template<size_t limit>
		int ReadLimit(BBuffer& out) {
			assert(&out != this);
			size_t siz = 0;
			if (auto r = Read(siz)) return r;
			if (limit && siz > limit) return -1;
			if (offset + siz > len) return -2;
			out.Clear();
			out.AddRange(buf + offset, siz);
			offset += siz;
			return 0;
		}

		// 读 vector<vector<vector<... 同时检查长度限制
		template<size_t limit, size_t ...limits, typename T, typename ENABLED = std::enable_if_t<xx::IsVector_v<T> && xx::DeepLevel_v<T> == sizeof...(limits)>>
		int ReadLimit(T& out) {
			size_t siz = 0;
			if (auto rtv = Read(siz)) return rtv;
			if (limit != 0 && siz > limit) return -1;
			if (offset + siz > len) return -2;
			out.resize(siz);
			auto buf = out.data();
			if (siz == 0) return 0;
			if constexpr (sizeof...(limits)) {
				static_assert(IsVector_v<typename T::value_type>);
				for (size_t i = 0; i < siz; ++i) {
					if (int r = ReadLimit<limits...>(buf[i])) return r;
				}
			}
			else {
				if constexpr (sizeof(T) == 1 || std::is_same_v<float, T>) {
					::memcpy(buf, buf + offset, siz * sizeof(T));
					offset += siz * sizeof(T);
				}
				else {
					for (size_t i = 0; i < siz; ++i) {
						if (int r = Read(buf[i])) return r;
					}
				}
			}
			return 0;
		}



		// 写 一组关联数据( 会为追踪引用关系准备字典 )
		template<typename ...TS>
		void WriteRoot(TS const&...vs) noexcept {
			if (!ptrs) {
				MakeTo(ptrs);
			}
			assert(ptrs->empty());
			offsetRoot = len;
			Write(vs...);
			ptrs->clear();
		}

		// 读 一组关联数据( 会为追踪引用关系准备字典 )
		template<typename ...TS>
		int ReadRoot(TS&...vs) noexcept {
			if (!idxs) {
				MakeTo(idxs);
			}
			assert(idxs->empty());
			offsetRoot = offset;
			int r = Read(vs...);
			idxs->clear();
			return r;
		}


		// 写 引用类实例
		template<typename T, typename ENABLED = std::enable_if_t<std::is_base_of_v<Object, T>>>
		void WritePtr(std::shared_ptr<T> const& v) noexcept {
			uint16_t typeId = 0;
			if (!v) {
				Write(typeId);
				return;
			}
			typeId = v->GetTypeId();
			assert(typeId);					// forget Register TypeId ? 
			Write(typeId);

			auto iter = ptrs->find((void*)&*v);
			size_t offs;
			if (iter == ptrs->end()) {
				offs = len - offsetRoot;
				(*ptrs)[(void*)&*v] = offs;
			}
			else {
				offs = iter->second;
			}
			Write(offs);
			if (iter == ptrs->end()) {
				v->ToBBuffer(*this);
			}
		}

		// 读 引用类实例
		template<typename T, typename ENABLED = std::enable_if_t<std::is_base_of_v<Object, T>>>
		int ReadPtr(std::shared_ptr<T>& v) noexcept {
			v.reset();
			uint16_t typeId;
			if (auto r = Read(typeId)) return r;
			if (typeId == 0) return 0;	// nullptr
			if (!creators[typeId]) return -1;		// forget Register?

			auto offs = offset - offsetRoot;
			size_t ptrOffset;
			if (auto r = Read(ptrOffset)) return r;
			if (ptrOffset == offs) {
				std::shared_ptr<Object> o;
				o = CreateByTypeId(typeId);
				if (!o) return -3;
				v = std::dynamic_pointer_cast<T>(o);
				if (!v) return -4;
				(*idxs)[ptrOffset] = o;
				if (auto r = o->FromBBuffer(*this)) return r;
			}
			else {
				auto iter = idxs->find(ptrOffset);
				if (iter == idxs->end()) return -6;
				if (iter->second->GetTypeId() != typeId) return -7;
				v = std::dynamic_pointer_cast<T>(iter->second);
				if (!v) return -8;
			}
			return 0;
		}

		/**********************************************************************************************************************/
		// 全局静态创建函数注册相关

		// 类实例创建函数集
		typedef std::shared_ptr<Object>(*Creator)();
		inline static std::array<Creator, 1 << (sizeof(uint16_t) * 8)> creators;

		// 注册类实例创建函数
		template<typename T, typename ENABLED = std::enable_if_t<std::is_base_of_v<Object, T>>>
		inline static void Register(uint16_t const& typeId) noexcept {
			creators[typeId] = []()->std::shared_ptr<Object> { return xx::TryMake<T>(); };
		}

		// 根据 typeId 调用类创建函数 并返回对象
		inline static std::shared_ptr<Object> CreateByTypeId(uint16_t typeId) {
			return creators[typeId] ? creators[typeId]() : std::shared_ptr<Object>();
		}
	};


	// 标识内存可移动
	template<>
	struct IsTrivial<BBuffer, void> : std::true_type {};

	/**********************************************************************************************************************/
	// 适配 SFuncs

	template<>
	struct SFuncs<BBuffer, void> {
		static inline void Append(std::string& s, BBuffer const& in) noexcept {
			xx::Append(s, "{ \"len\":", in.len, ", \"cap\":", in.cap, ", \"offset\":", in.offset, ", \"buf\":[ ");
			for (size_t i = 0; i < in.len; i++) {
				xx::Append(s, (int)in.buf[i], ", ");
			}
			if (in.len) s.resize(s.size() - 2);
			s += " ] }";
		}
	};

	/**********************************************************************************************************************/
	// 适配 BFuncs

	// 适配 xx::BBuffer
	template<>
	struct BFuncs<BBuffer, void> {
		static inline void Write(BBuffer& bb, BBuffer const& in) noexcept {
			assert(&in != &bb);
			bb.Write(in.len);
			bb.AddRange(in.buf, in.len);
		}
		static inline int Read(BBuffer& bb, BBuffer& out) noexcept {
			return bb.ReadLimit<0>(out);
		}
	};

	// 适配 1 字节长度的 数值 或 float( 这些类型直接 memcpy )
	template<typename T>
	struct BFuncs<T, std::enable_if_t< (std::is_arithmetic_v<T> && sizeof(T) == 1) || (std::is_floating_point_v<T> && sizeof(T) == 4) >> {
		static inline void Write(BBuffer& bb, T const& in) noexcept {
			bb.Reserve(bb.len + sizeof(T));
			memcpy(bb.buf + bb.len, &in, sizeof(T));
			bb.len += sizeof(T);
		}
		static inline int Read(BBuffer& bb, T& out) noexcept {
			if (bb.offset + sizeof(T) > bb.len) return -12;
			memcpy(&out, bb.buf + bb.offset, sizeof(T));
			bb.offset += sizeof(T);
			return 0;
		}
	};

	// 适配 2+ 字节整数( 变长读写 )
	template<typename T>
	struct BFuncs<T, std::enable_if_t<std::is_integral_v<T> && sizeof(T) >= 2>> {
		static inline void Write(BBuffer& bb, T const& in) noexcept {
			bb.WriteVarIntger(in);
		}
		static inline int Read(BBuffer& bb, T& out) noexcept {
			return bb.ReadVarInteger(out);
		}
	};

	// 适配 enum( 根据原始数据类型调上面的适配 )
	template<typename T>
	struct BFuncs<T, std::enable_if_t<std::is_enum_v<T>>> {
		typedef std::underlying_type_t<T> UT;
		static inline void Write(BBuffer& bb, T const& in) noexcept {
			bb.Write((UT const&)in);
		}
		static inline int Read(BBuffer& bb, T& out) noexcept {
			return bb.Read((UT&)out);
		}
	};

	// 适配 double
	template<>
	struct BFuncs<double, void> {
		static inline void Write(BBuffer& bb, double const& in) noexcept {
			bb.Reserve(bb.len + sizeof(double) + 1);
			if (in == 0) {
				bb.buf[bb.len++] = 0;
			}
			else if (std::isnan(in)) {
				bb.buf[bb.len++] = 1;
			}
			else if (in == -std::numeric_limits<double>::infinity()) {	// negative infinity
				bb.buf[bb.len++] = 2;
			}
			else if (in == std::numeric_limits<double>::infinity()) {	// positive infinity
				bb.buf[bb.len++] = 3;
			}
			else {
				auto i = (int32_t)in;
				if (in == (double)i) {
					bb.buf[bb.len++] = 4;
					bb.WriteVarIntger<int32_t, false>(i);
				}
				else {
					bb.buf[bb.len] = 5;
					memcpy(bb.buf + bb.len + 1, &in, sizeof(double));
					bb.len += sizeof(double) + 1;
				}
			}
		}
		static inline int Read(BBuffer& bb, double& out) noexcept {
			if (bb.offset >= bb.len) return -13;	// 确保还有 1 字节可读
			switch (bb.buf[bb.offset++]) {			// 跳过 1 字节
			case 0:
				out = 0;
				return 0;
			case 1:
				out = std::numeric_limits<double>::quiet_NaN();
				return 0;
			case 2:
				out = -std::numeric_limits<double>::infinity();
				return 0;
			case 3:
				out = std::numeric_limits<double>::infinity();
				return 0;
			case 4: {
				int32_t i = 0;
				if (auto rtv = BFuncs<int32_t>::Read(bb, i)) return rtv;
				out = i;
				return 0;
			}
			case 5: {
				if (bb.len < bb.offset + sizeof(double)) return -14;
				memcpy(&out, bb.buf + bb.offset, sizeof(double));
				bb.offset += sizeof(double);
				return 0;
			}
			default:
				return -15;							// failed
			}
		}
	};


	// 适配 literal char[len] string  ( 写入 变长长度-1 + 内容. 不写入末尾 0 )
	template<size_t len>
	struct BFuncs<char[len], void> {
		static inline void Write(BBuffer& bb, char const(&in)[len]) noexcept {
			bb.Write((size_t)(len - 1));
			bb.AddRange((uint8_t*)in, len - 1);
		}
		static inline int Read(BBuffer& bb, char(&out)[len]) noexcept {
			size_t readLen = 0;
			if (auto r = bb.Read(readLen)) return r;
			if (bb.offset + readLen > bb.len) return -19;
			if (readLen >= len) return -20;
			memcpy(out, bb.buf + bb.offset, readLen);
			out[readLen] = 0;
			bb.offset += readLen;
			return 0;
		}
	};

	// 适配 std::string ( 写入 变长长度 + 内容 )
	template<>
	struct BFuncs<std::string, void> {
		static inline void Write(BBuffer& bb, std::string const& in) noexcept {
			bb.Write(in.size());
			bb.AddRange((uint8_t*)in.data(), in.size());
		}
		static inline int Read(BBuffer& bb, std::string& out) noexcept {
			return bb.ReadLimit<0>(out);
		}
	};



	// 适配 std::optional<T>
	template<typename T>
	struct BFuncs<std::optional<T>, void> {
		static inline void Write(BBuffer& bb, std::optional<T> const& in) noexcept {
			if (in.has_value()) {
				bb.Write((uint8_t)1, in.value());
			}
			else {
				bb.Write((uint8_t)0);
			}
		}
		static inline int Read(BBuffer& bb, std::optional<T>& out) noexcept {
			uint8_t hasValue = 0;
			if (int r = bb.Read(hasValue)) return r;
			if (!hasValue) return 0;
			if (!out.has_value()) {
				out.emplace();
			}
			return bb.Read(out.value());
		}
	};

	// 适配 std::vector<T>
	template<typename T>
	struct BFuncs<std::vector<T>, void> {
		static inline void Write(BBuffer& bb, std::vector<T> const& in) noexcept {
			auto buf = in.data();
			auto len = in.size();
			bb.Reserve(bb.len + 5 + len * sizeof(T));
			bb.Write(len);
			if (!len) return;
			if constexpr (sizeof(T) == 1 || std::is_same_v<float, T>) {
				::memcpy(bb.buf + bb.len, buf, len * sizeof(T));
				bb.len += len * sizeof(T);
			}
			else {
				for (size_t i = 0; i < len; ++i) {
					bb.Write(buf[i]);
				}
			}
		}
		static inline int Read(BBuffer& bb, std::vector<T>& out) noexcept {
			return bb.ReadLimit<0>(out);
		}
	};

	// 适配 std::shared_ptr<T : Object>
	template<typename T>
	struct BFuncs<std::shared_ptr<T>, std::enable_if_t<std::is_base_of_v<Object, T> || std::is_same_v<std::string, T>>> {
		static inline void Write(BBuffer& bb, std::shared_ptr<T> const& in) noexcept {
			bb.WritePtr(in);
		}
		static inline int Read(BBuffer& bb, std::shared_ptr<T>& out) noexcept {
			return bb.ReadPtr(out);
		}
	};

	// 适配 std::weak_ptr<T : Object>
	template<typename T>
	struct BFuncs<std::weak_ptr<T>, std::enable_if_t<std::is_base_of_v<Object, T> || std::is_same_v<std::string, T>>> {
		static inline void Write(BBuffer& bb, std::weak_ptr<T> const& in) noexcept {
			if (auto ptr = in.lock()) {
				bb.WritePtr(ptr);
			}
			else {
				bb.Write((uint16_t)0);
			}
		}
		static inline int Read(BBuffer& bb, std::weak_ptr<T>& out) noexcept {
			std::shared_ptr<T> ptr;
			if (int r = bb.ReadPtr(ptr)) return r;
			out = ptr;
			return 0;
		}
	};
}
