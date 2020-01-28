using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Collections.Generic;

public static class GenCPP_Class
{
    static void GenH_Include(this StringBuilder sb, string templateName)
    {
        sb.Append(@"#pragma once
#include""" + templateName + @"_class.inc""
");
    }
    static void GenH_IncludeEnd(this StringBuilder sb, List<Type> cs, string templateName, TemplateLibrary.Filter<TemplateLibrary.CppFilter> filter)
    {
        sb.Append(@"
#include """ + templateName + "_class_end" + (filter != null ? "_filter" : "") + ".inc" + @"""");
    }

    static void GenH_RootNamespace(this StringBuilder sb, string templateName)
    {
        sb.Append(@"
namespace " + templateName + @" {");
    }

    static void GenH_RootNamespaceEnd(this StringBuilder sb)
    {
        sb.Append(@"
}");
    }

    static void GenH_PkgGenMd5(this StringBuilder sb, string templateName)
    {
        sb.Append(@"
	struct PkgGenMd5 {
		inline static const std::string value = """ + StringHelpers.MD5PlaceHolder + @""";
    };
	struct AllTypesRegister {
        AllTypesRegister();
    };
    inline AllTypesRegister allTypesRegisterInstance;
");
    }

    static void GenH_Predeclare(this StringBuilder sb, List<Type> cs, TemplateLibrary.Filter<TemplateLibrary.CppFilter> filter)
    {
        for (int i = 0; i < cs.Count; ++i)
        {
            var c = cs[i];
            if (filter != null && !filter.Contains(c)) continue;

            if (c.Namespace != null && (i == 0 || (i > 0 && cs[i - 1].Namespace != c.Namespace))) // namespace 去重
            {
                sb.Append(@"
namespace " + c.Namespace.Replace(".", "::") + @" {");
            }

            sb.Append(c._GetDesc()._GetComment_Cpp(4) + @"
    " + (c.IsValueType ? "struct" : "struct") + @" " + c.Name + @";");
            if (c._IsUserClass())
            {
                sb.Append(@"
    using " + c.Name + @"_s = std::shared_ptr<" + c.Name + @">;
    using " + c.Name + @"_w = std::weak_ptr<" + c.Name + @">;
");
            }

            if (c.Namespace != null && ((i < cs.Count - 1 && cs[i + 1].Namespace != c.Namespace) || i == cs.Count - 1))
            {
                sb.Append(@"
}");
            }
        }
    }

    static void GenH_Enums(this StringBuilder sb, List<Type> ts, TemplateLibrary.Filter<TemplateLibrary.CppFilter> filter)
    {
        var es = ts._GetEnums();
        for (int i = 0; i < es.Count; ++i)
        {
            var e = es[i];
            if (filter != null && !filter.Contains(e)) continue;

            if (e.Namespace != null && (i == 0 || (i > 0 && es[i - 1].Namespace != e.Namespace))) // namespace 去重
            {
                sb.Append(@"
namespace " + e.Namespace.Replace(".", "::") + @" {");
            }

            sb.Append(e._GetDesc()._GetComment_Cpp(4) + @"
    enum class " + e.Name + @" : " + e._GetEnumUnderlyingTypeName_Cpp() + @" {");

            var fs = e._GetEnumFields();
            foreach (var f in fs)
            {
                sb.Append(f._GetDesc()._GetComment_Cpp(8) + @"
        " + f.Name + " = " + f._GetEnumValue(e) + ",");
            }

            sb.Append(@"
    };");

            if (e.Namespace != null && ((i < es.Count - 1 && es[i + 1].Namespace != e.Namespace) || i == es.Count - 1))
            {
                sb.Append(@"
}");
            }
        }
    }

    static void GenH_Class(this StringBuilder sb, Type c, string templateName, object o)
    {
        // 定位到基类
        var bt = c.BaseType;
        var btn = c._HasBaseType() ? bt._GetTypeDecl_Cpp(templateName) : "::xx::Object";

        sb.Append(c._GetDesc()._GetComment_Cpp(4) + @"
    struct " + c.Name + @" : " + btn + @" {");

        GenH_Class_Fields(sb, c, templateName, o);



        sb.Append(@"
        XX_CODEGEN_CLASS_HEADER(" + c.Name + ", " + btn + @")");

        sb.Append(@"
    };");
    }

    static void GenH_Class_Fields(StringBuilder sb, Type c, string templateName, object o)
    {
        if (c._Has<TemplateLibrary.Include>())
        {
            sb.Append(@"
#include""" + c._GetTypeDecl_Lua(templateName) + @".inc""");
        }

        var fs = c._GetFieldsConsts();
        foreach (var f in fs)
        {
            var ft = f.FieldType;
            var ftn = ft._GetTypeDecl_Cpp(templateName);
            sb.Append(f._GetDesc()._GetComment_Cpp(8) + @"
        " + (f.IsStatic ? "constexpr " : "") + ftn + " " + f.Name);

            if (ft._IsExternal() && !ft._GetExternalSerializable() && !string.IsNullOrEmpty(ft._GetExternalCppDefaultValue()))
            {
                sb.Append(" = " + ft._GetExternalCppDefaultValue() + ";");
            }
            else
            {
                var v = f.GetValue(f.IsStatic ? null : o);
                var dv = ft._GetDefaultValueDecl_Cpp(v, templateName);
                if (dv != "")
                {
                    sb.Append(" = " + dv + ";");
                }
                else
                {
                    sb.Append(";");
                }
            }
        }
    }

    static void GenH_Struct(this StringBuilder sb, Type c, string templateName, object o)
    {
        // 定位到基类
        var bt = c.BaseType;
        var btn = c._HasBaseType() ? (" : " + bt._GetTypeDecl_Cpp(templateName)) : "";

        sb.Append(c._GetDesc()._GetComment_Cpp(4) + @"
    struct " + c.Name + btn + @" {");

        GenH_Class_Fields(sb, c, templateName, o);

        sb.Append(@"
    };");
    }

    static void GenH_ClassAndStruct(this StringBuilder sb, List<Type> cs, string templateName, TemplateLibrary.Filter<TemplateLibrary.CppFilter> filter, Assembly asm)
    {
        for (int i = 0; i < cs.Count; ++i)
        {
            var c = cs[i];
            if (filter != null && !filter.Contains(c)) continue;
            var o = asm.CreateInstance(c.FullName);

            if (c.Namespace != null && (i == 0 || (i > 0 && cs[i - 1].Namespace != c.Namespace))) // namespace 去重
            {
                sb.Append(@"
namespace " + c.Namespace.Replace(".", "::") + @" {");
            }


            if (c._IsUserClass())
            {
                sb.GenH_Class(c, templateName, o);
            }
            else
            {
                sb.GenH_Struct(c, templateName, o);
            }

            if (c.Namespace != null && ((i < cs.Count - 1 && cs[i + 1].Namespace != c.Namespace) || i == cs.Count - 1))
            {
                sb.Append(@"
}");
            }
        }
    }

    static void GenH_StructTemplates(this StringBuilder sb, List<Type> cs, TemplateLibrary.TypeIds typeIds, TemplateLibrary.Filter<TemplateLibrary.CppFilter> filter, string templateName)
    {
        sb.Append(@"
namespace xx {");
        cs = cs._GetStructs();
        foreach (var c in cs)
        {
            if (filter != null && !filter.Contains(c)) continue;
            var ctn = c._GetTypeDecl_Cpp(templateName);

            sb.Append(@"
	template<>
	struct BFuncs<" + ctn + @", void> {
		static void Write(BBuffer& bb, " + ctn + @" const& in) noexcept;
		static int Read(BBuffer& bb, " + ctn + @"& out) noexcept;
	};
	template<>
	struct SFuncs<" + ctn + @", void> {
		static inline void Append(std::string& s, " + ctn + @" const& in) noexcept;
		static inline void AppendCore(std::string& s, " + ctn + @" const& in) noexcept;
    };
	template<>
    struct IFuncs<" + ctn + @", void> {
		static inline int InitCascade(void* const& o, " + ctn + @" const& in) noexcept;
		static inline int InitCascadeCore(void* const& o, " + ctn + @" const& in) noexcept;
    };");
            // todo: IFuncs? 
        }

        // 遍历所有 type 及成员数据类型 生成  BBuffer.Register< T >( typeId ) 函数组
        foreach (var kv in typeIds.types)
        {
            if (filter != null && !filter.Contains(kv.Key)) continue;
            var ct = kv.Key;
            if (ct._IsString() || ct._IsBBuffer() || ct._IsExternal() && !ct._GetExternalSerializable()) continue;
            var typeId = kv.Value;
            var ctn = ct._GetTypeDecl_Cpp(templateName);

            sb.Append(@"
    template<> struct TypeId<" + ctn + @"> { static const uint16_t value = " + typeId + @"; };");
        }

        sb.Append(@"
}");
    }




    static void GenCPP_Include(this StringBuilder sb, string templateName, TemplateLibrary.Filter<TemplateLibrary.CppFilter> filter, List<Type> cs)
    {
        sb.Append(@"#include """ + templateName + "_class" + (filter != null ? "_filter" : "") + ".h" + @"""
#ifdef NEED_INCLUDE_"+ templateName + @"_class" + (filter != null ? "_filter" : "") + "_hpp" + @"
#include """ + templateName + "_class" + (filter != null ? "_filter" : "") + ".hpp" + @"""
#endif
");
        foreach (var c in cs)
        {
            if (c._Has<TemplateLibrary.Include>())
            {
                sb.Append(@"#include """ + c._GetTypeDecl_Lua(templateName) + @".hpp""
");
            }
        }
        sb.Append(@"
");
    }

    static void GenCPP_BBWrites(this StringBuilder sb, string templateName, Type c, bool isThis = true)
    {
        var cn = isThis ? "this->" : "in.";
        var ctn = c._GetTypeDecl_Cpp(templateName);
        if (isThis)
        {
            sb.Append(@"
    void " + c.Name + @"::ToBBuffer(xx::BBuffer& bb) const noexcept {");
        }
        else
        {
            sb.Append(@"
	void BFuncs<" + ctn + @", void>::Write(BBuffer& bb, " + ctn + @" const& in) noexcept {");
        }

        if (c._HasBaseType())
        {
            var bt = c.BaseType;
            var btn = bt._GetTypeDecl_Cpp(templateName);
            if (isThis)
            {
                sb.Append(@"
        this->BaseType::ToBBuffer(bb);");
            }
            else
            {
                sb.Append(@"
        BFuncs<" + btn + ">::Write(bb, in);");
            }
        }

        var fs = c._GetFields();
        foreach (var f in fs)
        {
            var ft = f.FieldType;
            if (ft._IsExternal() && !ft._GetExternalSerializable()) continue;
            if (f._Has<TemplateLibrary.Custom>())
            {
                // todo: call custom func
                throw new NotImplementedException();
            }
            else
            {
                sb.Append(@"
        bb.Write(" + cn + f.Name + ");");
            }
        }

        sb.Append(@"
    }");
    }

    static void GenCPP_BBReads(this StringBuilder sb, string templateName, Type c, bool isThis = true)
    {
        var ctn = c._GetTypeDecl_Cpp(templateName);
        if (isThis)
        {
            sb.Append(@"
    int " + c.Name + @"::FromBBuffer(xx::BBuffer& bb) noexcept {");
        }
        else
        {
            sb.Append(@"
	int BFuncs<" + ctn + @", void>::Read(BBuffer& bb, " + ctn + @"& out) noexcept {");
        }

        if (c._HasBaseType())
        {
            var bt = c.BaseType;
            var btn = bt._GetTypeDecl_Cpp(templateName);

            if (isThis)
            {
                sb.Append(@"
        if (int r = this->BaseType::FromBBuffer(bb)) return r;");
            }
            else
            {
                sb.Append(@"
        if (int r = BFuncs<" + btn + ">::Read(bb, out)) return r;");
            }
        }

        var cn = isThis ? "this->" : "out.";
        var fs = c._GetFields();
        foreach (var f in fs)
        {
            if (f.FieldType._IsExternal() && !f.FieldType._GetExternalSerializable()) continue;
            if (f.FieldType._IsContainer())
            {
                sb.Append(@"
        bb.readLengthLimit = " + f._GetLimit() + ";");
            }

            sb.Append(@"
        if (int r = bb.Read(" + cn + f.Name + @")) return r;");
        }

        sb.Append(@"
        return 0;
    }");
    }

    static void GenCPP_StrAppends(this StringBuilder sb, string templateName, Type c, bool isThis = true)
    {
        var ctn = c._GetTypeDecl_Cpp(templateName);
        if (isThis)
        {
            sb.Append(@"
    void " + c.Name + @"::ToString(std::string& s) const noexcept {
        if (this->toStringFlag)
        {
        	xx::Append(s, ""[ \""***** recursived *****\"" ]"");
        	return;
        }
        else this->SetToStringFlag();

        xx::Append(s, ""{ \""structTypeName\"":\""" + (string.IsNullOrEmpty(c.Namespace) ? c.Name : c.Namespace + "." + c.Name) + @"\"", \""structTypeId\"":"", GetTypeId());
        ToStringCore(s);
        xx::Append(s, "" }"");");
        }
        else
        {
            sb.Append(@"
	void SFuncs<" + ctn + @", void>::Append(std::string& s, " + ctn + @" const& in) noexcept {
        xx::Append(s, ""{ \""structTypeName\"":\""" + (string.IsNullOrEmpty(c.Namespace) ? c.Name : c.Namespace + "." + c.Name) + @"\"""");
        AppendCore(s, in);
        xx::Append(s, "" }"");");
        }

        if (isThis)
        {
            sb.Append(@"
        
        this->SetToStringFlag(false);");
        }

        if (isThis)
        {
            sb.Append(@"
    }
    void " + c.Name + @"::ToStringCore(std::string& s) const noexcept {");
        }
        else
        {
            sb.Append(@"
    }
	void SFuncs<" + ctn + @", void>::AppendCore(std::string& s, " + ctn + @" const& in) noexcept {");
        }
        if (c._HasBaseType())
        {
            var bt = c.BaseType;
            var btn = bt._GetTypeDecl_Cpp(templateName);
            if (isThis)
            {
                sb.Append(@"
        this->BaseType::ToStringCore(s);");
            }
            else
            {
                sb.Append(@"
        SFuncs<" + btn + ">::AppendCore(s, in);");
            }
        }

        var cn = isThis ? "this->" : "in.";
        var fs = c._GetFields();
        foreach (var f in fs)
        {
            if (f.FieldType._IsExternal() && !f.FieldType._GetExternalSerializable()) continue;
            sb.Append(@"
        xx::Append(s, ""\""" + f.Name + @"\"" : \"""", " + cn + f.Name + @", ""\"""");");
        }
        sb.Append(@"
    }");
    }

    static void GenCPP_InitCascades(this StringBuilder sb, string templateName, Type c, bool isThis = true)
    {
        var ctn = c._GetTypeDecl_Cpp(templateName);
        sb.Append(@"
#ifndef CUSTOM_INITCASCADE_" + c._GetTypeDecl_Lua(templateName) + @"");
        if (isThis)
        {
            sb.Append(@"
    int " + c.Name + @"::InitCascade(void* const& o) noexcept {
        return this->InitCascadeCore(o);
    }");
        }
        else
        {
            sb.Append(@"
	int IFuncs<" + ctn + @", void>::InitCascade(void* const& o, " + ctn + @" const& in) noexcept {
        return InitCascadeCore(o, in);
    }");
        }
        sb.Append(@"
#endif");

        if (isThis)
        {
            sb.Append(@"
    int " + c.Name + @"::InitCascadeCore(void* const& o) noexcept {");
        }
        else
        {
            sb.Append(@"
    int IFuncs<" + ctn + @", void>::InitCascadeCore(void* const& o, " + ctn + @" const& in) noexcept {");
        }

        if (c._HasBaseType())
        {
            var bt = c.BaseType;
            var btn = bt._GetTypeDecl_Cpp(templateName);
            if (isThis)
            {
            sb.Append(@"
        if (int r = this->BaseType::InitCascade(o)) return r;");
            }
            else
            {
                sb.Append(@"
        if (int r = IFuncs<" + btn + ">::InitCascadeCore(o, in)) return r;");
            }
        }

        var cn = isThis ? "this->" : "in.";
        var fs = c._GetFields();
        foreach (var f in fs)
        {
            var ft = f.FieldType;
            if (!ft._IsList() && !ft._IsUserClass() || ft._IsShared() || ft._IsWeak() || ft._IsExternal() && !ft._GetExternalSerializable()) continue;
            if (ft._IsList())
            {
                // 找出数组最底层子类型
                do
                {
                    ft = ft.GenericTypeArguments[0];
                }
                while (ft._IsList());
                if (!ft._IsUserClass() || ft._IsShared() || ft._IsWeak() || ft._IsExternal() && !ft._GetExternalSerializable()) continue;

                sb.Append(@"
        if (int r = xx::InitCascade(o, " + cn + f.Name + @")) return r;");
            }
            else
            {
                sb.Append(@"
        if (" + cn + f.Name + @") {
            if (int r = " + cn + f.Name + @"->InitCascade(o)) return r;
        }");
            }
        }

        sb.Append(@"
        return 0;
    }");
    }

    static void GenCPP_StructTemplate(this StringBuilder sb, string templateName, List<Type> cs, TemplateLibrary.Filter<TemplateLibrary.CppFilter> filter)
    {
        cs = cs._GetStructs();
        if (cs.Count == 0) return;
        sb.Append(@"
namespace xx {");
        foreach (var c in cs)
        {
            if (filter != null && !filter.Contains(c)) continue;
            sb.GenCPP_BBWrites(templateName, c, false);
            sb.GenCPP_BBReads(templateName, c, false);
            sb.GenCPP_StrAppends(templateName, c, false);
            sb.GenCPP_InitCascades(templateName, c, false);
        }

        sb.Append(@"
}");    // namespace xx
    }

    static void GenCPP_FuncImpls(this StringBuilder sb, string templateName, List<Type> cs, TemplateLibrary.Filter<TemplateLibrary.CppFilter> filter, TemplateLibrary.TypeIds typeIds)
    {
        sb.Append(@"
namespace " + templateName + @" {");

        cs = cs._GetClasss();
        for (int i = 0; i < cs.Count; ++i)
        {
            var c = cs[i];
            if (filter != null && !filter.Contains(c)) continue;

            // namespace c_ns {
            if (c.Namespace != null && (i == 0 || (i > 0 && cs[i - 1].Namespace != c.Namespace))) // namespace 去重
            {
                sb.Append(@"
namespace " + c.Namespace.Replace(".", "::") + @" {");
            }

            sb.Append(@"
    uint16_t " + c.Name + @"::GetTypeId() const noexcept {
        return " + typeIds.types[c] + @";
    }");
            sb.GenCPP_BBWrites(templateName, c);
            sb.GenCPP_BBReads(templateName, c);
            sb.GenCPP_StrAppends(templateName, c);
            sb.GenCPP_InitCascades(templateName, c);

            // namespace }
            if (c.Namespace != null && ((i < cs.Count - 1 && cs[i + 1].Namespace != c.Namespace) || i == cs.Count - 1))
            {
                sb.Append(@"
}");
            }
        }

        sb.Append(@"
}");    // namespace templateName
    }

    static void GenCpp_Registers(this StringBuilder sb, string templateName, TemplateLibrary.Filter<TemplateLibrary.CppFilter> filter, TemplateLibrary.TypeIds typeIds)
    {
        sb.Append(@"
namespace " + templateName + @" {
	AllTypesRegister::AllTypesRegister() {");
        foreach (var kv in typeIds.types)
        {
            var ct = kv.Key;
            if (filter != null && !filter.Contains(ct)) continue;
            if (ct._IsString() || ct._IsBBuffer() || ct._IsExternal() && !ct._GetExternalSerializable()) continue;
            var ctn = ct._GetTypeDecl_Cpp(templateName);

            sb.Append(@"
	    xx::BBuffer::Register<" + ctn + @">(" + kv.Value + @");");
        }
        sb.Append(@"
	}
}
");
    }





    static void GenInc(this StringBuilder sb)
    {
        sb.Append(@"#include ""xx_bbuffer.h""
#include ""xx_pos.h""
#include ""xx_random.h""
#include ""xx_dict.h""
");
    }
    static void GenIncEnd(this StringBuilder sb)
    {
        sb.Append(@"#include ""xx_random.hpp""
");
    }




    public static void Gen(Assembly asm, string outDir, string templateName, TemplateLibrary.Filter<TemplateLibrary.CppFilter> filter = null)
    {
        var ts = asm._GetTypes();
        var typeIds = new TemplateLibrary.TypeIds(asm);

        var sb = new StringBuilder();
        var cs = ts._GetClasssStructs();
        cs._SortByInheritRelation();

        // 生成 .h
        sb.GenH_Include(templateName);
        sb.GenH_RootNamespace(templateName);
        sb.GenH_PkgGenMd5(templateName);
        sb.GenH_Enums(ts, filter);
        sb.GenH_Predeclare(cs, filter);
        sb.GenH_ClassAndStruct(cs, templateName, filter, asm);
        sb.GenH_RootNamespaceEnd();
        sb.GenH_StructTemplates(cs, typeIds, filter, templateName);
        sb.GenH_IncludeEnd(cs, templateName, filter);
        sb._WriteToFile(Path.Combine(outDir, templateName + "_class" + (filter != null ? "_filter" : "") + ".h"));

        // 生成 .cpp
        sb.Clear();
        sb.GenCPP_Include(templateName, filter, cs);
        sb.GenCPP_StructTemplate(templateName, cs, filter);
        sb.GenCPP_FuncImpls(templateName, cs, filter, typeIds);
        sb.GenCpp_Registers(templateName, filter, typeIds);
        sb._WriteToFile(Path.Combine(outDir, templateName + "_class" + (filter != null ? "_filter" : "") + ".cpp"));

        // 生成 .inc, hpp
        sb.Clear();
        sb.GenInc();
        sb._WriteToFile(Path.Combine(outDir, templateName + "_class" + (filter != null ? "_filter" : "") + ".inc"));
        sb.Clear();
        sb.GenIncEnd();
        sb._WriteToFile(Path.Combine(outDir, templateName + "_class_end" + (filter != null ? "_filter" : "") + ".inc"));
        sb.Clear();
        sb._WriteToFile(Path.Combine(outDir, templateName + "_class" + (filter != null ? "_filter" : "") + ".hpp"));


        // 生成 里面的类.inc .hpp
        sb.Clear();
        foreach (var c in cs)
        {
            if (c._Has<TemplateLibrary.Include>())
            {
                sb._WriteToFile(Path.Combine(outDir, c._GetTypeDecl_Lua(templateName) + ".inc"));
                sb._WriteToFile(Path.Combine(outDir, c._GetTypeDecl_Lua(templateName) + ".hpp"));
            }
        }
    }
}




//var ms = c._GetMethods();
//foreach (var m in ms)
//{
//    var ps = m.GetParameters();
//    var rt = m.ReturnType;
//    var rtn = rt._GetTypeDecl_Cpp(templateName, "_s");
//    sb.Append(m._GetDesc()._GetComment_Cpp(8) + @"
//virtual " + rtn + " " + m.Name + "(");
//    foreach (var p in ps)
//    {
//        string attr = " ";
//        if (p._Has<TemplateLibrary.ConstRef>()) attr = " const& ";
//        if (p._Has<TemplateLibrary.PointerConstRef>()) attr = "* const& ";
//        sb.Append(p._GetDesc()._GetComment_Cpp(12) + @"
//    " + (p != ps[0] ? ", " : "") + p.ParameterType._GetTypeDecl_Cpp(templateName) + attr + p.Name);
//    }
//    sb.Append(") noexcept;");
//}


//        var ms = c._GetMethods();
//        foreach (var m in ms)
//        {
//            var ps = m.GetParameters();
//            var rt = m.ReturnType;
//            var rtn = rt._GetTypeDecl_Cpp(templateName, "_s");

//            sb.Append(@"
//" + rtn + " " + c.Name + "::" + m.Name + "(");
//            foreach (var p in ps)
//            {
//                string attr = " ";
//                if (p._Has<TemplateLibrary.ConstRef>()) attr = " const& ";
//                if (p._Has<TemplateLibrary.PointerConstRef>()) attr = "* const& ";

//                sb.Append(p._GetDesc()._GetComment_Cpp(12) + @"
//        " + (p != ps[0] ? ", " : "") + p.ParameterType._GetTypeDecl_Cpp(templateName) + attr + p.Name);
//            }
//            sb.Append(") noexcept {" + (rtn != "void" ? (" return " + rtn + "(); ") : "") + "}");
//        }