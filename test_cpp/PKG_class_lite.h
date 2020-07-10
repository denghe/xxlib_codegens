#pragma once
#include "xx_object.h"
#include "PKG_class_lite.h.inc"  // user create it for extend include files
namespace PKG {
	struct PkgGenMd5 {
		inline static const std::string value = "#*MD5<65502dc9d3b31ea59b7193aec8366d8e>*#";
    };

    struct C;
    struct D;
    struct A {
        XX_GENCODE_STRUCT_H(A)
        int32_t x = 0;
        int32_t y = 0;
        std::shared_ptr<PKG::C> c;
    };
    struct B : PKG::A {
        XX_GENCODE_STRUCT_H(B)
        int32_t z = 0;
        std::weak_ptr<PKG::C> wc;
    };
    struct C : xx::Object {
        XX_GENCODE_OBJECT_H(C, xx::Object)
        PKG::A a;
        PKG::B b;
    };
    struct D : PKG::C {
        XX_GENCODE_OBJECT_H(D, PKG::C)
        std::string name;
    };
}
namespace xx {
	template<>
	struct DataFuncsEx<PKG::A, void> {
		static void Write(DataWriter& dw, PKG::A const& in);
		static int Read(DataReader& dr, PKG::A& out);
	};
	template<>
	struct DataFuncsEx<PKG::B, void> {
		static void Write(DataWriter& dw, PKG::B const& in);
		static int Read(DataReader& dr, PKG::B& out);
	};
}