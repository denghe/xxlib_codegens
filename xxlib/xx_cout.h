#pragma once
#include "xx_string.h"

namespace xx {
	// 替代 std::cout. 支持实现了 SFuncs 模板适配的类型
	template<typename...Args>
	inline void Cout(Args const& ...args) {
		std::string s;
		Append(s, args...);
		for (auto&& c : s) {
			if (!c) c = '^';
		}
		std::cout << s;		// 似乎会受 fcontext 切换影响 输出不能
		fflush(stdout);
	}

	// 在 Cout 基础上添加了换行
	template<typename...Args>
	inline void CoutN(Args const& ...args) {
		std::string s;
		Append(s, args...);
		for (auto&& c : s) {
			if (!c) c = '^';
		}
		std::cout << s << std::endl;
	}

	// 在 CoutN 基础上于头部添加了时间
	template<typename...Args>
	inline void CoutTN(Args const& ...args) {
		std::string s = "[";
		NowToString(s);
		s += "] ";
		Append(s, args...);
		for (auto&& c : s) {
			if (!c) c = '^';
		}
		std::cout << s << std::endl;
	}

	// 立刻输出
	inline void CoutFlush() {
		fflush(stdout);
	}

	/************************************************************************************/
	// 针对 windows cmd window 设置其输出字符形态为 utf8

	inline void SetConsoleUtf8() {
#ifdef _WIN32
		// 控制台显示乱码纠正, 设置字符集  system("chcp 65001");
		SetConsoleOutputCP(65001);
		CONSOLE_FONT_INFOEX info = { 0 };
		// 以下设置字体来支持中文显示。  
		info.cbSize = sizeof(info);
		info.dwFontSize.Y = 18;
		info.dwFontSize.X = 10;
		info.FontWeight = FW_NORMAL;
		wcscpy_s(info.FaceName, L"新宋体");
		SetCurrentConsoleFontEx(GetStdHandle(STD_OUTPUT_HANDLE), NULL, &info);
#endif
	}
}
