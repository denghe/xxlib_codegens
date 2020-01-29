#include "xx_all.h"
int main() {
	return 0;
}


//#include "PKG_class.h"
//
//// todo: 令 List 不再继承自 xx::Object. List, BBuffer, Random 都属于 struct 值类型, 都走 SFuncs BFuncs IFuncs 模板适配
//
//template<size_t limit, size_t ...limits, typename T>
//bool CheckLimitCore(T const& v) {
//	if (v.size() > limit) return true;
//	if constexpr (sizeof...(limits)) {
//		for (auto&& o : v) {
//			if (CheckLimitCore<limits...>(o)) return true;
//		}
//	}
//	return false;
//}
//
//template<size_t...limits, typename T, typename ENABLED = std::enable_if_t<xx::IsVector_v<T> && xx::DeepLevel_v<T> == sizeof...(limits)>>
//bool CheckLimit(T const& v) {
//	return CheckLimitCore<limits...>(v);
//}
//
//int main() {
//	std::vector<int> ints;
//	ints.emplace_back(1);
//	ints.emplace_back(2);
//	std::vector<std::vector<int>> intss;
//	intss.push_back(ints);
//	intss.push_back(ints);
//	intss.push_back(ints);
//	std::cout << CheckLimit<1>(ints) << std::endl;
//	std::cout << CheckLimit<2>(ints) << std::endl;
//	std::cout << CheckLimit<2, 2>(intss) << std::endl;
//	std::cout << CheckLimit<3, 1>(intss) << std::endl;
//	std::cout << CheckLimit<3, 2>(intss) << std::endl;
//
//
//	//Test(intss);
//	//Test<1, 3>(intss);
//
//	//xx::BBuffer bb;
//	////PKG::TestNamespace::B b;
//	////b._float = 1.23f;
//	////b._pos = { 1.1f, 2.2f };
//	////bb.Write(b);
//	//xx::CoutN(bb);
//
//	std::cin.get();
//	return 0;
//}
