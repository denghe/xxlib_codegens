#include "PKG_class.h"

namespace xx {
	void BFuncs<::PKG::TestNamespace::A, void>::Write(BBuffer& bb, ::PKG::TestNamespace::A const& in) noexcept {
        bb.Write(in._pos);
    }
	int BFuncs<::PKG::TestNamespace::A, void>::Read(BBuffer& bb, ::PKG::TestNamespace::A& out) noexcept {
        if (int r = bb.Read(out._pos)) return r;
        return 0;
    }
	void SFuncs<::PKG::TestNamespace::A, void>::Append(std::string& s, ::PKG::TestNamespace::A const& in) noexcept {
        xx::Append(s, "{ \"structTypeName\":\"TestNamespace.A\"");
        AppendCore(s, in);
        xx::Append(s, " }");
    }
	void SFuncs<::PKG::TestNamespace::A, void>::AppendCore(std::string& s, ::PKG::TestNamespace::A const& in) noexcept {
        xx::Append(s, "\"_pos\" : \"", in._pos, "\"");
    }
#ifndef CUSTOM_INITCASCADE_PKG_TestNamespace_A
	int IFuncs<::PKG::TestNamespace::A, void>::InitCascade(void* const& o, ::PKG::TestNamespace::A const& in) noexcept {
        return InitCascadeCore(o, in);
    }
#endif
    int IFuncs<::PKG::TestNamespace::A, void>::InitCascadeCore(void* const& o, ::PKG::TestNamespace::A const& in) noexcept {
        return 0;
    }
	void BFuncs<::PKG::TestNamespace::B, void>::Write(BBuffer& bb, ::PKG::TestNamespace::B const& in) noexcept {
        BFuncs<::PKG::TestNamespace::A>::Write(bb, in);
        bb.Write(in._float);
    }
	int BFuncs<::PKG::TestNamespace::B, void>::Read(BBuffer& bb, ::PKG::TestNamespace::B& out) noexcept {
        if (int r = BFuncs<::PKG::TestNamespace::A>::Read(bb, out)) return r;
        if (int r = bb.Read(out._float)) return r;
        return 0;
    }
	void SFuncs<::PKG::TestNamespace::B, void>::Append(std::string& s, ::PKG::TestNamespace::B const& in) noexcept {
        xx::Append(s, "{ \"structTypeName\":\"TestNamespace.B\"");
        AppendCore(s, in);
        xx::Append(s, " }");
    }
	void SFuncs<::PKG::TestNamespace::B, void>::AppendCore(std::string& s, ::PKG::TestNamespace::B const& in) noexcept {
        SFuncs<::PKG::TestNamespace::A>::AppendCore(s, in);
        xx::Append(s, "\"_float\" : \"", in._float, "\"");
    }
#ifndef CUSTOM_INITCASCADE_PKG_TestNamespace_B
	int IFuncs<::PKG::TestNamespace::B, void>::InitCascade(void* const& o, ::PKG::TestNamespace::B const& in) noexcept {
        return InitCascadeCore(o, in);
    }
#endif
    int IFuncs<::PKG::TestNamespace::B, void>::InitCascadeCore(void* const& o, ::PKG::TestNamespace::B const& in) noexcept {
        if (int r = IFuncs<::PKG::TestNamespace::A>::InitCascadeCore(o, in)) return r;
        return 0;
    }
}
namespace PKG {
namespace TestNamespace {
    uint16_t Foo1::GetTypeId() const noexcept {
        return 13001;
    }
    void Foo1::ToBBuffer(xx::BBuffer& bb) const noexcept {
        bb.Write(this->_byte);
        bb.Write(this->_sbyte);
        bb.Write(this->_ushort);
        bb.Write(this->_short);
        bb.Write(this->_uint);
        bb.Write(this->_int);
        bb.Write(this->_ulong);
        bb.Write(this->_long);
        bb.Write(this->_float);
        bb.Write(this->_double);
        bb.Write(this->_random);
        bb.Write(this->_pos);
    }
    int Foo1::FromBBuffer(xx::BBuffer& bb) noexcept {
        if (int r = bb.Read(this->_byte)) return r;
        if (int r = bb.Read(this->_sbyte)) return r;
        if (int r = bb.Read(this->_ushort)) return r;
        if (int r = bb.Read(this->_short)) return r;
        if (int r = bb.Read(this->_uint)) return r;
        if (int r = bb.Read(this->_int)) return r;
        if (int r = bb.Read(this->_ulong)) return r;
        if (int r = bb.Read(this->_long)) return r;
        if (int r = bb.Read(this->_float)) return r;
        if (int r = bb.Read(this->_double)) return r;
        if (int r = bb.Read(this->_random)) return r;
        if (int r = bb.Read(this->_pos)) return r;
        return 0;
    }
    void Foo1::ToString(std::string& s) const noexcept {
        if (this->toStringFlag)
        {
        	xx::Append(s, "[ \"***** recursived *****\" ]");
        	return;
        }
        else this->SetToStringFlag();

        xx::Append(s, "{ \"structTypeName\":\"TestNamespace.Foo1\", \"structTypeId\":", GetTypeId());
        ToStringCore(s);
        xx::Append(s, " }");
        
        this->SetToStringFlag(false);
    }
    void Foo1::ToStringCore(std::string& s) const noexcept {
        xx::Append(s, "\"_byte\" : \"", this->_byte, "\"");
        xx::Append(s, "\"_sbyte\" : \"", this->_sbyte, "\"");
        xx::Append(s, "\"_ushort\" : \"", this->_ushort, "\"");
        xx::Append(s, "\"_short\" : \"", this->_short, "\"");
        xx::Append(s, "\"_uint\" : \"", this->_uint, "\"");
        xx::Append(s, "\"_int\" : \"", this->_int, "\"");
        xx::Append(s, "\"_ulong\" : \"", this->_ulong, "\"");
        xx::Append(s, "\"_long\" : \"", this->_long, "\"");
        xx::Append(s, "\"_float\" : \"", this->_float, "\"");
        xx::Append(s, "\"_double\" : \"", this->_double, "\"");
        xx::Append(s, "\"_random\" : \"", this->_random, "\"");
        xx::Append(s, "\"_pos\" : \"", this->_pos, "\"");
    }
#ifndef CUSTOM_INITCASCADE_PKG_TestNamespace_Foo1
    int Foo1::InitCascade(void* const& o) noexcept {
        return this->InitCascadeCore(o);
    }
#endif
    int Foo1::InitCascadeCore(void* const& o) noexcept {
        return 0;
    }
}
    uint16_t Bar::GetTypeId() const noexcept {
        return 13002;
    }
    void Bar::ToBBuffer(xx::BBuffer& bb) const noexcept {
        bb.Write(this->foo1s_v);
        bb.Write(this->foo1s_s);
        bb.Write(this->bs_v);
    }
    int Bar::FromBBuffer(xx::BBuffer& bb) noexcept {
        bb.readLengthLimit = 0;
        if (int r = bb.Read(this->foo1s_v)) return r;
        bb.readLengthLimit = 0;
        if (int r = bb.Read(this->foo1s_s)) return r;
        bb.readLengthLimit = 0;
        if (int r = bb.Read(this->bs_v)) return r;
        return 0;
    }
    void Bar::ToString(std::string& s) const noexcept {
        if (this->toStringFlag)
        {
        	xx::Append(s, "[ \"***** recursived *****\" ]");
        	return;
        }
        else this->SetToStringFlag();

        xx::Append(s, "{ \"structTypeName\":\"Bar\", \"structTypeId\":", GetTypeId());
        ToStringCore(s);
        xx::Append(s, " }");
        
        this->SetToStringFlag(false);
    }
    void Bar::ToStringCore(std::string& s) const noexcept {
        xx::Append(s, "\"foo1s_v\" : \"", this->foo1s_v, "\"");
        xx::Append(s, "\"foo1s_s\" : \"", this->foo1s_s, "\"");
        xx::Append(s, "\"bs_v\" : \"", this->bs_v, "\"");
    }
#ifndef CUSTOM_INITCASCADE_PKG_Bar
    int Bar::InitCascade(void* const& o) noexcept {
        return this->InitCascadeCore(o);
    }
#endif
    int Bar::InitCascadeCore(void* const& o) noexcept {
        if (int r = xx::InitCascade(o, this->foo1s_v)) return r;
        return 0;
    }
    uint16_t Node::GetTypeId() const noexcept {
        return 13003;
    }
    void Node::ToBBuffer(xx::BBuffer& bb) const noexcept {
        bb.Write(this->node_s);
        bb.Write(this->node_w);
    }
    int Node::FromBBuffer(xx::BBuffer& bb) noexcept {
        if (int r = bb.Read(this->node_s)) return r;
        if (int r = bb.Read(this->node_w)) return r;
        return 0;
    }
    void Node::ToString(std::string& s) const noexcept {
        if (this->toStringFlag)
        {
        	xx::Append(s, "[ \"***** recursived *****\" ]");
        	return;
        }
        else this->SetToStringFlag();

        xx::Append(s, "{ \"structTypeName\":\"Node\", \"structTypeId\":", GetTypeId());
        ToStringCore(s);
        xx::Append(s, " }");
        
        this->SetToStringFlag(false);
    }
    void Node::ToStringCore(std::string& s) const noexcept {
        xx::Append(s, "\"node_s\" : \"", this->node_s, "\"");
        xx::Append(s, "\"node_w\" : \"", this->node_w, "\"");
    }
#ifndef CUSTOM_INITCASCADE_PKG_Node
    int Node::InitCascade(void* const& o) noexcept {
        return this->InitCascadeCore(o);
    }
#endif
    int Node::InitCascadeCore(void* const& o) noexcept {
        return 0;
    }
}
namespace PKG {
	AllTypesRegister::AllTypesRegister() {
	    xx::BBuffer::Register<::PKG::TestNamespace::Foo1>(13001);
	    xx::BBuffer::Register<::PKG::Bar>(13002);
	    xx::BBuffer::Register<::PKG::Node>(13003);
	}
}
