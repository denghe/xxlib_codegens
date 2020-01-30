#pragma once
#include "xx_base.h"

namespace xx {
	// 最基础的二进制数据容器, bbuffer 的基类
	// 自带引用计数( 在 buf 头预留 32 字节, 引用计数就放在头部, 同时也方便后期在头部填充其他信息 )
	// 两种用法: 1. 数据填充    2. 数据只读引用 & 删除
	struct Data {
		char* buf = nullptr;
		size_t				len = 0;
		size_t				cap = 0;

		// buf 头部预留空间大小. 至少需要装得下 sizeof(size_t)
		static const size_t	recvLen = 16;

		// cap 用来表达用法 2 的特殊值
		static const size_t	special = (size_t)-1;

		Data() = default;
		explicit Data(size_t const& newCap) {
			if (newCap) {
				auto siz = Round2n(recvLen + cap);
				buf = (char*)::malloc(siz) + recvLen;
				cap = siz - recvLen;
			}
		}

		// 复制构造
		Data(Data const& o) {
			operator=(o);
		}
		Data& operator=(Data&& o) {
			std::swap(buf, o.buf);
			std::swap(len, o.len);
			std::swap(cap, o.cap);
			return *this;
		}

		// 移动构造
		Data(Data&& o) {
			operator=(std::move(o));
		}
		Data& operator=(Data const& o) {
			if (o.cap == special) {
				buf = o.buf;
				len = o.len;
				cap = o.cap;
				++Refs();
			}
			else {
				Clear(cap < o.len);
				AddRange(o.buf, o.len);
			}
			return *this;
		}

		// 判断数据是否一致
		bool operator==(Data const& o) {
			if (&o == this) return true;
			if (len != o.len) return false;
			return 0 == ::memcmp(buf, o.buf, len);
		}

		// 确保空间足够
		void Reserve(size_t const& newCap) {
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
		size_t Resize(size_t const& newLen) {
			assert(cap != special);
			if (newLen > len) {
				Reserve(newLen);
			}
			auto rtv = len;
			len = newLen;
			return rtv;
		}

		// 下标访问
		char& operator[](size_t const& idx) {
			assert(idx < len);
			return buf[idx];
		}
		char const& operator[](size_t const& idx) const {
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

		// 追加一段 buf
		void AddRange(char const* const& ptr, size_t const& siz) {
			assert(cap != special);
			Reserve(len + siz);
			::memcpy(buf + len, ptr, siz);
			len += siz;
		}

		// unsafe: 应对某些需求, 直接篡改内存
		void Reset(char* const& newBuf = nullptr, size_t const& newLen = 0, size_t const& newCap = 0) {
			assert(cap != special);
			buf = newBuf;
			len = newLen;
			cap = newCap;
		}

		// 初始化引用计数( 开启只读引用计数模式. 没数据不允许开启 )
		void InitRefs() {
			assert(cap != special);
			assert(len);
			cap = special;
			Refs() = 1;
		}

		// 访问引用计数
		size_t& Refs() const {
			assert(cap == special);
			return *(size_t*)(buf - recvLen);
		}

		~Data() {
			if (cap == special && --Refs()) return;
			if (cap) {
				::free(buf - recvLen);
			}
		}

		void Clear(bool const& freeBuf = false) {
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
}
