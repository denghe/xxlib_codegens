#include "xx_itempool.h"

struct RefBase;
inline xx::ItemPool<std::unique_ptr<RefBase>> allRefs;

struct RefBase {
	int indexAtContainer = -1;
	inline void Dispose() {
		if (indexAtContainer != -1) {
			auto indexAtContainer = this->indexAtContainer;	// copy to stack for fix gcc issue
			allRefs.RemoveAt(indexAtContainer);
		}
	}
};

template<typename T>
struct Ref {
	Ref() = default;
	Ref(Ref const&) = default;
	Ref& operator=(Ref const&) = default;

	int index = -1;
	int64_t version = 0;

	Ref(T* const& ptr) {
		static_assert(std::is_base_of_v<RefBase, T>);
		Reset(ptr);
	}
	Ref(std::unique_ptr<T> const& ptr) : Ref(ptr.get()) {}
	Ref(Ref&& o)
		: index(o.index)
		, version(o.version) {
		o.items = nullptr;
		o.index = -1;
		o.version = 0;
	}
	Ref& operator=(Ref&& o) {
		std::swap(index, o.index);
		std::swap(version, o.version);
		return *this;
	}
	template<typename U>
	Ref& operator=(std::enable_if_t<std::is_base_of_v<T, U>, Ref<U>> const& o) {
		return operator=(*(Ref<T>*) & o);
	}
	template<typename U>
	Ref& operator=(std::enable_if_t<std::is_base_of_v<T, U>, Ref<U>>&& o) {
		return operator=(std::move(*(Ref<T>*) & o));
	}
	template<typename U>
	Ref<U> As() const {
		if (!dynamic_cast<U*>(Lock())) return Ref<U>();
		return *(Ref<U>*)this;
	}
	operator bool() const {
		return version && allRefs.VersionAt(index) == version;
	}
	T* operator->() const {
		if (!operator bool()) throw - 1;		// 空指针
		return (T*)allRefs.ValueAt(index).get();
	}
	T* Lock() const {
		return operator bool() ? (T*)allRefs.ValueAt(index).get() : nullptr;
	}
	template<typename U = T>
	void Reset(T* const& ptr = nullptr) {
		static_assert(std::is_base_of_v<T, U>);
		if (!ptr) {
			index = -1;
			version = 0;
		}
		else {
			assert(ptr->indexAtContainer != -1);
			index = ptr->indexAtContainer;
			version = allRefs.VersionAt(index);
		}
	}
};
template<typename A, typename B>
inline bool operator==(Ref<A> const& a, Ref<B> const& b) {
	return a.Lock() == b.Lock();
}
template<typename A, typename B>
inline bool operator!=(Ref<A> const& a, Ref<B> const& b) {
	return a.Lock() != b.Lock();
}

template<typename T, typename ENABLED = std::enable_if_t<std::is_base_of_v<RefBase, T>>, typename ...Args>
Ref<T> MakeRef(Args &&... args) {
	auto&& u = std::make_unique<T>(std::forward<Args>(args)...);
	auto&& p = u.get();
	p->indexAtContainer = allRefs.Add(std::move(u));
	return p;
}

struct Node : RefBase {
	Ref<Node> parent;
	std::vector<Ref<Node>> childs;
};

int main(int argc, char** argv) {
	auto&& n = MakeRef<Node>();
	n->parent = n;
	n->childs.push_back(n);
	n->Dispose();

	std::cin.get();
	return 0;
}









//#include "xx_serializer.h"
//#include "xx_epoll.h"
//#include "PKG_class.h"
//#include "xx_sqlite.h"
//
//int main(int argc, char** argv) {
//
//
//
//	{
//		auto&& f = std::make_shared<PKG::Foo>();
//		auto&& list_string = f->list_list_string.emplace_back();
//		list_string.emplace_back("asdf");
//		auto&& list_data = f->list_list_data.emplace_back();
//		list_data.emplace_back(xx::Data{ 1,2,3,4/*,5*/ });
//		f->nullable_list_nullable_list_nullable_string.emplace();
//		auto&& nullable_list_nullable_string = f->nullable_list_nullable_list_nullable_string->emplace_back();
//		nullable_list_nullable_string.emplace();
//		nullable_list_nullable_string->emplace_back("qwert");
//		xx::CoutSN(f);
//		xx::Serializer s;
//		s.WriteRoot(f);
//		xx::Deserializer ds;
//		ds.SetData(s.GetData());
//		int r = ds.ReadRoot(f);
//		xx::CoutSN(f, r);
//	}






	//PKG::A a;
	//a.nullable_string = "asdf";
	//xx::CoutN("a = ", a);
	//auto b = std::move(a);
	//xx::CoutN("a = ", a);
	//xx::CoutN("b = ", b);
	//a = b;
	//xx::CoutN("a = ", a);




	//xx::SQLite::Connection c(std::string(argv[0]) + ".db3");
	//if (!c) {
	//	xx::CoutN("db open failed. code = ", c.lastErrorCode);
	//	return c.lastErrorCode;
	//}
	//auto&& DumpError = [&] {
	//	xx::CoutN("throw exception: code = ", c.lastErrorCode, ", message = ", c.lastErrorMessage);
	//};
	//if (int v; c.TryExecute("select null", v)) DumpError(); else xx::CoutN(v);
	//if (std::optional<int> v; c.TryExecute("select null", v)) DumpError(); else xx::CoutN(v);
	//if (double v; c.TryExecute("select 1.234", v)) DumpError(); else xx::CoutN(v);
	//if (std::string v; c.TryExecute("select 'asdf'", v)) DumpError(); else xx::CoutN(v);
	//if (std::optional<std::string> v; c.TryExecute("select 'qwer'", v)) DumpError(); else xx::CoutN(v);

	//auto&& Try = [&](std::function<void()>&& f) {
	//	try {
	//		f();
	//	}
	//	catch (int const& ec) {
	//		xx::CoutN("throw exception: code = ", c.lastErrorCode, ", message = ", c.lastErrorMessage);
	//		assert(ec == c.lastErrorCode);
	//	}
	//};
	//Try([&] { xx::CoutN(c.Execute<int>("select null")); });
	//Try([&] { xx::CoutN(c.Execute<std::optional<int>>("select null")); });
	//Try([&] { xx::CoutN(c.Execute<std::optional<std::string>>("select 'asdf'")); });


	//{
	//	xx::Serializer s;
	//	{
	//		PKG::NodeContainer nc;
	//		xx::MakeTo(nc.node);
	//		nc.node->parent = nc.node;
	//		xx::CoutSN(nc);
	//		s.WriteRoot(nc);
	//	}
	//	xx::Deserializer ds;
	//	ds.SetData(s.GetData());
	//	{
	//		PKG::NodeContainer o;
	//		int r = ds.ReadRoot(o);
	//		xx::CoutSN(o, r);
	//	}
	//}

	//{
	//	xx::Serializer s;
	//	{
	//		auto&& n = std::make_shared<PKG::Node>();
	//		n->parent = n;
	//		xx::CoutSN(n);
	//		s.WriteRoot(n);
	//	}
	//	xx::Deserializer ds;
	//	ds.SetData(s.GetData());
	//	{
	//		std::shared_ptr<xx::Object> o;
	//		int r = ds.ReadRoot(o);
	//		xx::CoutSN(o, r);
	//	}
	//}

	//{
	//	auto&& f = std::make_shared<PKG::Foo>();
	//	auto&& c0 = f->list_list_nullable_b.emplace_back();
	//	auto&& c0c0 = c0.emplace_back();
	//	auto&& b = c0c0.emplace();
	//	b._int = 123;
	//	b.nullable_data.emplace("asdf", 4);
	//	xx::CoutSN(f);
	//	xx::Serializer s;
	//	s.WriteRoot(f);
	//	xx::Deserializer ds;
	//	ds.SetData(s.GetData());
	//	int r = ds.ReadRoot(f);
	//	xx::CoutSN(f, r);
	//}

	//{
	//	xx::Serializer s;
	//	s.Write(1, 2, 3, 4, 5);
	//	auto&& d = s.GetData();
	//	s.Write(d, "abc123");
	//	d = s.GetData();
	//	xx::Deserializer ds;
	//	ds.SetData(d);
	//	int i1, i2, i3, i4, i5;
	//	std::string str;
	//	auto r = ds.Read(d, str);
	//	xx::CoutSN(r, d, str);
	//	ds.SetData(d);
	//	r = ds.Read(i1, i2, i3, i4, i5);
	//	xx::CoutSN(r, i1, i2, i3, i4, i5);
	//}

//	std::cin.get();
//	return 0;
//}

//{
//	xx::Data d;
//	d.WriteBuf("asdf", 4);
//	xx::Data d2;
//	d2 = std::move(d);
//	xx::Data d3(d2);
//	d3.SetReadonlyMode();
//	std::cout << d3.Refs() << std::endl;
//	{
//		auto d4 = d3;
//		std::cout << d3.Refs() << std::endl;
//		auto d5 = d4;
//		std::cout << d3.Refs() << std::endl;
//	}
//	std::cout << d3.Refs() << std::endl;
//}

//#include "PKG_class.h"
//
//// todo: 令 List 不再继承自 xx::Object. List, Serializer, Random 都属于 struct 值类型, 都走 SFuncs BFuncs IFuncs 模板适配
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
//	//xx::Serializer bb;
//	////PKG::TestNamespace::B b;
//	////b._float = 1.23f;
//	////b._pos = { 1.1f, 2.2f };
//	////bb.Write(b);
//	//xx::CoutN(bb);
//
//	std::cin.get();
//	return 0;
//}









//#include <iostream>
//#include "ajson.hpp"
//#define List vector
//using namespace std;
//
//struct Foo {
//	int i = 0;
//	List<string> ss;
//};
//AJSON(Foo, i, ss);
//
//struct Foos {
//	List<Foo> foos;
//};
//AJSON(Foos, foos);
//
//int main() {
//	Foos c;
//	ajson::load_from_buff(c, R"({
//	"foos":[
//	{
//		"i":123,
//		"ss":["asdf", "qwer"]
//	},
//	{
//		"i":345,
//		"ss":["zzzz", "xxxxxx"]
//	}]
//})");
//	for (auto&& foo : c.foos) {
//		std::cout << foo.i << std::endl;
//		for (auto&& s : foo.ss) {
//			std::cout << s << std::endl;
//		}
//	}
//	std::cin.get();
//	return 0;
//}