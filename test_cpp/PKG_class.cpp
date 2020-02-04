#include "PKG_class.h"
#ifdef NEED_INCLUDE_PKG_class_hpp
#include "PKG_class.hpp"
#endif


namespace xx {
	void BFuncs<PKG::NS1::A, void>::Serialize(Serializer& bb, PKG::NS1::A const& in) noexcept {
        bb.Write(in._byte);
        bb.Write(in._sbyte);
        bb.Write(in._ushort);
        bb.Write(in._short);
        bb.Write(in._uint);
        bb.Write(in._int);
        bb.Write(in._ulong);
        bb.Write(in._long);
        bb.Write(in._float);
        bb.Write(in._double);
        bb.Write(in._bool);
        bb.Write(in._string);
        bb.Write(in._data);
    }
	int BFuncs<PKG::NS1::A, void>::Deserialize(Deserializer& bb, PKG::NS1::A& out) noexcept {
        if (int r = bb.Read(out._byte)) return r;
        if (int r = bb.Read(out._sbyte)) return r;
        if (int r = bb.Read(out._ushort)) return r;
        if (int r = bb.Read(out._short)) return r;
        if (int r = bb.Read(out._uint)) return r;
        if (int r = bb.Read(out._int)) return r;
        if (int r = bb.Read(out._ulong)) return r;
        if (int r = bb.Read(out._long)) return r;
        if (int r = bb.Read(out._float)) return r;
        if (int r = bb.Read(out._double)) return r;
        if (int r = bb.Read(out._bool)) return r;
        if (int r = bb.ReadLimit<16>(out._string)) return r;
        if (int r = bb.ReadLimit<32>(out._data)) return r;
        return 0;
    }
	void SFuncs<PKG::NS1::A, void>::Append(std::string& s, PKG::NS1::A const& in) noexcept {
        xx::Append(s, "{ \"structTypeName\":\"NS1.A\"");
        AppendCore(s, in);
        xx::Append(s, " }");
    }
	void SFuncs<PKG::NS1::A, void>::AppendCore(std::string& s, PKG::NS1::A const& in) noexcept {
        xx::Append(s, "\"_byte\" : \"", in._byte, "\"");
        xx::Append(s, "\"_sbyte\" : \"", in._sbyte, "\"");
        xx::Append(s, "\"_ushort\" : \"", in._ushort, "\"");
        xx::Append(s, "\"_short\" : \"", in._short, "\"");
        xx::Append(s, "\"_uint\" : \"", in._uint, "\"");
        xx::Append(s, "\"_int\" : \"", in._int, "\"");
        xx::Append(s, "\"_ulong\" : \"", in._ulong, "\"");
        xx::Append(s, "\"_long\" : \"", in._long, "\"");
        xx::Append(s, "\"_float\" : \"", in._float, "\"");
        xx::Append(s, "\"_double\" : \"", in._double, "\"");
        xx::Append(s, "\"_bool\" : \"", in._bool, "\"");
        xx::Append(s, "\"_string\" : \"", in._string, "\"");
        xx::Append(s, "\"_data\" : \"", in._data, "\"");
    }
#ifndef CUSTOM_INITCASCADE_PKG_NS1_A
	int CFuncs<PKG::NS1::A, void>::Cascade(void* const& o, PKG::NS1::A const& in) noexcept {
        return CascadeCore(o, in);
    }
#endif
    int CFuncs<PKG::NS1::A, void>::CascadeCore(void* const& o, PKG::NS1::A const& in) noexcept {
        return 0;
    }
	void BFuncs<PKG::A, void>::Serialize(Serializer& bb, PKG::A const& in) noexcept {
        BFuncs<PKG::NS1::A>::Serialize(bb, in);
        bb.Write(in.nullable_int);
        bb.Write(in.nullable_string);
        bb.Write(in.nullable_data);
    }
	int BFuncs<PKG::A, void>::Deserialize(Deserializer& bb, PKG::A& out) noexcept {
        if (int r = BFuncs<PKG::NS1::A>::Deserialize(bb, out)) return r;
        if (int r = bb.Read(out.nullable_int)) return r;
        if (int r = bb.Read(out.nullable_string)) return r;
        if (int r = bb.Read(out.nullable_data)) return r;
        return 0;
    }
	void SFuncs<PKG::A, void>::Append(std::string& s, PKG::A const& in) noexcept {
        xx::Append(s, "{ \"structTypeName\":\"A\"");
        AppendCore(s, in);
        xx::Append(s, " }");
    }
	void SFuncs<PKG::A, void>::AppendCore(std::string& s, PKG::A const& in) noexcept {
        SFuncs<PKG::NS1::A>::AppendCore(s, in);
        xx::Append(s, "\"nullable_int\" : \"", in.nullable_int, "\"");
        xx::Append(s, "\"nullable_string\" : \"", in.nullable_string, "\"");
        xx::Append(s, "\"nullable_data\" : \"", in.nullable_data, "\"");
    }
#ifndef CUSTOM_INITCASCADE_PKG_A
	int CFuncs<PKG::A, void>::Cascade(void* const& o, PKG::A const& in) noexcept {
        return CascadeCore(o, in);
    }
#endif
    int CFuncs<PKG::A, void>::CascadeCore(void* const& o, PKG::A const& in) noexcept {
        if (int r = CFuncs<PKG::NS1::A>::CascadeCore(o, in)) return r;
        return 0;
    }
	void BFuncs<PKG::NS3::NS4::A, void>::Serialize(Serializer& bb, PKG::NS3::NS4::A const& in) noexcept {
        BFuncs<PKG::A>::Serialize(bb, in);
        bb.Write(in.list_nullable_int);
        bb.Write(in.list_nullable_string);
        bb.Write(in.list_nullable_data);
    }
	int BFuncs<PKG::NS3::NS4::A, void>::Deserialize(Deserializer& bb, PKG::NS3::NS4::A& out) noexcept {
        if (int r = BFuncs<PKG::A>::Deserialize(bb, out)) return r;
        if (int r = bb.Read(out.list_nullable_int)) return r;
        if (int r = bb.Read(out.list_nullable_string)) return r;
        if (int r = bb.Read(out.list_nullable_data)) return r;
        return 0;
    }
	void SFuncs<PKG::NS3::NS4::A, void>::Append(std::string& s, PKG::NS3::NS4::A const& in) noexcept {
        xx::Append(s, "{ \"structTypeName\":\"NS3.NS4.A\"");
        AppendCore(s, in);
        xx::Append(s, " }");
    }
	void SFuncs<PKG::NS3::NS4::A, void>::AppendCore(std::string& s, PKG::NS3::NS4::A const& in) noexcept {
        SFuncs<PKG::A>::AppendCore(s, in);
        xx::Append(s, "\"list_nullable_int\" : \"", in.list_nullable_int, "\"");
        xx::Append(s, "\"list_nullable_string\" : \"", in.list_nullable_string, "\"");
        xx::Append(s, "\"list_nullable_data\" : \"", in.list_nullable_data, "\"");
    }
#ifndef CUSTOM_INITCASCADE_PKG_NS3_NS4_A
	int CFuncs<PKG::NS3::NS4::A, void>::Cascade(void* const& o, PKG::NS3::NS4::A const& in) noexcept {
        return CascadeCore(o, in);
    }
#endif
    int CFuncs<PKG::NS3::NS4::A, void>::CascadeCore(void* const& o, PKG::NS3::NS4::A const& in) noexcept {
        if (int r = CFuncs<PKG::A>::CascadeCore(o, in)) return r;
        return 0;
    }
	void BFuncs<PKG::B, void>::Serialize(Serializer& bb, PKG::B const& in) noexcept {
        BFuncs<PKG::NS3::NS4::A>::Serialize(bb, in);
        bb.Write(in._int);
        bb.Write(in.nullable_list_nullable_string);
        bb.Write(in.nullable_list_nullable_data);
    }
	int BFuncs<PKG::B, void>::Deserialize(Deserializer& bb, PKG::B& out) noexcept {
        if (int r = BFuncs<PKG::NS3::NS4::A>::Deserialize(bb, out)) return r;
        if (int r = bb.Read(out._int)) return r;
        if (int r = bb.Read(out.nullable_list_nullable_string)) return r;
        if (int r = bb.Read(out.nullable_list_nullable_data)) return r;
        return 0;
    }
	void SFuncs<PKG::B, void>::Append(std::string& s, PKG::B const& in) noexcept {
        xx::Append(s, "{ \"structTypeName\":\"B\"");
        AppendCore(s, in);
        xx::Append(s, " }");
    }
	void SFuncs<PKG::B, void>::AppendCore(std::string& s, PKG::B const& in) noexcept {
        SFuncs<PKG::NS3::NS4::A>::AppendCore(s, in);
        xx::Append(s, "\"_int\" : \"", in._int, "\"");
        xx::Append(s, "\"nullable_list_nullable_string\" : \"", in.nullable_list_nullable_string, "\"");
        xx::Append(s, "\"nullable_list_nullable_data\" : \"", in.nullable_list_nullable_data, "\"");
    }
#ifndef CUSTOM_INITCASCADE_PKG_B
	int CFuncs<PKG::B, void>::Cascade(void* const& o, PKG::B const& in) noexcept {
        return CascadeCore(o, in);
    }
#endif
    int CFuncs<PKG::B, void>::CascadeCore(void* const& o, PKG::B const& in) noexcept {
        if (int r = CFuncs<PKG::NS3::NS4::A>::CascadeCore(o, in)) return r;
        return 0;
    }
}
namespace PKG {
    uint16_t Foo::GetTypeId() const noexcept {
        return 13002;
    }
    void Foo::Serialize(xx::Serializer& bb) const noexcept {
        bb.Write(this->bs);
    }
    int Foo::Deserialize(xx::Deserializer& bb) noexcept {
        if (int r = bb.ReadLimit<3, 1>(this->bs)) return r;
        return 0;
    }
    void Foo::ToString(std::string& s) const noexcept {
        if (this->toStringFlag)
        {
        	xx::Append(s, "[ \"***** recursived *****\" ]");
        	return;
        }
        else this->SetToStringFlag();

        xx::Append(s, "{ \"structTypeName\":\"Foo\", \"structTypeId\":", GetTypeId());
        ToStringCore(s);
        xx::Append(s, " }");
        
        this->SetToStringFlag(false);
    }
    void Foo::ToStringCore(std::string& s) const noexcept {
        xx::Append(s, "\"bs\" : \"", this->bs, "\"");
    }
#ifndef CUSTOM_INITCASCADE_PKG_Foo
    int Foo::Cascade(void* const& o) noexcept {
        return this->CascadeCore(o);
    }
#endif
    int Foo::CascadeCore(void* const& o) noexcept {
        return 0;
    }
    uint16_t Node::GetTypeId() const noexcept {
        return 13001;
    }
    void Node::Serialize(xx::Serializer& bb) const noexcept {
        bb.Write(this->parent);
    }
    int Node::Deserialize(xx::Deserializer& bb) noexcept {
        if (int r = bb.Read(this->parent)) return r;
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
        xx::Append(s, "\"parent\" : \"", this->parent, "\"");
    }
#ifndef CUSTOM_INITCASCADE_PKG_Node
    int Node::Cascade(void* const& o) noexcept {
        return this->CascadeCore(o);
    }
#endif
    int Node::CascadeCore(void* const& o) noexcept {
        return 0;
    }
    uint16_t NodeContainer::GetTypeId() const noexcept {
        return 13003;
    }
    void NodeContainer::Serialize(xx::Serializer& bb) const noexcept {
        bb.Write(this->node);
        bb.Write(this->foo);
    }
    int NodeContainer::Deserialize(xx::Deserializer& bb) noexcept {
        if (int r = bb.Read(this->node)) return r;
        if (int r = bb.Read(this->foo)) return r;
        return 0;
    }
    void NodeContainer::ToString(std::string& s) const noexcept {
        if (this->toStringFlag)
        {
        	xx::Append(s, "[ \"***** recursived *****\" ]");
        	return;
        }
        else this->SetToStringFlag();

        xx::Append(s, "{ \"structTypeName\":\"NodeContainer\", \"structTypeId\":", GetTypeId());
        ToStringCore(s);
        xx::Append(s, " }");
        
        this->SetToStringFlag(false);
    }
    void NodeContainer::ToStringCore(std::string& s) const noexcept {
        xx::Append(s, "\"node\" : \"", this->node, "\"");
        xx::Append(s, "\"foo\" : \"", this->foo, "\"");
    }
#ifndef CUSTOM_INITCASCADE_PKG_NodeContainer
    int NodeContainer::Cascade(void* const& o) noexcept {
        return this->CascadeCore(o);
    }
#endif
    int NodeContainer::CascadeCore(void* const& o) noexcept {
        return 0;
    }
}
namespace PKG {
	AllTypesRegister::AllTypesRegister() {
	    xx::Deserializer::Register<PKG::Foo>(13002);
	    xx::Deserializer::Register<PKG::Node>(13001);
	    xx::Deserializer::Register<PKG::NodeContainer>(13003);
	}
}
