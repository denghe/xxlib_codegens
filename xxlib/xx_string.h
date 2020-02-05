#pragma once
#include "xx_base.h"
namespace xx {

	/************************************************************************************/
	// Append 相关

	// for Append
	template<typename T>
	void AppendCore(std::string& s, T const& v) {
		SFuncs<T>::Append(s, v);
	}

	// 便于向 std::string 追加内容的工具函数
	template<typename ...Args>
	void Append(std::string& s, Args const& ... args) {
		std::initializer_list<int> n{ ((AppendCore(s, args)), 0)... };
		(void)(n);
	}

	/************************************************************************************/
	// 各种针对 原生, std 对象的适配

	// 适配 char* \0 结尾 字串( 不是很高效 )
	template<>
	struct SFuncs<char*, void> {
		static inline void Append(std::string& s, char* const& in) noexcept {
			if (in) {
				s.append(in);
			};
		}
	};

	// 适配 char const* \0 结尾 字串( 不是很高效 )
	template<>
	struct SFuncs<char const*, void> {
		static inline void Append(std::string& s, char const* const& in) noexcept {
			if (in) {
				s.append(in);
			};
		}
	};

	// 适配 literal char[len] string
	template<size_t len>
	struct SFuncs<char[len], void> {
		static inline void Append(std::string& s, char const(&in)[len]) noexcept {
			s.append(in);
		}
	};

	// 适配所有数字
	template<typename T>
	struct SFuncs<T, std::enable_if_t<std::is_arithmetic_v<T>>> {
		static inline void Append(std::string& s, T const& in) noexcept {
			if constexpr (std::is_same_v<bool, T>) {
				s.append(in ? "true" : "false");
			}
			else if constexpr (std::is_same_v<char, T>) {
				s.append(std::to_string((int)in));
			}
			else {
				s.append(std::to_string(in));
			}
		}
	};

	// 适配 enum( 根据原始数据类型调上面的适配 )
	template<typename T>
	struct SFuncs<T, std::enable_if_t<std::is_enum_v<T>>> {
		static inline void Append(std::string& s, T const& in) noexcept {
			s.append(std::to_string((std::underlying_type_t<T>)in));
		}
	};


	// 适配 std::string
	template<typename T>
	struct SFuncs<T, std::enable_if_t<std::is_base_of_v<std::string, T>>> {
		static inline void Append(std::string& s, T const& in) noexcept {
			s.append("\"");
			s.append(in);
			s.append("\"");
		}
	};


	// 适配 std::optional<T>
	template<typename T>
	struct SFuncs<std::optional<T>, void> {
		static inline void Append(std::string& s, std::optional<T> const& in) noexcept {
			if (in.has_value()) {
				SFuncs<T>::Append(s, in.value());
			}
			else {
				s.append("nil");
			}
		}
	};

	// 适配 std::vector<T>
	template<typename T>
	struct SFuncs<std::vector<T>, void> {
		static inline void Append(std::string& s, std::vector<T> const& in) noexcept {
			s += "[ ";
			for (auto&& o : in) {
				SFuncs<T>::Append(s, o);
				s += ", ";
			}
			if (in.size()) {
				s.resize(s.size() - 2);
				s += " ]";
			}
			else {
				s[s.size() - 1] = ']';
			}
		}
	};

	/************************************************************************************/
	// 类型转换 相关

	// 转换 s 数据类型 为 T 填充 dst. 成功返回 true. 失败 dst 将填充默认值并返回 false
	template<typename T>
	inline bool TryParse(char const* const& s, T& dst) {
		if (!s) {
			dst = T();
			return false;
		}
		else if constexpr (std::is_integral_v<T>&& std::is_unsigned_v<T> && sizeof(T) <= 4) {
			dst = (T)strtoul(s, nullptr, 0);
		}
		else if constexpr (std::is_integral_v<T> && !std::is_unsigned_v<T> && sizeof(T) <= 4) {
			dst = (T)atoi(s);
		}
		else if constexpr (std::is_integral_v<T>&& std::is_unsigned_v<T> && sizeof(T) == 8) {
			dst = strtoull(s, nullptr, 0);
		}
		else if constexpr (std::is_integral_v<T> && !std::is_unsigned_v<T> && sizeof(T) == 8) {
			dst = atoll(s);
		}
		else if constexpr (std::is_floating_point_v<T> && sizeof(T) == 4) {
			dst = strtof(s, nullptr);
		}
		else if constexpr (std::is_floating_point_v<T> && sizeof(T) == 8) {
			dst = atof(s);
		}
		else if constexpr (std::is_same_v<T, bool>) {
			dst = s[0] == '1' || s[0] == 't' || s[0] == 'T' || s[0] == 'y' || s[0] == 'Y';
		}
		else if constexpr (std::is_same_v<T, std::string>) {
			dst = s;
		}
		// todo: more
		return false;
	}


	inline int FromHex(uint8_t const& c) noexcept {
		if (c >= 'A' && c <= 'Z') return c - 'A' + 10;
		else if (c >= 'a' && c <= 'z') return c - 'a' + 10;
		else if (c >= '0' && c <= '9') return c - '0';
		else return 0;
	}


	/************************************************************************************/
	// http 相关

	inline void UrlDecode(std::string const& src, std::string& dst) {
		auto&& length = src.size();
		dst.clear();
		dst.reserve(length);
		for (size_t i = 0; i < length; i++) {
			if (src[i] == '+') {
				dst += ' ';
			}
			else if (src[i] == '%') {
				if (i + 2 >= length) return;
				auto high = FromHex(src[i + 1]);
				auto low = FromHex(src[i + 2]);
				i += 2;
				dst += ((char)(uint8_t)(high * 16 + low));
			}
			else dst += src[i];
		}
	}

	inline std::string UrlDecode(std::string const& src) {
		std::string rtv;
		::xx::UrlDecode(src, rtv);
		return rtv;
	}


	inline void ToHex(uint8_t const& c, uint8_t& h1, uint8_t& h2) {
		auto a = c / 16;
		auto b = c % 16;
		h1 = (uint8_t)(a + ((a <= 9) ? '0' : ('a' - 10)));
		h2 = (uint8_t)(b + ((b <= 9) ? '0' : ('a' - 10)));
	}



	inline void UrlEncode(std::string const& src, std::string& dst) {
		auto&& str = src.c_str();
		auto&& siz = src.size();
		dst.clear();
		dst.reserve(siz * 2);
		for (size_t i = 0; i < siz; ++i) {
			char c = str[i];
			if ((c >= '0' && c <= '9') ||
				(c >= 'a' && c <= 'z') ||
				(c >= 'A' && c <= 'Z') ||
				c == '-' || c == '_' || c == '.' || c == '!' || c == '~' ||
				c == '*' || c == '\'' || c == '(' || c == ')') {
				dst += c;
			}
			else if (c == ' ') {
				dst += '+';
			}
			else
			{
				dst += '%';
				uint8_t h1, h2;
				ToHex(c, h1, h2);
				dst += h1;
				dst += h2;
			}
		}
	}

	inline std::string UrlEncode(std::string const& src) {
		std::string rtv;
		::xx::UrlEncode(src, rtv);
		return rtv;
	}



	inline void HtmlEncode(std::string const& src, std::string& dst) {
		auto&& str = src.c_str();
		auto&& siz = src.size();
		dst.clear();
		dst.reserve(siz * 2);	// 估算. * 6 感觉有点浪费
		for (size_t i = 0; i < siz; ++i) {
			auto c = str[i];
			switch (c) {
			case '&':  dst.append("&amp;"); break;
			case '\"': dst.append("&quot;"); break;
			case '\'': dst.append("&apos;"); break;
			case '<':  dst.append("&lt;");  break;
			case '>':  dst.append("&gt;");  break;
			default:   dst += c;			break;
			}
		}
	}

	inline std::string HtmlEncode(std::string const& src) {
		std::string rtv;
		::xx::HtmlEncode(src, rtv);
		return rtv;
	}
}
