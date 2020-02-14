#pragma once
#include "xx_serializer.h"
#include"PKG_class.inc"

namespace PKG {
	struct PkgGenMd5 {
		inline static const std::string value = "#*MD5<0532f26bd513df873eb404458ab6afcd>*#";
    };
	struct AllTypesRegister {
        AllTypesRegister();
    };
    inline AllTypesRegister allTypesRegisterInstance;

namespace NS1 {
    // 测试传统值类型
    struct A;
}
    // 测试可空值类型
    struct A;
namespace NS3::NS4 {
    // 测试可空值类型数组
    struct A;
}
    // 测试可空可空值类型数组
    struct B;
    // 包含结构体 B 用于收发. 测试多层 List + Limit
    struct Foo;
    // 测试 Weak 递归引用
    struct Node;
    // 测试 Shared
    struct NodeContainer;
namespace NS1 {
    // 测试传统值类型
    struct A {
        uint8_t _byte = 0;
        int8_t _sbyte = 0;
        uint16_t _ushort = 0;
        int16_t _short = 0;
        uint32_t _uint = 0;
        int32_t _int = 0;
        uint64_t _ulong = 0;
        int64_t _long = 0;
        float _float = 0;
        double _double = 0;
        bool _bool = false;
        std::string _string;
        xx::Data _data;
        XX_CODEGEN_STRUCT_HEADER(A, void)
    };
}
    // 测试可空值类型
    struct A : PKG::NS1::A {
        std::optional<int32_t> nullable_int;
        std::optional<std::string> nullable_string;
        std::optional<xx::Data> nullable_data;
        XX_CODEGEN_STRUCT_HEADER(A, PKG::NS1::A)
    };
namespace NS3::NS4 {
    // 测试可空值类型数组
    struct A : PKG::A {
        std::vector<std::optional<int32_t>> list_nullable_int;
        std::vector<std::optional<std::string>> list_nullable_string;
        std::vector<std::optional<xx::Data>> list_nullable_data;
        XX_CODEGEN_STRUCT_HEADER(A, PKG::A)
    };
}
    // 测试可空可空值类型数组
    struct B : PKG::NS3::NS4::A {
        std::optional<std::vector<std::optional<int32_t>>> nullable_list_nullable_int;
        std::optional<std::vector<std::optional<std::string>>> nullable_list_nullable_string;
        std::optional<std::vector<std::optional<xx::Data>>> nullable_list_nullable_data;
        XX_CODEGEN_STRUCT_HEADER(B, PKG::NS3::NS4::A)
    };
    // 包含结构体 B 用于收发. 测试多层 List + Limit
    struct Foo : ::xx::Object {
        std::vector<std::vector<std::optional<PKG::B>>> bs;
        XX_CODEGEN_CLASS_HEADER(Foo, ::xx::Object)
    };
    // 测试 Weak 递归引用
    struct Node : ::xx::Object {
        std::weak_ptr<PKG::Node> parent;
        XX_CODEGEN_CLASS_HEADER(Node, ::xx::Object)
    };
    // 测试 Shared
    struct NodeContainer : ::xx::Object {
        std::shared_ptr<PKG::Node> node;
        XX_CODEGEN_CLASS_HEADER(NodeContainer, ::xx::Object)
    };
}
namespace xx {
	template<>
	struct BFuncs<PKG::NS1::A, void> {
		static void Serialize(Serializer& bb, PKG::NS1::A const& in) noexcept;
		static int Deserialize(Deserializer& bb, PKG::NS1::A& out) noexcept;
	};
	template<>
	struct SFuncs<PKG::NS1::A, void> {
		static void Append(std::string& s, PKG::NS1::A const& in) noexcept;
		static void AppendCore(std::string& s, PKG::NS1::A const& in) noexcept;
    };
	template<>
    struct CFuncs<PKG::NS1::A, void> {
		static int Cascade(void* const& o, PKG::NS1::A const& in) noexcept;
		static int CascadeCore(void* const& o, PKG::NS1::A const& in) noexcept;
    };
	template<>
	struct BFuncs<PKG::A, void> {
		static void Serialize(Serializer& bb, PKG::A const& in) noexcept;
		static int Deserialize(Deserializer& bb, PKG::A& out) noexcept;
	};
	template<>
	struct SFuncs<PKG::A, void> {
		static void Append(std::string& s, PKG::A const& in) noexcept;
		static void AppendCore(std::string& s, PKG::A const& in) noexcept;
    };
	template<>
    struct CFuncs<PKG::A, void> {
		static int Cascade(void* const& o, PKG::A const& in) noexcept;
		static int CascadeCore(void* const& o, PKG::A const& in) noexcept;
    };
	template<>
	struct BFuncs<PKG::NS3::NS4::A, void> {
		static void Serialize(Serializer& bb, PKG::NS3::NS4::A const& in) noexcept;
		static int Deserialize(Deserializer& bb, PKG::NS3::NS4::A& out) noexcept;
	};
	template<>
	struct SFuncs<PKG::NS3::NS4::A, void> {
		static void Append(std::string& s, PKG::NS3::NS4::A const& in) noexcept;
		static void AppendCore(std::string& s, PKG::NS3::NS4::A const& in) noexcept;
    };
	template<>
    struct CFuncs<PKG::NS3::NS4::A, void> {
		static int Cascade(void* const& o, PKG::NS3::NS4::A const& in) noexcept;
		static int CascadeCore(void* const& o, PKG::NS3::NS4::A const& in) noexcept;
    };
	template<>
	struct BFuncs<PKG::B, void> {
		static void Serialize(Serializer& bb, PKG::B const& in) noexcept;
		static int Deserialize(Deserializer& bb, PKG::B& out) noexcept;
	};
	template<>
	struct SFuncs<PKG::B, void> {
		static void Append(std::string& s, PKG::B const& in) noexcept;
		static void AppendCore(std::string& s, PKG::B const& in) noexcept;
    };
	template<>
    struct CFuncs<PKG::B, void> {
		static int Cascade(void* const& o, PKG::B const& in) noexcept;
		static int CascadeCore(void* const& o, PKG::B const& in) noexcept;
    };
    template<> struct TypeId<PKG::Foo> { static const uint16_t value = 13002; };
    template<> struct TypeId<PKG::Node> { static const uint16_t value = 13001; };
    template<> struct TypeId<PKG::NodeContainer> { static const uint16_t value = 13003; };
}
#include "PKG_class_end.inc"