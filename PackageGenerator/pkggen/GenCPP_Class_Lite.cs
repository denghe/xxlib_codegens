using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Collections.Generic;


// 不支持 引用, 全是值类型. 只生成模板适配序列化函数. 基于 xx_data, xx_serialize

public static class GenCPP_Class_Lite {
    static void GenH_Head(this StringBuilder sb, string templateName) {
        sb.Append(@"#pragma once
#include ""xx_data_view.h""
namespace " + templateName + @" {
	struct PkgGenMd5 {
		inline static const std::string value = """ + StringHelpers.MD5PlaceHolder + @""";
    };
");
    }

    static void GenH_Tail(this StringBuilder sb, string templateName) {
        sb.Append(@"
}");
    }

    static void GenH_Enums(this StringBuilder sb, List<Type> ts) {
        var es = ts._GetEnums();
        for (int i = 0; i < es.Count; ++i) {
            var e = es[i];
            if (e.Namespace != null && (i == 0 || (i > 0 && es[i - 1].Namespace != e.Namespace))) // namespace 去重
            {
                sb.Append(@"
namespace " + e.Namespace.Replace(".", "::") + @" {");
            }

            sb.Append(e._GetDesc()._GetComment_Cpp(4) + @"
    enum class " + e.Name + @" : " + e._GetEnumUnderlyingTypeName_Cpp() + @" {");

            var fs = e._GetEnumFields();
            foreach (var f in fs) {
                sb.Append(f._GetDesc()._GetComment_Cpp(8) + @"
        " + f.Name + " = " + f._GetEnumValue(e) + ",");
            }

            sb.Append(@"
    };");

            if (e.Namespace != null && ((i < es.Count - 1 && es[i + 1].Namespace != e.Namespace) || i == es.Count - 1)) {
                sb.Append(@"
}");
            }
        }
    }


    static void GenH_ClassAndStruct(this StringBuilder sb, List<Type> cs, string templateName, Assembly asm) {
        for (int i = 0; i < cs.Count; ++i) {
            var c = cs[i];
            var o = asm.CreateInstance(c.FullName);

            if (c.Namespace != null && (i == 0 || (i > 0 && cs[i - 1].Namespace != c.Namespace))) // namespace 去重
            {
                sb.Append(@"
namespace " + c.Namespace.Replace(".", "::") + @" {");
            }


            sb.GenH_Struct(c, templateName, o);

            if (c.Namespace != null && ((i < cs.Count - 1 && cs[i + 1].Namespace != c.Namespace) || i == cs.Count - 1)) {
                sb.Append(@"
}");
            }
        }
    }

    static void GenH_Struct(this StringBuilder sb, Type c, string templateName, object o) {
        // 定位到基类
        var bt = c.BaseType;

        var btn = c._HasBaseType() ? (" : " + bt._GetTypeDecl_Cpp(templateName)) : "";
        sb.Append(c._GetDesc()._GetComment_Cpp(4) + @"
    struct " + c.Name + btn + @" {");

        sb.GenH_Struct_Fields(c, templateName, o);
        sb.GenH_Struct_CopyAssign(templateName, c);

        sb.Append(@"
    };");
    }

    static void GenH_Struct_Fields(this StringBuilder sb, Type c, string templateName, object o) {
        var fs = c._GetFieldsConsts();
        foreach (var f in fs) {
            var ft = f.FieldType;
            var ftn = ft._GetTypeDecl_Cpp(templateName);
            sb.Append(f._GetDesc()._GetComment_Cpp(8) + @"
        " + (f.IsStatic ? "constexpr " : "") + ftn + " " + f.Name);

            if (ft._IsExternal() && !ft._GetExternalSerializable() && !string.IsNullOrEmpty(ft._GetExternalCppDefaultValue())) {
                sb.Append(" = " + ft._GetExternalCppDefaultValue() + ";");
            }
            else {
                var v = f.GetValue(f.IsStatic ? null : o);
                var dv = ft._GetDefaultValueDecl_Cpp(v, templateName);
                if (dv != "") {
                    sb.Append(" = " + dv + ";");
                }
                else {
                    sb.Append(";");
                }
            }
        }
    }


    static void GenH_Struct_CopyAssign(this StringBuilder sb, string templateName, Type c) {
        sb.Append(@"

        " + c.Name + @"() = default;
        " + c.Name + @"(" + c.Name + @" const&) = default;
        " + c.Name + @"& operator=(" + c.Name + @" const&) = default;
        " + c.Name + @"(" + c.Name + @"&& o);
        " + c.Name + @"& operator=(" + c.Name + @"&& o);");
    }


    static void GenH_StructTemplates(this StringBuilder sb, List<Type> cs, TemplateLibrary.TypeIds typeIds, string templateName) {
        sb.Append(@"
namespace xx {");
        cs = cs._GetStructs();
        foreach (var c in cs) {
            var ctn = c._GetTypeDecl_Cpp(templateName);

            sb.Append(@"
	template<>
	struct DataFuncs<" + ctn + @", void> {
		static void Write(Data& d, " + ctn + @" const& in) noexcept;
		static int Read(DataReader& d, " + ctn + @"& out) noexcept;
	};");
        }

        // todo: 获取所有标记有 TypeId attribute 的类 生成

        // 遍历所有 type 及成员数据类型 生成  Deserializer.Register< T >( typeId ) 函数组
        foreach (var kv in typeIds.types) {
            var ct = kv.Key;
            if (ct._IsString() || ct._IsData() || ct._IsExternal() && !ct._GetExternalSerializable()) continue;
            var typeId = kv.Value;
            var ctn = ct._GetTypeDecl_Cpp(templateName);

            sb.Append(@"
    template<> struct TypeId<" + ctn + @"> { static const uint16_t value = " + typeId + @"; };");
        }

        sb.Append(@"
}");
    }





    static void GenCPP_Struct_CopyAssign(this StringBuilder sb, string templateName, Type c) {
        sb.Append(@"
    inline " + c.Name + @"::" + c.Name + @"(" + c.Name + @"&& o) {
        this->operator=(std::move(o));
    }
    inline " + c.Name + @"& " + c.Name + @"::operator=(" + c.Name + @"&& o) {");
        if (c._HasBaseType()) {
            var bt = c.BaseType;
            var btn = bt._GetTypeDecl_Cpp(templateName);
            sb.Append(@"
        this->BaseType::operator=(std::move(o));");
        }
        var fs = c._GetFields();
        foreach (var f in fs) {
            var ft = f.FieldType;
            sb.Append(@"
        std::swap(this->" + f.Name + ", o." + f.Name + ");");
        }
        sb.Append(@"
        return *this;
    }");
    }

    static void GenCPP_Serialize(this StringBuilder sb, string templateName, Type c) {
        var ctn = c._GetTypeDecl_Cpp(templateName);
        sb.Append(@"
	inline void DataFuncs<" + ctn + @", void>::Write(Data& d, " + ctn + @" const& in) noexcept {");

        if (c._HasBaseType()) {
            var bt = c.BaseType;
            var btn = bt._GetTypeDecl_Cpp(templateName);
            sb.Append(@"
        DataFuncs<" + btn + ">::Write(d, in);");
        }

        var fs = c._GetFields();
        foreach (var f in fs) {
            var ft = f.FieldType;
            if (ft._IsExternal() && !ft._GetExternalSerializable()) continue;
            if (f._Has<TemplateLibrary.Custom>()) {
                // todo
                throw new NotImplementedException();
            }
            else {
                sb.Append(@"
        ::xx::Write(d, in." + f.Name + ");");
            }
        }

        sb.Append(@"
    }");
    }

    static void GenCPP_Deserialize(this StringBuilder sb, string templateName, Type c) {
        var ctn = c._GetTypeDecl_Cpp(templateName);
        sb.Append(@"
	inline int DataFuncs<" + ctn + @", void>::Read(DataReader& d, " + ctn + @"& out) noexcept {");

        if (c._HasBaseType()) {
            var bt = c.BaseType;
            var btn = bt._GetTypeDecl_Cpp(templateName);

            sb.Append(@"
        if (int r = DataFuncs<" + btn + ">::Read(d, out)) return r;");
        }

        var fs = c._GetFields();
        foreach (var f in fs) {
            if (f.FieldType._IsExternal() && !f.FieldType._GetExternalSerializable()) continue;
            var funcName = new StringBuilder("Read");
            var limits = f._GetLimits();
            if (f.FieldType._IsContainer() && limits.Count > 0) {
                funcName.Append("Limit<");
                foreach (var limit in limits) {
                    funcName.Append(limit.ToString() + ", ");
                }
                funcName.Length -= 2;
                funcName.Append(">");
            }
            sb.Append(@"
        if (int r = d." + funcName + "(out." + f.Name + @")) return r;");
        }

        sb.Append(@"
        return 0;
    }");
    }

    static void GenCPP_FuncImpls(this StringBuilder sb, string templateName, List<Type> cs) {
        sb.Append(@"



namespace " + templateName + @" {");

        for (int i = 0; i < cs.Count; ++i) {
            var c = cs[i];

            // namespace c_ns {
            if (c.Namespace != null && (i == 0 || (i > 0 && cs[i - 1].Namespace != c.Namespace))) // namespace 去重
            {
                sb.Append(@"
namespace " + c.Namespace.Replace(".", "::") + @" {");
            }

            sb.GenCPP_Struct_CopyAssign(templateName, c);

            // namespace }
            if (c.Namespace != null && ((i < cs.Count - 1 && cs[i + 1].Namespace != c.Namespace) || i == cs.Count - 1)) {
                sb.Append(@"
}");
            }
        }

        sb.Append(@"
}");    // namespace templateName

        sb.Append(@"
namespace xx {");
        for (int i = 0; i < cs.Count; ++i) {
            var c = cs[i];

            sb.GenCPP_Serialize(templateName, c);
            sb.GenCPP_Deserialize(templateName, c);
        }
        sb.Append(@"
}
");
    }



    public static void Gen(Assembly asm, string outDir, string templateName) {
        var ts = asm._GetTypes();
        var typeIds = new TemplateLibrary.TypeIds(asm);

        var sb = new StringBuilder();
        var cs = ts._GetClasssStructs();
        cs._SortByInheritRelation();

        // 生成 .h
        sb.GenH_Head(templateName);
        sb.GenH_Enums(ts);
        sb.GenH_ClassAndStruct(cs, templateName, asm);
        sb.GenH_Tail(templateName);
        sb.GenH_StructTemplates(cs, typeIds, templateName);
        sb.GenCPP_FuncImpls(templateName, cs);
        sb._WriteToFile(Path.Combine(outDir, templateName + "_class_lite.h"));
    }
}
