﻿#pragma once
#include <time.h>
#include <chrono>

namespace xx {
	/************************************************************************************/
	// time_point <--> .net DateTime.Now.ToUniversalTime().Ticks converts

	// 经历时间精度: 秒后 7 个 0( 这是 windows 下最高精度. android/ios 会低1个0的精度 )
	typedef std::chrono::duration<long long, std::ratio<1LL, 10000000LL>> duration_10m;

	// 时间点 转 epoch (精度为秒后 7 个 0)
	inline int64_t TimePointToEpoch10m(std::chrono::system_clock::time_point const& val) noexcept {
		return std::chrono::duration_cast<duration_10m>(val.time_since_epoch()).count();
	}

	//  epoch (精度为秒后 7 个 0) 转 时间点
	inline std::chrono::system_clock::time_point Epoch10mToTimePoint(int64_t const& val) noexcept {
		return std::chrono::system_clock::time_point(std::chrono::duration_cast<std::chrono::system_clock::duration>(duration_10m(val)));
	}


	// 得到当前时间点
	inline std::chrono::system_clock::time_point NowTimePoint() noexcept {
		return std::chrono::system_clock::now();
	}

	// 得到当前时间点的 epoch (精度为秒后 7 个 0)
	inline int64_t NowEpoch10m() noexcept {
		return TimePointToEpoch10m(NowTimePoint());
	}


	// epoch (精度为秒后 7 个 0) 转为 .Net DateTime Utc Ticks
	inline int64_t Epoch10mToUtcDateTimeTicks(int64_t const& val) noexcept {
		return val + 621355968000000000LL;
	}

	// .Net DateTime Utc Ticks 转为 epoch (精度为秒后 7 个 0)
	inline int64_t UtcDateTimeTicksToEpoch10m(int64_t const& val) noexcept {
		return val - 621355968000000000LL;
	}


	// 时间点 转 epoch (精度为秒)
	inline int32_t TimePointToEpoch(std::chrono::system_clock::time_point const& val) noexcept {
		return (int32_t)(val.time_since_epoch().count() / 10000000);
	}

	//  epoch (精度为秒) 转 时间点
	inline std::chrono::system_clock::time_point EpochToTimePoint(int32_t const& val) noexcept {
		return std::chrono::system_clock::time_point(std::chrono::system_clock::time_point::duration((int64_t)val * 10000000));
	}


	// 得到当前 system 时间点的 epoch (精度为 ms)
	inline int64_t NowSystemEpochMS() noexcept {
		return std::chrono::duration_cast<std::chrono::milliseconds>(std::chrono::system_clock::now().time_since_epoch()).count();
	}
	// 得到当前 steady 时间点的 epoch (精度为 ms)
	inline int64_t NowSteadyEpochMS() noexcept {
		return std::chrono::duration_cast<std::chrono::milliseconds>(std::chrono::steady_clock::now().time_since_epoch()).count();
	}

    // 得到当前 steady 时间点的 epoch (精度为 seconds. 精确到小数点后 6 位)
    inline double NowSteadyEpochSeconds() noexcept {
        return (double)std::chrono::duration_cast<std::chrono::microseconds>(std::chrono::steady_clock::now().time_since_epoch()).count() / 1000000.0;
    }


//	// 当前时间转为字符串并填充
//	inline void NowToString(std::string& s) noexcept {
//		auto&& t = std::chrono::system_clock::to_time_t(std::chrono::system_clock::now());
//		std::tm tm;
//#ifdef _WIN32
//		localtime_s(&tm, &t);
//#else
//		localtime_r(&t, &tm);
//#endif
//		std::stringstream ss;
//		ss << std::put_time(&tm, "%Y-%m-%d %X");
//		s += ss.str();
//	}
//
//	// 当前时间转为字符串并返回
//	inline std::string NowToString() noexcept {
//		std::string s;
//		NowToString(s);
//		return s;
//	}
}
