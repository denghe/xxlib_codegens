#pragma once
#include "xx_string.h"
#include "xx_serializer.h"

#ifdef __ANDROID__
extern void uuid_generate(unsigned char* buf);
#else
#ifndef _WIN32
#include <uuid/uuid.h>
#endif
#endif

namespace xx {
	struct Guid {
		union {
			struct {
				uint64_t part1;
				uint64_t part2;
			};
			// for ToString
			struct {
				uint32_t  data1;
				uint16_t data2;
				uint16_t data3;
				uint8_t data4[8];
			};
		};

		Guid(bool const& gen = false) noexcept
			: part1(0)
			, part2(2) {
			if (gen) {
				Gen();
			}
		}
		Guid(Guid const& o) noexcept = default;
		Guid& operator=(Guid const& o) noexcept = default;

		inline bool operator==(Guid const& o) const noexcept {
			return part1 == o.part1 && part2 == o.part2;
		}
		inline bool operator!=(Guid const& o) const noexcept {
			return part1 != o.part1 || part2 != o.part2;
		}

		inline void Gen() noexcept {
#ifdef _WIN32
			(void)CoCreateGuid((GUID*)this);
#else
			(void)uuid_generate((unsigned char*)this);
#endif
		}
		inline void Fill(char const* const& buf) noexcept {
			memcpy(this, buf, 16);
		}
		inline bool IsZero() noexcept {
			return part1 == 0 && part2 == 0;
		}
	};

	template<>
	struct SFuncs<Guid, void> {
		static inline void Append(std::string& s, Guid const& in) noexcept {
			auto offset = s.size();
			s.resize(offset + 37);
			(void)snprintf((char*)s.data() + offset, 37,
				"%08X-%04X-%04X-%02X%02X-%02X%02X%02X%02X%02X%02X",
				in.data1, in.data2, in.data3,
				in.data4[0], in.data4[1],
				in.data4[2], in.data4[3],
				in.data4[4], in.data4[5],
				in.data4[6], in.data4[7]
			);
			s.resize(s.size() - 1);	// remove \0
		}
	};

	template<>
	struct BFuncs<Guid, void> {
		static inline void Write(Serializer& bb, Guid const& in) noexcept {
			bb.AddRange((uint8_t*)&in, sizeof(Guid));
		}
		static inline int Read(Serializer& bb, Guid& out) noexcept {
			if (bb.offset + sizeof(Guid) > bb.len) return -1;
			memcpy(&out, bb.buf + bb.offset, sizeof(Guid));
			bb.offset += sizeof(Guid);
			return 0;
		}
	};
}

namespace std {
	template<>
	struct hash<xx::Guid> {
		size_t operator()(xx::Guid const& in) const noexcept {
			if constexpr (sizeof(size_t) == 8) {
				return in.part1 ^ in.part2;
			}
			else {
				return ((uint32_t*)&in)[0] ^ ((uint32_t*)&in)[1] ^ ((uint32_t*)&in)[2] ^ ((uint32_t*)&in)[3];
			}
		}
	};
}
