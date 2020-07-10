#include "PKG_class_lite.h"
#include "PKG_class_lite.cpp.inc"
namespace PKG {
    A::A(A&& o) {
        this->operator=(std::move(o));
    }
    A& A::operator=(A&& o) {
        std::swap(this->x, o.x);
        std::swap(this->y, o.y);
        std::swap(this->c, o.c);
        return *this;
    }
    B::B(B&& o) {
        this->operator=(std::move(o));
    }
    B& B::operator=(B&& o) {
        this->PKG::A::operator=(std::move(o));
        std::swap(this->z, o.z);
        std::swap(this->wc, o.wc);
        return *this;
    }
    C::C(C&& o) {
        this->operator=(std::move(o));
    }
    C& C::operator=(C&& o) {
        std::swap(this->a, o.a);
        std::swap(this->b, o.b);
        return *this;
    }
    uint16_t C::GetTypeId() const {
        return xx::TypeId_v<PKG::C>;
    }
    void C::Serialize(xx::DataWriterEx& dw) const {
        dw.Write(this->a);
        dw.Write(this->b);
    }
    int C::Deserialize(xx::DataReaderEx& dr) {
        if (int r = dr.Read(this->a)) return r;
        if (int r = dr.Read(this->b)) return r;
        return 0;
    }
    void C::ToString(std::string& s) const {
        if (this->toStringFlag) {
        	xx::Append(s, "[\"***** recursived *****\"]");
        	return;
        }
        else {
            ((C*)this)->toStringFlag = true;
        }
        xx::Append(s, "{\"structTypeId\":", GetTypeId());
        ToStringCore(s);
        s.push_back('}');
        ((C*)this)->toStringFlag = false;
    }
    void C::ToStringCore(std::string& s) const {
        xx::Append(s, ",\"a\":", this->a);
        xx::Append(s, ",\"b\":", this->b);
    }
    D::D(D&& o) {
        this->operator=(std::move(o));
    }
    D& D::operator=(D&& o) {
        this->PKG::C::operator=(std::move(o));
        std::swap(this->name, o.name);
        return *this;
    }
    uint16_t D::GetTypeId() const {
        return xx::TypeId_v<PKG::D>;
    }
    void D::Serialize(xx::DataWriterEx& dw) const {
        this->BaseType::Serialize(dw);
        dw.Write(this->name);
    }
    int D::Deserialize(xx::DataReaderEx& dr) {
        if (int r = this->BaseType::Deserialize(dr)) return r;
        if (int r = dr.Read(this->name)) return r;
        return 0;
    }
    void D::ToString(std::string& s) const {
        if (this->toStringFlag) {
        	xx::Append(s, "[\"***** recursived *****\"]");
        	return;
        }
        else {
            ((D*)this)->toStringFlag = true;
        }
        xx::Append(s, "{\"structTypeId\":", GetTypeId());
        ToStringCore(s);
        s.push_back('}');
        ((D*)this)->toStringFlag = false;
    }
    void D::ToStringCore(std::string& s) const {
        this->BaseType::ToStringCore(s);
        xx::Append(s, ",\"name\":", this->name);
    }
}
namespace xx {
	void DataFuncsEx<PKG::A, void>::Write(DataWriter& dw, PKG::A const& in) {
        dw.Write(in.x);
        dw.Write(in.y);
        dw.Write(in.c);
    }
	int DataFuncsEx<PKG::A, void>::Read(DataReader& d, PKG::A& out) {
        if (int r = d.Read(out.x)) return r;
        if (int r = d.Read(out.y)) return r;
        if (int r = d.Read(out.c)) return r;
        return 0;
    }
	void DataFuncsEx<PKG::B, void>::Write(DataWriter& dw, PKG::B const& in) {
        DataFuncsEx<PKG::A>::Write(dw, in);
        dw.Write(in.z);
        dw.Write(in.wc);
    }
	int DataFuncsEx<PKG::B, void>::Read(DataReader& d, PKG::B& out) {
        if (int r = DataFuncsEx<PKG::A>::Read(d, out)) return r;
        if (int r = d.Read(out.z)) return r;
        if (int r = d.Read(out.wc)) return r;
        return 0;
    }
}
