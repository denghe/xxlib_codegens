﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;


public static class TypeHelpers
{

    /// <summary>
    /// 从 template 对应的 asm 中提取用户填写的 types
    /// </summary>
    public static List<Type> _GetTypes(this Assembly asm)
    {
        return asm.GetTypes().Where(t => t.Namespace != nameof(TemplateLibrary)).ToList();
    }

    /// <summary>
    /// 从 template 对应的 asm 中提取用户填写的 具备指定 Attribute 的 types
    /// </summary>
    public static List<Type> _GetTypes<T>(this Assembly asm)
    {
        return asm.GetTypes().Where(t => t.Namespace != nameof(TemplateLibrary) && t._Has<T>()).ToList();
    }


    /// <summary>
    /// 从 types 中过滤出所有枚举类型
    /// </summary>
    public static List<Type> _GetEnums(this List<Type> ts, bool exceptExternal = true)
    {
        return ts.Where(t => t.IsEnum && !t._IsExternal()).ToList();
    }

    /// <summary>
    /// 从 types 中过滤出所有值类型
    /// </summary>
    public static List<Type> _GetStructs(this List<Type> ts, bool exceptExternal = true)
    {
        return ts.Where(t => t._IsUserStruct()).ToList();
    }

    /// <summary>
    /// 从 types 中过滤出所有引用类型
    /// </summary>
    public static List<Type> _GetClasss(this List<Type> ts, bool exceptExternal = true)
    {
        return ts.Where(t => t._IsUserClass()).ToList();
    }

    /// <summary>
    /// 从 types 中过滤出所有引用类型
    /// </summary>
    public static List<Type> _GetClasssStructs(this List<Type> ts, bool exceptExternal = true)
    {
        return ts.Where(t => (t._IsUserClass() || t._IsUserStruct())).ToList();
    }


    /// <summary>
    /// 从 types 中过滤出所有含指定 T(Attribute) 的 interface
    /// </summary>
    public static List<Type> _GetInterfaces<T>(this List<Type> ts)
    {
        return (from t in ts where (t.IsInterface && t._Has<T>()) select t).ToList();
    }


    public class TypeParentCount : IComparable<TypeParentCount>
    {
        public Type parent;
        public int totalChildLevel;

        public int CompareTo(TypeParentCount other)
        {
            return -this.totalChildLevel.CompareTo(other.totalChildLevel);  // 从大到小倒排
        }
    }

    /// <summary>
    /// 根据继承关系排序, 父前子后( 便于 C++ 生成 )
    /// </summary>
    public static List<Type> _SortByInheritRelation(this List<Type> ts)
    {
        // 建一个 type,  parent + 后代层数  字典. 先扫父子关系并填充, 再扫一次算后代层数
        // 最后类型根据后代层数倒排( 层数高的在前 )

        var dict = new Dictionary<Type, TypeParentCount>();
        foreach (var t in ts)
        {
            dict.Add(t, new TypeParentCount { parent = t._HasBaseType() ? t.BaseType : null });
        }
        foreach (var t in ts)
        {
            var parent = dict[t].parent;
            while (parent != null)
            {
                dict[parent].totalChildLevel++;
                parent = dict[parent].parent;
            }
        }
        var list = new List<TypeParentCount>();
        foreach (var t in ts)
        {
            list.Add(new TypeParentCount { parent = t, totalChildLevel = dict[t].totalChildLevel });
        }

        list.Sort();
        ts.Clear();
        ts.AddRange(list.Select(a => a.parent));
        return ts;
    }

    class TypeComparer : Comparer<Type>
    {
        public override int Compare(Type x, Type y)
        {
            return x.FullName.CompareTo(y.FullName);
        }
    }

    /// <summary>
    /// 根据 FullName 排序
    /// </summary>
    public static List<Type> _SortByFullName(this List<Type> ts)
    {
        ts.Sort(new TypeComparer());
        return ts;
    }

    // todo: 根据继承关系及值类型成员引用关系排序( 当前为简化设计, 模板中并不支持值类型成员 )


    /// <summary>
    /// 获取类型的成员函数列表
    /// </summary>
    public static List<MethodInfo> _GetMethods(this Type t)
    {
        return t.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).Where(
            m => m.Name != "ToString" &&
            m.Name != "Equals" &&
            m.Name != "GetHashCode" &&
            m.Name != "GetType" &&
            m.Name != "Finalize" &&
            m.Name != "MemberwiseClone"
            ).ToList();
    }



    /// <summary>
    /// 获取类型( 特指interface )的 property 列表
    /// </summary>
    public static List<PropertyInfo> _GetProperties(this Type t)
    {
        return t.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).ToList();
    }


    /// <summary>
    /// 获取 t( enum ) 的成员列表
    /// </summary>
    public static List<FieldInfo> _GetEnumFields(this Type t)
    {
        return t.GetFields(BindingFlags.Static | BindingFlags.Public).ToList();
    }

    /// <summary>
    /// 返回 t 是否为 string
    /// </summary>
    public static bool _IsString(this Type t)
    {
        return t.Namespace == nameof(System) && t.Name == nameof(String);
    }

    /// <summary>
    /// 返回 t 是否为 object
    /// </summary>
    public static bool _IsObject(this Type t)
    {
        return t.Namespace == nameof(System) && t.Name == nameof(Object);
    }

    /// <summary>
    /// 返回 t 是否为 Weak
    /// </summary>
    public static bool _IsWeak(this Type t)
    {
        return t.Namespace == nameof(TemplateLibrary) && t.Name == "Weak`1";
    }

    /// <summary>
    /// 返回 t 是否为 Weak
    /// </summary>
    public static bool _IsUnique(this Type t)
    {
        return t.Namespace == nameof(TemplateLibrary) && t.Name == "Unique`1";
    }

    /// <summary>
    /// 返回 t 是否为 Shared
    /// </summary>
    public static bool _IsShared(this Type t)
    {
        return t.Namespace == nameof(TemplateLibrary) && t.Name == "Shared`1";
    }


    /// <summary>
    /// 返回 t 是否为 Data
    /// </summary>
    public static bool _IsData(this Type t)
    {
        return t.IsArray && t.Name == "Byte[]";
    }


    /// <summary>
    /// 返回 t 是否为容器( string, data, list )
    /// </summary>
    public static bool _IsContainer(this Type t)
    {
        // todo: Nullable 包裹的也算容器
        if(t._IsNullable())
        {
            t = t._GetChildType();
        }
        return t._IsString() || t._IsList() || t._IsData();
    }

    /// <summary>
    /// 返回泛型第一个类型
    /// </summary>
    public static Type _GetChildType(this Type t)
    {
        return t.GenericTypeArguments[0];
    }

    /// <summary>
    /// 获取字段是否只读属性
    /// </summary>
    public static bool _IsReadOnly<T>(this T t) where T : ICustomAttributeProvider
    {
        foreach (var a in t.GetCustomAttributes(false))
        {
            if (a is TemplateLibrary.Column)
                return ((TemplateLibrary.Column)a).readOnly;
        }
        return false;
    }


    /// <summary>
    /// 返回 t 是否为 用户类
    /// </summary>
    public static bool _IsUserClass(this Type t)
    {
        return t.Namespace != nameof(System) && t.Namespace != nameof(TemplateLibrary) && t.IsClass && !t._Has<TemplateLibrary.Struct>() && !t._IsExternal();
    }

    /// <summary>
    /// 返回 t 是否为 数据库中的可空类型
    /// </summary>
    public static bool _IsSqlNullable(this Type t)
    {
        return t.IsValueType && t._IsNullable() || t._IsString() || t._IsData();
    }


    /// <summary>
    /// 返回 t 是否为 void
    /// </summary>
    public static bool _IsVoid(this Type t)
    {
        return t.Namespace == nameof(System) && t.Name == "Void";
    }

    /// <summary>
    /// 返回 t 是否为 DateTime
    /// </summary>
    public static bool _IsDateTime(this Type t)
    {
        return t.Namespace == nameof(TemplateLibrary) && t.Name == "DateTime";
    }


    /// <summary>
    /// 返回 t 是否为 数字类型( byte ~ int64, float, double )
    /// </summary>
    public static bool _IsNumeric(this Type t)
    {
        return t.Namespace == nameof(System) && t.IsValueType && (
                t.Name == "Byte" ||
                t.Name == "UInt8" ||
                t.Name == "UInt16" ||
                t.Name == "UInt32" ||
                t.Name == "UInt64" ||
                t.Name == "SByte" ||
                t.Name == "Int8" ||
                t.Name == "Int16" ||
                t.Name == "Int32" ||
                t.Name == "Int64" ||
                t.Name == "Double" ||
                t.Name == "Float" ||
                t.Name == "Single" ||
                t.Name == "Boolean" ||
                t.Name == "Bool"
            );
    }

    /// <summary>
    /// 返回 t 是否为 C# 中的 Nullable<>
    /// </summary>
    public static bool _IsNullable(this Type t)
    {
        return t.IsGenericType && (t.Namespace == nameof(System) || t.Namespace == nameof(TemplateLibrary)) && t.Name == "Nullable`1";
    }

    /// <summary>
    /// 返回 t 是否为 struct
    /// </summary>
    public static bool _IsUserStruct(this Type t)
    {
        return (t.IsClass && t._Has<TemplateLibrary.Struct>())
            || (t.IsValueType && !t.IsEnum && !t._IsNullable() && !t._IsNumeric() && !t._IsExternal());
    }


    /// <summary>
    /// 返回 t 是否为 Tuple<........>
    /// </summary>
    public static bool _IsTuple(this Type t)
    {
        return t.IsGenericType && t.Namespace == nameof(TemplateLibrary) && t.Name.StartsWith("Tuple`");
    }

    /// <summary>
    /// 返回 t 是否为 List<T>
    /// </summary>
    public static bool _IsList(this Type t)
    {
        return t.IsGenericType && t.Namespace == nameof(TemplateLibrary) && t.Name == "List`1";
    }

    /// <summary>
    /// 返回 t 是否为外部扩展类型
    /// </summary>
    public static bool _IsExternal(this Type t)
    {
        return _Has<TemplateLibrary.External>(t);
    }
    public static bool _GetExternalSerializable(this Type t)
    {
        foreach (var a in t.GetCustomAttributes(false))
        {
            if (a is TemplateLibrary.External)
                return ((TemplateLibrary.External)a).serializable;
        }
        return false;
    }
    public static string _GetExternalCppDefaultValue(this Type t)
    {
        foreach (var a in t.GetCustomAttributes(false))
        {
            if (a is TemplateLibrary.External)
                return ((TemplateLibrary.External)a).cppDefaultValue;
        }
        return "";
    }
    public static string _GetExternalCsharpDefaultValue(this Type t)
    {
        foreach (var a in t.GetCustomAttributes(false))
        {
            if (a is TemplateLibrary.External)
                return ((TemplateLibrary.External)a).csharpDefaultValue;
        }
        return "";
    }
    public static string _GetExternalLuaDefaultValue(this Type t)
    {
        foreach (var a in t.GetCustomAttributes(false))
        {
            if (a is TemplateLibrary.External)
                return ((TemplateLibrary.External)a).luaDefaultValue;
        }
        return "";
    }

    /// <summary>
    /// 获取 class 的成员列表( 含 const )
    /// </summary>
    public static List<FieldInfo> _GetFieldsConsts(this Type t)
    {
        return t.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance).ToList();
    }

    /// <summary>
    /// 获取 class 的成员列表( 不含 const )
    /// </summary>
    public static List<FieldInfo> _GetFields(this Type t)
    {
        return t.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).ToList();
    }


    /// <summary>
    /// 获取 class 的附加有指定 Attribute 的成员列表( 不含 const )
    /// </summary>
    public static List<FieldInfo> _GetFields<T>(this Type t)
    {
        return t.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).Where(f => f._Has<T>()).ToList();
    }



    /// <summary>
    /// 获取 C# 的默认值填充代码
    /// </summary>
    public static string _GetDefaultValueDecl_Csharp(this object v)
    {
        if (v == null) return "";
        var t = v.GetType();
        if (t._IsNullable())
        {
            return "";
        }
        else if (t.IsValueType)
        {
            if (t.IsEnum)
            {
                var sv = v._ToEnumInteger(t);
                if (sv == "0") return "";
                // 如果 v 的值在枚举中找不到, 输出硬转格式. 否则输出枚举项
                var fs = t._GetEnumFields();
                if (fs.Exists(f => f._GetEnumValue(t).ToString() == sv))
                {
                    return t.FullName + "." + v.ToString();
                }
                else
                {
                    return "(" + _GetTypeDecl_Csharp(t) + ")" + v._ToEnumInteger(t);
                }
            }
            var s = v.ToString();
            if (s == "0") return "";
            if (s == "True" || s == "False") return s.ToLower();
            return "";
        }
        else if (t._IsString())
        {
            return "@\"" + ((string)v).Replace("\"", "\"\"") + "\"";
        }
        else
        {
            return v.ToString();
        }
        // todo: 其他需要引号的类型的处理, 诸如 DateTime, Guid 啥的
    }


    /// <summary>
    /// 获取 CPP 的默认值填充代码
    /// </summary>
    public static string _GetDefaultValueDecl_Cpp(this Type t, object v, string templateName)
    {
        if (t._IsNullable())
        {
            return v == null ? "" : v.ToString();
        }
        if (t.IsGenericType || t._IsData())
        {
            return "";
        }
        if (t._IsString())
        {
            return v == null ? "" : ("@\"" + ((string)v).Replace("\"", "\"\"") + "\"");
        }
        if (t.IsValueType)
        {
            if (t.IsEnum)
            {
                var sv = v._ToEnumInteger(t);
                if (sv == "0") return "(" + _GetTypeDecl_Cpp(t, templateName) + ")0";
                // 如果 v 的值在枚举中找不到, 输出硬转格式. 否则输出枚举项
                var fs = t._GetEnumFields();
                if (fs.Exists(f => f._GetEnumValue(t).ToString() == sv))
                {
                    return _GetTypeDecl_Cpp(t, templateName) + "::" + v.ToString();
                }
                else
                {
                    return "(" + _GetTypeDecl_Cpp(t, templateName) + ")" + v._ToEnumInteger(t);
                }
            }
            if (t._IsNumeric()) return v.ToString().ToLower();   // lower for Ture, False bool
            else return "";
        }
        throw new NotImplementedException();
    }


    /// <summary>
    /// 获取 LUA 的默认值填充代码
    /// </summary>
    public static string _GetDefaultValueDecl_Lua(this object v, string templateName)
    {
        if (v == null) return "null";
        var t = v.GetType();
        if (t._IsNullable())
        {
            return "";
        }
        else if (t.IsValueType)
        {
            if (t.IsEnum)
            {
                var sv = v._ToEnumInteger(t);
                if (sv == "0") return "0";
                // 如果 v 的值在枚举中找不到, 输出硬转格式. 否则输出枚举项
                var fs = t._GetEnumFields();
                if (fs.Exists(f => f._GetEnumValue(t).ToString() == sv))
                {
                    return _GetTypeDecl_Lua(t, templateName) + "." + v.ToString();
                }
                else
                {
                    return sv.ToString();
                }
            }
            if (t._IsNumeric()) return v.ToString().ToLower();   // lower for Ture, False bool
            else return "";
        }
        else if (t._IsString())
        {
            return "[[" + (string)v + "]]";
        }
        else
        {
            return v.ToString();
        }
        // todo: 其他需要引号的类型的处理, 诸如 DateTime, Guid 啥的
    }


    /// <summary>
    /// 获取 C# 模板 的类型声明串
    /// </summary>
    public static string _GetTypeDecl_GenTypeIdTemplate(this Type t)
    {
        if (t._IsNullable())
        {
            return t.GenericTypeArguments[0]._GetTypeDecl_GenTypeIdTemplate() + "?";
        }
        if (t.IsArray)
        {
            throw new NotSupportedException();
        }
        else if (t._IsTuple())
        {
            string rtv = "Tuple<";
            for (int i = 0; i < t.GenericTypeArguments.Length; ++i)
            {
                if (i > 0)
                    rtv += ", ";
                rtv += _GetTypeDecl_GenTypeIdTemplate(t.GenericTypeArguments[i]);
            }
            rtv += ">";
            return rtv;
        }
        else if (t.IsEnum)
        {
            return t.FullName;
        }
        else
        {
            if (t.Namespace == nameof(TemplateLibrary))
            {
                if (t.Name == "Weak`1")
                {
                    return "Weak<" + _GetTypeDecl_GenTypeIdTemplate(t.GenericTypeArguments[0]) + ">";
                }
                if (t.Name == "Shared`1")
                {
                    return "Shared<" + _GetTypeDecl_GenTypeIdTemplate(t.GenericTypeArguments[0]) + ">";
                }
                else if (t.Name == "List`1")
                {
                    return "List<" + _GetTypeDecl_GenTypeIdTemplate(t.GenericTypeArguments[0]) + ">";
                }
                else if (t.Name == "DateTime")
                {
                    return "DateTime";
                }
                else if (t.Name == "Data")
                {
                    return "Data";
                }
            }
            else if (t.Namespace == nameof(System))
            {
                switch (t.Name)
                {
                    case "Object":
                        return "object";
                    case "Void":
                        return "void";
                    case "Byte":
                        return "byte";
                    case "UInt8":
                        return "byte";
                    case "UInt16":
                        return "ushort";
                    case "UInt32":
                        return "uint";
                    case "UInt64":
                        return "ulong";
                    case "SByte":
                        return "sbyte";
                    case "Int8":
                        return "sbyte";
                    case "Int16":
                        return "short";
                    case "Int32":
                        return "int";
                    case "Int64":
                        return "long";
                    case "Double":
                        return "double";
                    case "Float":
                        return "float";
                    case "Single":
                        return "float";
                    case "Boolean":
                        return "bool";
                    case "Bool":
                        return "bool";
                    case "String":
                        return "string";
                }
            }

            return t.FullName;
            //throw new Exception("unhandled data type");
        }
    }

    /// <summary>
    /// 获取 C# 的类型声明串
    /// </summary>
    public static string _GetTypeDecl_Csharp(this Type t)
    {
        if (t._IsNullable())
        {
            return t.GenericTypeArguments[0]._GetTypeDecl_Csharp() + "?";
        }
        if (t.IsArray)
        {
            throw new NotSupportedException();
            //return _GetCSharpTypeDecl(t.GetElementType()) + "[]";
        }
        else if (t._IsTuple())
        {
            string rtv = "Tuple<";
            for (int i = 0; i < t.GenericTypeArguments.Length; ++i)
            {
                if (i > 0)
                    rtv += ", ";
                rtv += _GetTypeDecl_Csharp(t.GenericTypeArguments[i]);
            }
            rtv += ">";
            return rtv;
        }
        else if (t.IsEnum)
        {
            return t.FullName;
        }
        else
        {
            if (t.Namespace == nameof(TemplateLibrary))
            {
                if (t.Name == "Weak`1")
                {
                    return "xx.Weak<" + _GetTypeDecl_Csharp(t.GenericTypeArguments[0]) + ">";
                }
                if (t.Name == "Shared`1")
                {
                    return "xx.Shared<" + _GetTypeDecl_Csharp(t.GenericTypeArguments[0]) + ">";
                }
                else if (t.Name == "List`1")
                {
                    return "xx.List<" + _GetTypeDecl_Csharp(t.GenericTypeArguments[0]) + ">";
                }
                else if (t.Name == "DateTime")
                {
                    return "DateTime";
                }
                else if (t.Name == "Data")
                {
                    return "xx.Data";
                }
            }
            else if (t.Namespace == nameof(System))
            {
                switch (t.Name)
                {
                    case "Object":
                        return "xx.Object";
                    case "Void":
                        return "void";
                    case "Byte":
                        return "byte";
                    case "UInt8":
                        return "byte";
                    case "UInt16":
                        return "ushort";
                    case "UInt32":
                        return "uint";
                    case "UInt64":
                        return "ulong";
                    case "SByte":
                        return "sbyte";
                    case "Int8":
                        return "sbyte";
                    case "Int16":
                        return "short";
                    case "Int32":
                        return "int";
                    case "Int64":
                        return "long";
                    case "Double":
                        return "double";
                    case "Float":
                        return "float";
                    case "Single":
                        return "float";
                    case "Boolean":
                        return "bool";
                    case "Bool":
                        return "bool";
                    case "String":
                        return "string";
                }
            }

            return t.FullName;
            //throw new Exception("unhandled data type");
        }
    }


    /// <summary>
    /// 获取 C++ 的类型声明串
    /// </summary>
    public static string _GetTypeDecl_Cpp(this Type t, string templateName)
    {
        if (t._IsNullable())
        {
            return "std::optional<" + t.GenericTypeArguments[0]._GetTypeDecl_Cpp(templateName) + ">";
        }
        if (t._IsData())
        {
            return "xx::Data";
        }
        else if (t._IsTuple())
        {
            string rtv = "std::tuple<";
            for (int i = 0; i < t.GenericTypeArguments.Length; ++i)
            {
                if (i > 0)
                {
                    rtv += ", ";
                }
                rtv += t.GenericTypeArguments[i]._GetTypeDecl_Cpp(templateName);
            }
            rtv += ">";
            return rtv;
        }
        else if (t.IsEnum)  // enum & struct
        {
            return (t._IsExternal() ? "" : templateName) + "::" + t.FullName.Replace(".", "::");
        }
        else
        {
            if (t.Namespace == nameof(TemplateLibrary))
            {
                switch (t.Name)
                {
                    case "Weak`1":
                        return "std::weak_ptr<" + _GetTypeDecl_Cpp(t.GenericTypeArguments[0], templateName) + ">";
                    case "Shared`1":
                        return "std::shared_ptr<" + _GetTypeDecl_Cpp(t.GenericTypeArguments[0], templateName) + ">";
                    case "Unique`1":
                        return "std::unique_ptr<" + _GetTypeDecl_Cpp(t.GenericTypeArguments[0], templateName) + ">";
                    case "List`1":
                        {
                            var ct = t.GenericTypeArguments[0];
                            return "std::vector" + @"<" + ct._GetTypeDecl_Cpp(templateName) + ">";
                        }
                    case "Data":
                        return "xx::Data";
                    default:
                        throw new NotImplementedException();
                }
            }
            else if (t.Namespace == nameof(System))
            {
                switch (t.Name)
                {
                    case "Object":
                        return "::xx::Object";
                    case "Void":
                        return "void";
                    case "Byte":
                        return "uint8_t";
                    case "UInt8":
                        return "uint8_t";
                    case "UInt16":
                        return "uint16_t";
                    case "UInt32":
                        return "uint32_t";
                    case "UInt64":
                        return "uint64_t";
                    case "SByte":
                        return "int8_t";
                    case "Int8":
                        return "int8_t";
                    case "Int16":
                        return "int16_t";
                    case "Int32":
                        return "int32_t";
                    case "Int64":
                        return "int64_t";
                    case "Double":
                        return "double";
                    case "Float":
                        return "float";
                    case "Single":
                        return "float";
                    case "Boolean":
                        return "bool";
                    case "Bool":
                        return "bool";
                    case "String":
                        return "std::string";
                }
            }
            return (t._IsExternal() ? "" : templateName) + "::" + t.FullName.Replace(".", "::");
            //return (t._IsExternal() ? "" : ("::" + templateName)) + "::" + t.FullName.Replace(".", "::") + (t.IsValueType ? "" : ((t._IsExternal() && !t._GetExternalSerializable()) ? "" : suffix));
        }
    }



    /// <summary>
    /// 获取 C++ 的类型声明串
    /// </summary>
    public static string _GetTypeDecl_Lua(this Type t, string templateName)
    {
        if (t._IsNullable())
        {
            return "Nullable" + _GetTypeDecl_Lua(t.GenericTypeArguments[0], templateName);
        }
        else if (t._IsWeak())
        {
            return "Weak_" + _GetTypeDecl_Lua(t.GenericTypeArguments[0], templateName);
        }
        else if (t._IsList())
        {
            string rtv = t.Name.Substring(0, t.Name.IndexOf('`')) + "_";
            for (int i = 0; i < t.GenericTypeArguments.Length; ++i)
            {
                if (i > 0)
                    rtv += "_";
                rtv += _GetTypeDecl_Lua(t.GenericTypeArguments[i], templateName);
            }
            rtv += "_";
            return rtv;
        }
        else if (t.Namespace == nameof(System) || t.Namespace == nameof(TemplateLibrary))
        {
            return t.Name;
        }
        return (t._IsExternal() ? "" : templateName) + "_" + t.FullName.Replace(".", "_");
    }





    /// <summary>
    /// 获取枚举对应的数字类型的类型名
    /// </summary>
    public static string _GetEnumUnderlyingTypeName_Csharp(this Type e)
    {
        switch (e.GetEnumUnderlyingType().Name)
        {
            case "Byte":
                return "byte";
            case "SByte":
                return "sbyte";
            case "UInt16":
                return "ushort";
            case "Int16":
                return "short";
            case "UInt32":
                return "uint";
            case "Int32":
                return "int";
            case "UInt64":
                return "ulong";
            case "Int64":
                return "long";
        }
        throw new Exception("unsupported data type");
    }

    /// <summary>
    /// 获取枚举对应的数字类型的类型名之 cpp 版
    /// </summary>
    public static string _GetEnumUnderlyingTypeName_Cpp(this Type e)
    {
        switch (e.GetEnumUnderlyingType().Name)
        {
            case "Byte":
                return "uint8_t";
            case "SByte":
                return "int8_t";
            case "UInt16":
                return "uint16_t";
            case "Int16":
                return "int16_t";
            case "UInt32":
                return "uint32_t";
            case "Int32":
                return "int32_t";
            case "UInt64":
                return "uint64_t";
            case "Int64":
                return "int64_t";
        }
        throw new Exception("unsupported data type");
    }


    /// <summary>
    /// 获取枚举项 f 的数字值
    /// </summary>
    public static string _GetEnumValue(this FieldInfo f, Type e)
    {
        return f.GetValue(null)._ToEnumInteger(e);
    }

    /// <summary>
    /// 将枚举转为数字值
    /// </summary>
    public static string _ToEnumInteger(this object enumValue, System.Type e)
    {
        switch (e.GetEnumUnderlyingType().Name)
        {
            case "Byte":
                return Convert.ToByte(enumValue).ToString();
            case "SByte":
                return Convert.ToSByte(enumValue).ToString();
            case "UInt16":
                return Convert.ToUInt16(enumValue).ToString();
            case "Int16":
                return Convert.ToInt16(enumValue).ToString();
            case "UInt32":
                return Convert.ToUInt32(enumValue).ToString();
            case "Int32":
                return Convert.ToInt32(enumValue).ToString();
            case "UInt64":
                return Convert.ToUInt64(enumValue).ToString();
            case "Int64":
                return Convert.ToInt64(enumValue).ToString();
        }
        throw new Exception("unknown Underlying Type");
    }

    /// <summary>
    /// 获取 C# 风格的注释
    /// </summary>
    public static string _GetComment_CSharp(this string s, int space)
    {
        if (s.Trim() == "")
            return "";
        var sps = new string(' ', space);
        s = s.Replace("\r\n", "\n")
         .Replace("\r", "\n")
         .Replace("\n", "\r\n" + sps + "/// ");
        return "\r\n" +
    sps + @"/// <summary>
" + sps + @"/// " + s + @"
" + sps + @"/// </summary>";
    }


    /// <summary>
    /// 获取 Cpp 风格的注释
    /// </summary>
    public static string _GetComment_Cpp(this string s, int space)
    {
        if (s.Trim() == "")
            return "";
        var sps = new string(' ', space);
        s = s.Replace("\r\n", "\n")
         .Replace("\r", "\n")
         .Replace("\n", "\r\n" + sps + "// ");
        return "\r\n"
 + sps + @"// " + s;
    }


    /// <summary>
    /// 获取 LUA 风格的注释
    /// </summary>
    public static string _GetComment_Lua(this string s, int space)
    {
        if (s.Trim() == "")
            return "";
        var sps = new string(' ', space);
        return @"
" + sps + @"--[[
" + sps + s + @"
" + sps + "]]";
    }


    public static string _GetSpace(this string s, int space)
    {
        return new string(' ', space);
    }



    /// <summary>
    /// 将填写的 Sql 文本按占位符 {?} 的位置切割成  string / int 交替形态, 以便于遍历和生成相关拼接语句
    /// </summary>
    public static List<object> _SpliteSql(this string sql)
    {
        var rtv = new List<object>();
        var sb = new StringBuilder();
        string numStr = "";
        int offset = 0, i = 0;
        while (offset < sql.Length)
        {
            var c = sql[offset];
            if (c == '{')
            {
                c = sql[++offset];
                if (c == '{')
                {
                    sb.Append('{');
                }
                else
                {
                    while (offset < sql.Length)
                    {
                        c = sql[offset];
                        if (c == '}')
                        {
                            rtv.Add(sb.ToString());
                            sb.Clear();

                            int.TryParse(numStr, out i);
                            rtv.Add(i);
                            numStr = "";

                            break;
                        }
                        else
                        {
                            numStr += c;
                        }
                        ++offset;
                    }
                }
            }
            else
            {
                sb.Append(c);
            }
            ++offset;
        }
        if (sb.Length > 0)
        {
            rtv.Add(sb.ToString());
        }
        return rtv;
    }

    /// <summary>
    /// 返回类型对应的 SqlDataReader 之数据读取函数名
    /// </summary>
    public static string _GetDataReaderFuncName(this Type t)
    {
        if (t._IsNullable())
        {
            return _GetDataReaderFuncName(t.GenericTypeArguments[0]);
        }
        if (t.Namespace == nameof(TemplateLibrary))
        {
            switch (t.Name)
            {
                case "DateTime":
                    return "r.GetDateTime";
                //case "Data":
                //    return "r.GetXXXXXXXXXXXXX";

                default:
                    throw new Exception("unhandled data type");
            }
        }
        else if (t.Namespace == nameof(System))
        {
            switch (t.Name)
            {
                case "Void":
                    throw new Exception("impossible");
                case "Byte":
                    return "r.GetByte";
                case "UInt8":
                    return "r.GetSByte";
                case "UInt16":
                    return "r.GetUInt16";
                case "UInt32":
                    return "r.GetUInt32";
                case "UInt64":
                    return "r.GetUInt64";
                case "SByte":
                    return "r.GetSByte";
                case "Int8":
                    return "r.GetByte";
                case "Int16":
                    return "r.GetInt16";
                case "Int32":
                    return "r.GetInt32";
                case "Int64":
                    return "r.GetInt64";
                case "Double":
                    return "r.GetDouble";
                case "Float":
                    return "r.GetFloat";
                case "Single":
                    return "r.GetFloat";
                case "Boolean":
                    return "r.GetBoolean";
                case "Bool":
                    return "r.GetBoolean";
                case "String":
                    return "r.GetString";
                //case "DateTime":
                //    return "r.GetDateTime";

                default:
                    throw new Exception("unhandled data type");
            }
        }
        else if (t.IsEnum)
        {
            return "(" + t.FullName + ")r.Get" + t.GetEnumUnderlyingType().Name;
        }
        //else if (t.Namespace == nameof(TemplateLibrary) && t.Name == "Data")
        //{
        //    return "r.GetBBuffer";
        //}
        else throw new Exception("todo");
    }

    /// <summary>
    /// 返回类型对应的 SqlDataReader 之数据读取函数名 之 C++ 版
    /// </summary>
    public static string _GetDataReaderFuncName_Cpp(this Type t, int colIdx)
    {
        if (t._IsNullable())
        {
            return _GetDataReaderFuncName_Cpp(t.GenericTypeArguments[0], colIdx);
        }
        else if (t.Namespace == nameof(TemplateLibrary))
        {
            switch (t.Name)
            {
                //case "DateTime":
                //    return "sr.GetDateTime";
                case "Data":
                    return "this->mempool->MPCreate<xx::Data>(sr.ReadBlob(" + colIdx + "))";

                default:
                    throw new Exception("unhandled data type");
            }
        }
        else if (t.Namespace == nameof(System))
        {
            switch (t.Name)
            {
                case "Void":
                    throw new Exception("impossible");
                case "Byte":
                    return "(uint8_t)sr.ReadInt32(" + colIdx + ")";
                case "UInt8":
                    return "(uint8_t)sr.ReadInt32(" + colIdx + ")";
                case "UInt16":
                    return "(uint16_t)sr.ReadInt32(" + colIdx + ")";
                case "UInt32":
                    return "(uint32_t)sr.ReadInt32(" + colIdx + ")";
                case "UInt64":
                    return "(uint64_t)sr.ReadInt64(" + colIdx + ")";
                case "SByte":
                    return "(int8_t)sr.ReadInt32(" + colIdx + ")";
                case "Int8":
                    return "(int8_t)sr.ReadInt32(" + colIdx + ")";
                case "Int16":
                    return "(int16_t)sr.ReadInt32(" + colIdx + ")";
                case "Int32":
                    return "sr.ReadInt32(" + colIdx + ")";
                case "Int64":
                    return "sr.ReadInt64(" + colIdx + ")";
                case "Double":
                    return "sr.ReadDouble(" + colIdx + ")";
                case "Float":
                    return "(float)sr.ReadDouble(" + colIdx + ")";
                case "Single":
                    return "(float)sr.ReadDouble(" + colIdx + ")";
                case "Boolean":
                    return "sr.ReadInt32(" + colIdx + ") ? true : false";
                case "Bool":
                    return "sr.ReadInt32(" + colIdx + ") ? true : false";
                case "String":
                    return "this->mempool->MPCreate<xx::String>(sr.ReadString(" + colIdx + "))";
                //case "DateTime":
                //    return "sr.GetDateTime";

                default:
                    throw new Exception("unhandled data type");
            }
        }
        else if (t.IsEnum)
        {
            return "(" + t.FullName + ")" + _GetDataReaderFuncName_Cpp(t.GetEnumUnderlyingType(), colIdx);
        }
        //else if (t.Namespace == nameof(TemplateLibrary) && t.Name == "Data")
        //{
        //    return "sr.GetBBuffer";
        //}
        else throw new Exception("unhandled data type:" + t.FullName);
    }









    /// <summary>
    /// 判断目标上是否有附加某个类型的 Attribute
    /// </summary>
    public static bool _Has<T>(this ICustomAttributeProvider f)
    {
        return f.GetCustomAttributes(false).Any(a => a is T);
    }

    /// <summary>
    /// 获取 Attribute 之 Limit 值. 未找到将返回 0
    /// </summary>
    public static int _GetLimit(this ICustomAttributeProvider t)
    {
        foreach (var a in t.GetCustomAttributes(false))
        {
            if (a is TemplateLibrary.Limit)
                return ((TemplateLibrary.Limit)a).value;
        }
        return 0;
    }

    /// <summary>
    /// 获取 Attribute 之 Limit 值的集合. 未找到将返回 0 长度集合
    /// </summary>
    public static List<int> _GetLimits(this ICustomAttributeProvider t)
    {
        var rtv = new List<int>();
        foreach (var a in t.GetCustomAttributes(false))
        {
            if (a is TemplateLibrary.Limit)
            {
                rtv.Add(((TemplateLibrary.Limit)a).value);
            }
        }
        return rtv;
    }



    /// <summary>
    /// 获取 Attribute 之 Desc 注释. 未找到将返回 ""
    /// </summary>
    public static string _GetDesc(this ICustomAttributeProvider t)
    {
        foreach (var a in t.GetCustomAttributes(false))
        {
            if (a is TemplateLibrary.Desc)
                return ((TemplateLibrary.Desc)a).value;
        }
        return "";
    }




    /// <summary>
    /// 获取 Attribute 之 TypeId 值. 未找到将返回 null
    /// </summary>
    public static ushort? _GetTypeId(this ICustomAttributeProvider t) {
        foreach (var a in t.GetCustomAttributes(false)) {
            if (a is TemplateLibrary.TypeId)
                return ((TemplateLibrary.TypeId)a).value;
        }
        return null;
    }




    /// <summary>
    /// 获取 Attribute 之 Sql 文本. 未找到将返回 ""
    /// </summary>
    public static string _GetSql(this ICustomAttributeProvider t)
    {
        foreach (var a in t.GetCustomAttributes(false))
        {
            if (a is TemplateLibrary.Sql)
                return ((TemplateLibrary.Sql)a).value;
        }
        return "";
    }

    /// <summary>
    /// 判断目标类型是否为派生类
    /// </summary>
    public static bool _HasBaseType(this Type t)
    {
        if (t.BaseType == null) return false;
        return t.BaseType != typeof(object) && t.BaseType != typeof(System.ValueType);
    }



}


///// <summary>
///// 获取 Attribute 之 Key. 未找到将返回 null
///// </summary>
//public static bool? _GetKey(this ICustomAttributeProvider t)
//{
//    foreach (var a in t.GetCustomAttributes(false))
//    {
//        if (a is TemplateLibrary.Key)
//            return ((TemplateLibrary.Key)a).value;
//    }
//    return false;
//}
