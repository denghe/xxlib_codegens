#pragma once
#include "xx_base.h"

namespace xx {
	// 最基础的二进制数据容器, bbuffer 的基类
	// 两种模式: 1. 追加模式    2. 只读引用模式
	struct Data {
		char*				buf = nullptr;
		size_t				len = 0;
		size_t				cap = 0;

		// buf 头部预留空间大小. 至少需要装得下 sizeof(size_t)
		static const size_t	recvLen = 16;

		// cap 用来表达用法 2 的特殊值
		static const size_t	special = (size_t)-1;

		Data() = default;

		// 预分配空间 构造
		explicit Data(size_t const& newCap) {
			if (newCap) {
				auto siz = Round2n(recvLen + cap);
				buf = (char*)::malloc(siz) + recvLen;
				cap = siz - recvLen;
			}
		}

		// 通过 复制一段数据 来构造
		Data(char const* const& ptr, size_t const& siz) {
			WriteBuf(ptr, siz);
		}

		// 复制构造
		Data(Data const& o) {
			operator=(o);
		}
		inline Data& operator=(Data const& o) {
			if (o.cap == special) {
				buf = o.buf;
				len = o.len;
				cap = o.cap;
				++Refs();
			}
			else {
				Clear();
				WriteBuf(o.buf, o.len);
			}
			return *this;
		}

		// 移动构造
		Data(Data&& o) {
			operator=(std::move(o));
		}
		inline Data& operator=(Data&& o) {
			std::swap(buf, o.buf);
			std::swap(len, o.len);
			std::swap(cap, o.cap);
			return *this;
		}

		// 判断数据是否一致
		inline bool operator==(Data const& o) {
			if (&o == this) return true;
			if (len != o.len) return false;
			return 0 == ::memcmp(buf, o.buf, len);
		}

		// 确保空间足够
		inline void Reserve(size_t const& newCap) {
			assert(cap != special);
			if (newCap <= cap) return;

			auto siz = Round2n(recvLen + newCap);
			auto newBuf = (char*)::malloc(siz) + recvLen;
			::memcpy(newBuf, buf, len);

			// 这里判断 cap 不判断 buf, 是因为 gcc 优化会导致 if 失效, 无论如何都会执行 free
			if (cap) {
				::free(buf - recvLen);
			}
			buf = newBuf;
			cap = siz - recvLen;
		}

		// 返回旧长度
		inline size_t Resize(size_t const& newLen) {
			assert(cap != special);
			if (newLen > len) {
				Reserve(newLen);
			}
			auto rtv = len;
			len = newLen;
			return rtv;
		}

		// 下标访问
		inline char& operator[](size_t const& idx) {
			assert(idx < len);
			return buf[idx];
		}
		inline char const& operator[](size_t const& idx) const {
			assert(idx < len);
			return buf[idx];
		}


		// 从头部移除指定长度数据( 常见于拆包处理移除掉已经访问过的包数据, 将残留部分移动到头部 )
		inline void RemoveFront(size_t const& siz) {
			assert(cap != special);
			assert(siz <= len);
			if (!siz) return;
			len -= siz;
			if (len) {
				::memmove(buf, buf + siz, len);
			}
		}

		// 追加写入一段 buf
		inline void WriteBuf(char const* const& ptr, size_t const& siz) {
			assert(cap != special);
			Reserve(len + siz);
			::memcpy(buf + len, ptr, siz);
			len += siz;
		}

		// 设置为只读模式, 并初始化引用计数( 开启只读引用计数模式. 没数据不允许开启 )
		inline void SetReadonlyMode() {
			assert(cap != special);
			assert(len);
			cap = special;
			Refs() = 1;
		}

		// 判断是否为只读模式
		inline bool Readonly() const {
			return cap == special;
		}

		// 访问引用计数
		inline size_t& Refs() const {
			assert(cap == special);
			return *(size_t*)(buf - recvLen);
		}

		// 引用模式减持, 追加模式释放 buf
		~Data() {
			if (cap == special && --Refs()) return;
			if (cap) {
				::free(buf - recvLen);
			}
		}

		// len 清 0, 可彻底释放 buf
		inline void Clear(bool const& freeBuf = false) {
			assert(cap != special);
			if (freeBuf && cap) {
				::free(buf - recvLen);
				buf = nullptr;
				cap = 0;
			}
			len = 0;
		}
	};

	// 标识内存可移动
	template<>
	struct IsTrivial<Data, void> : std::true_type {};


	/**********************************************************************************************************************/
	// 适配 SFuncs

	// 适配 xx::Data
	template<>
	struct SFuncs<Serializer, void> {
		static inline void Append(std::string& s, Data const& in) {
			xx::Append(s, "{ \"len\":", in.len, ", \"cap\":", in.cap, ", \"buf\":[ ");
			for (size_t i = 0; i < in.len; i++) {
				xx::Append(s, (int)in.buf[i], ", ");
			}
			if (in.len) s.resize(s.size() - 2);
			s += " ] }";
		}
	};
}
