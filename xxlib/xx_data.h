#pragma once
#include "xx_base.h"

namespace xx {
	// ������Ķ�������������, bbuffer �Ļ���
	// �Դ����ü���( �� buf ͷԤ�� 32 �ֽ�, ���ü����ͷ���ͷ��, ͬʱҲ���������ͷ�����������Ϣ )
	// �����÷�: 1. �������    2. ����ֻ������ & ɾ��
	struct Data {
		char* buf = nullptr;
		size_t				len = 0;
		size_t				cap = 0;

		// buf ͷ��Ԥ���ռ��С. ������Ҫװ���� sizeof(size_t)
		static const size_t	recvLen = 16;

		// cap ��������÷� 2 ������ֵ
		static const size_t	special = (size_t)-1;

		Data() = default;
		explicit Data(size_t const& newCap) {
			if (newCap) {
				auto siz = Round2n(recvLen + cap);
				buf = (char*)::malloc(siz) + recvLen;
				cap = siz - recvLen;
			}
		}

		// ���ƹ���
		Data(Data const& o) {
			operator=(o);
		}
		Data& operator=(Data&& o) {
			std::swap(buf, o.buf);
			std::swap(len, o.len);
			std::swap(cap, o.cap);
			return *this;
		}

		// �ƶ�����
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

		// �ж������Ƿ�һ��
		bool operator==(Data const& o) {
			if (&o == this) return true;
			if (len != o.len) return false;
			return 0 == ::memcmp(buf, o.buf, len);
		}

		// ȷ���ռ��㹻
		void Reserve(size_t const& newCap) {
			assert(cap != special);
			if (newCap <= cap) return;

			auto siz = Round2n(recvLen + newCap);
			auto newBuf = (char*)::malloc(siz) + recvLen;
			::memcpy(newBuf, buf, len);

			// �����ж� cap ���ж� buf, ����Ϊ gcc �Ż��ᵼ�� if ʧЧ, ������ζ���ִ�� free
			if (cap) {
				::free(buf - recvLen);
			}
			buf = newBuf;
			cap = siz - recvLen;
		}

		// ���ؾɳ���
		size_t Resize(size_t const& newLen) {
			assert(cap != special);
			if (newLen > len) {
				Reserve(newLen);
			}
			auto rtv = len;
			len = newLen;
			return rtv;
		}

		// �±����
		char& operator[](size_t const& idx) {
			assert(idx < len);
			return buf[idx];
		}
		char const& operator[](size_t const& idx) const {
			assert(idx < len);
			return buf[idx];
		}


		// ��ͷ���Ƴ�ָ����������( �����ڲ�������Ƴ����Ѿ����ʹ��İ�����, �����������ƶ���ͷ�� )
		inline void RemoveFront(size_t const& siz) {
			assert(cap != special);
			assert(siz <= len);
			if (!siz) return;
			len -= siz;
			if (len) {
				::memmove(buf, buf + siz, len);
			}
		}

		// ׷��һ�� buf
		void AddRange(char const* const& ptr, size_t const& siz) {
			assert(cap != special);
			Reserve(len + siz);
			::memcpy(buf + len, ptr, siz);
			len += siz;
		}

		// unsafe: Ӧ��ĳЩ����, ֱ�Ӵ۸��ڴ�
		void Reset(char* const& newBuf = nullptr, size_t const& newLen = 0, size_t const& newCap = 0) {
			assert(cap != special);
			buf = newBuf;
			len = newLen;
			cap = newCap;
		}

		// ��ʼ�����ü���( ����ֻ�����ü���ģʽ. û���ݲ������� )
		void InitRefs() {
			assert(cap != special);
			assert(len);
			cap = special;
			Refs() = 1;
		}

		// �������ü���
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

	// ��ʶ�ڴ���ƶ�
	template<>
	struct IsTrivial<Data, void> : std::true_type {};
}
