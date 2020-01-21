#pragma once
#include"PKG_class.inc"

namespace PKG {
	struct PkgGenMd5 {
		inline static const std::string value = "#*MD5<c08b4d136757ae33561c395c4e7619fa>*#";
    };
	struct AllTypesRegister {
        AllTypesRegister();
    };
    inline AllTypesRegister allTypesRegisterInstance;

namespace TestNamespace {
    struct A;
    struct Foo1;
    using Foo1_s = std::shared_ptr<Foo1>;
    using Foo1_w = std::weak_ptr<Foo1>;

    struct B;
}
    struct Bar;
    using Bar_s = std::shared_ptr<Bar>;
    using Bar_w = std::weak_ptr<Bar>;

    struct Node;
    using Node_s = std::shared_ptr<Node>;
    using Node_w = std::weak_ptr<Node>;

namespace TestNamespace {
    struct A {
        std::optional<::xx::Pos> _pos;
    };
    struct Foo1 : ::xx::Object {
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
        ::xx::Random_s _random = nullptr;
        ::xx::Pos _pos;
        XX_CODEGEN_CLASS_HEADER(Foo1, ::xx::Object)
    };
    struct B : ::PKG::TestNamespace::A {
        std::optional<float> _float;
    };
}
    struct Bar : ::xx::Object {
        std::vector<::PKG::TestNamespace::Foo1_s> foo1s_v;
        std::vector<std::shared_ptr<::PKG::TestNamespace::Foo1>> foo1s_s;
        std::vector<::PKG::TestNamespace::B> bs_v;
        XX_CODEGEN_CLASS_HEADER(Bar, ::xx::Object)
    };
    struct Node : ::xx::Object {
        std::shared_ptr<::PKG::Node> node_s;
        std::weak_ptr<::PKG::Node> node_w;
        XX_CODEGEN_CLASS_HEADER(Node, ::xx::Object)
    };
}
namespace xx {
	template<>
	struct BFuncs<::PKG::TestNamespace::A, void> {
		static void Write(BBuffer& bb, ::PKG::TestNamespace::A const& in) noexcept;
		static int Read(BBuffer& bb, ::PKG::TestNamespace::A& out) noexcept;
	};
	template<>
	struct SFuncs<::PKG::TestNamespace::A, void> {
		static inline void Append(std::string& s, ::PKG::TestNamespace::A const& in) noexcept;
		static inline void AppendCore(std::string& s, ::PKG::TestNamespace::A const& in) noexcept;
    };
	template<>
    struct IFuncs<::PKG::TestNamespace::A, void> {
		static inline int InitCascade(void* const& o, ::PKG::TestNamespace::A const& in) noexcept;
		static inline int InitCascadeCore(void* const& o, ::PKG::TestNamespace::A const& in) noexcept;
    };
	template<>
	struct BFuncs<::PKG::TestNamespace::B, void> {
		static void Write(BBuffer& bb, ::PKG::TestNamespace::B const& in) noexcept;
		static int Read(BBuffer& bb, ::PKG::TestNamespace::B& out) noexcept;
	};
	template<>
	struct SFuncs<::PKG::TestNamespace::B, void> {
		static inline void Append(std::string& s, ::PKG::TestNamespace::B const& in) noexcept;
		static inline void AppendCore(std::string& s, ::PKG::TestNamespace::B const& in) noexcept;
    };
	template<>
    struct IFuncs<::PKG::TestNamespace::B, void> {
		static inline int InitCascade(void* const& o, ::PKG::TestNamespace::B const& in) noexcept;
		static inline int InitCascadeCore(void* const& o, ::PKG::TestNamespace::B const& in) noexcept;
    };
    template<> struct TypeId<::PKG::TestNamespace::Foo1> { static const uint16_t value = 13001; };
    template<> struct TypeId<::PKG::Bar> { static const uint16_t value = 13002; };
    template<> struct TypeId<::PKG::Node> { static const uint16_t value = 13003; };
}
#include "xx_random.hpp"