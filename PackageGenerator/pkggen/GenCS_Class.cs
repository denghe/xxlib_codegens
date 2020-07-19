using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Collections.Generic;

// 基于 XxObject.cs
// 相较于 C++, struct 或 标记有 [Struct] 的 class 都实现 IObject
// 语言限制, 标记有 [Struct] 的 class 也只能生成为 class. 使用过程中无法直接 = copy. 得用 oh.Clone
// Shared<T>, Weak<T> 在类成员中体现为相应的 property, 在 List 中 Weak 包裹一下

public static class GenCS_Class {
    // 存放标记了 [TypeId(xxx)] 的 id, type 映射
    static Dictionary<ushort, Type> typeIdMappings = new Dictionary<ushort, Type>();

    public static void Gen(Assembly asm, string outDir, string templateName) {
        var ts = asm._GetTypes();
        // 填充 typeId for class
        foreach (var c in ts._GetClasss().Where(o => !o._Has<TemplateLibrary.Struct>())) {
            var id = c._GetTypeId();
            if (id == null) throw new Exception("type: " + c.FullName + " miss [TypeId(xxxxxx)]");
            else {
                if (typeIdMappings.ContainsKey(id.Value)) {
                    throw new Exception("type: " + c.FullName + "'s typeId is duplicated with " + typeIdMappings[id.Value].FullName);
                }
                else {
                    typeIdMappings.Add(id.Value, c);
                }
            }
        }

        var sb = new StringBuilder();

        // usings
        sb.Append(@"using System;");

        // template namespace
        sb.Append(@"
namespace " + templateName + @" {
    #region GenMD5, GenTypes.RegisterTo
    public static class PkgGenMd5 {
        public const string value = """ + StringHelpers.MD5PlaceHolder + @"""; 
    }
    public static class PkgGenTypes {
        public static void RegisterTo(xx.ObjectHelper oh) {");
        foreach (var kv in typeIdMappings) {
            var ctn = kv.Value._GetTypeDecl_Csharp();
            sb.Append(@"
	        oh.Register<" + ctn + @">(" + kv.Key + @");");
        }
        sb.Append(@"
        }
    }
    #endregion");

        var es = ts._GetEnums();
        for (int i = 0; i < es.Count; ++i) {
            var e = es[i];

            // namespace e_ns {
            if (e.Namespace != null && (i == 0 || (i > 0 && es[i - 1].Namespace != e.Namespace))) // namespace 去重
            {
                sb.Append(@"
namespace " + e.Namespace + @" {");
            }

            // desc
            // public enum xxxxxxxxx : underlyingType
            sb.Append(e._GetDesc()._GetComment_CSharp(4) + @"
    public enum " + e.Name + @" : " + e._GetEnumUnderlyingTypeName_Csharp() + @" {");

            // desc
            // xxxxxx = val
            var fs = e._GetEnumFields();
            foreach (var f in fs) {
                sb.Append(f._GetDesc()._GetComment_CSharp(8) + @"
        " + f.Name + " = " + f._GetEnumValue(e) + ",");
            }

            // enum /
            sb.Append(@"
    }");

            // namespace }
            if (e.Namespace != null && ((i < es.Count - 1 && es[i + 1].Namespace != e.Namespace) || i == es.Count - 1)) {
                sb.Append(@"
}");
            }

        }


        var cs = ts._GetClasssStructs();
        for (int i = 0; i < cs.Count; ++i) {
            var c = cs[i];
            var o = asm.CreateInstance(c.FullName);

            // namespace c_ns {
            if (c.Namespace != null && (i == 0 || (i > 0 && cs[i - 1].Namespace != c.Namespace))) // namespace 去重
            {
                sb.Append(@"
namespace " + c.Namespace + @" {");
            }

            // desc
            // public T xxxxxxxxx = defaultValue
            // public const T xxxxxxxxx = defaultValue

            // 这里并不将标记为 [Struct] 的 class 视作 struct. useCount 会浪费掉
            if (c.IsValueType) {
                sb.Append(c._GetDesc()._GetComment_CSharp(4) + @"
    public partial struct " + c.Name + @" : xx.IObject {");
            }
            else {
                sb.Append(c._GetDesc()._GetComment_CSharp(4) + @"
    public partial class " + c.Name + @" : " + c.BaseType._GetTypeDecl_Csharp() + @" {");
            }

            // consts( static ) / fields
            var fs = c._GetFieldsConsts();
            foreach (var f in fs) {
                var ft = f.FieldType;
                var ftn = ft._GetTypeDecl_Csharp();

                if (ft._IsShared()) {
                    var ftct = ft._GetChildType();
                    var ftctn = ftct._GetTypeDecl_Csharp();
                    sb.Append(f._GetDesc()._GetComment_CSharp(8) + @"
        #region std::shared_ptr<" + ftctn + @"> " + f.Name + @"
        " + ftctn + @" __" + f.Name + @";
        public " + ftctn + @" " + f.Name + @" {
            get { return __" + f.Name + @"; }
            set {
                if (__" + f.Name + @" != null) __" + f.Name + @".Unhold();
                if (value != null) value.Hold();
                __" + f.Name + @" = value;
            }
        }
        #endregion");
                }
                else if (ft._IsWeak()) {
                    var ftct = ft._GetChildType();
                    var ftctn = ftct._GetTypeDecl_Csharp();
                    sb.Append(f._GetDesc()._GetComment_CSharp(8) + @"
        #region std::weak_ptr<" + ftctn + @"> " + f.Name + @"
        " + ftctn + @" __" + f.Name + @";
        public " + ftctn + @" " + f.Name + @" {
            get {
                if (__" + f.Name + @" != null && __" + f.Name + @".useCount == 0) {
                    __" + f.Name + @" = null;
                }
                return __" + f.Name + @";
            }
            set { __" + f.Name + @" = value; }
        }
        #endregion");
                }
                else if (ft._IsList()) {
                    sb.Append(f._GetDesc()._GetComment_CSharp(8) + @"
        #region " + ft._GetTypeDecl_CsharpForDisplayCppType() + @" " + f.Name + @"
        " + ftn + " __" + f.Name + @" = new " + ftn + @"();
        public " + ftn + @" " + f.Name + @" {
            get {
                return __" + f.Name + @";
            }
        }
        #endregion");
                }
                else {
                    sb.Append(f._GetDesc()._GetComment_CSharp(8) + @"
        public " + (f.IsStatic ? "const " : "") + ftn + " " + f.Name);

                    var v = f.GetValue(f.IsStatic ? null : o);
                    var dv = v._GetDefaultValueDecl_Csharp();
                    if (dv != "") {
                        sb.Append(" = " + dv + ";");
                    }
                    else {
                        sb.Append(";");
                    }
                }
            }

            sb.Append(@"
        #region IObject impls
        public" + (c.IsValueType ? "" : " override") + @" ushort GetTypeId() {
            return xx.TypeId<" + c.Name + @">.value;
        }

        public" + (c.IsValueType ? "" : " override") + @" void Serialize(xx.DataWriter dw) {");

            if (!c.IsValueType && c._HasBaseType()) {
                sb.Append(@"
            base.Serialize(dw);");
            }
            fs = c._GetFields();
            foreach (var f in fs) {
                if (f.FieldType._IsExternal() && !f.FieldType._GetExternalSerializable()) continue;
                var ft = f.FieldType;
                if (f._Has<TemplateLibrary.Custom>()) {
                    throw new NotImplementedException();
                    //        sb.Append(@"
                    //bb.CustomWrite(bb, this, """ + f.Name + @""");");
                }
                else if (ft.IsEnum) {
                    sb.Append(@"
            dw.Write((" + ft._GetEnumUnderlyingTypeName_Csharp() + ")this." + f.Name + ");");
                }
                else if (!ft._IsNullable() && ft.IsValueType && !ft.IsPrimitive) {
                    // 防拷贝?
                    sb.Append(@"
            ((xx.IObject)this." + f.Name + ").Serialize(dw);");
                }
                else {
                    sb.Append(@"
            dw.Write(this." + f.Name + ");");
                }
            }

            sb.Append(@"
        }

        public" + (c.IsValueType ? "" : " override") + @" void Deserialize(xx.DataReader dr) {");
            if (!c.IsValueType && c._HasBaseType()) {
                sb.Append(@"
            base.Deserialize(dr);");
            }
            fs = c._GetFields();
            foreach (var f in fs) {
                if (f.FieldType._IsExternal() && !f.FieldType._GetExternalSerializable()) continue;
                var ft = f.FieldType;

                if (ft.IsEnum) {
                    var ftn = ft._GetTypeDecl_Csharp();
                    var etn = ft._GetEnumUnderlyingTypeName_Csharp();
                    sb.Append(@"
            {
                " + etn + @" tmp = 0;
                dr.Read(ref tmp);
                this." + f.Name + @" = (" + ftn + @")tmp;
            }");
                }
                else if (ft._IsShared()) {
                    var ftct = ft._GetChildType();
                    var ftctn = ftct._GetTypeDecl_Csharp();
                    sb.Append(@"
            {
                " + ftctn + @" " + f.Name + @" = null;
                dr.Read(ref " + f.Name + @");
                this." + f.Name + @" = " + f.Name + @";
            }");
                }
                else if (ft._IsWeak()) {
                    sb.Append(@"
            {
                __" + f.Name + @" = null;
                dr.Read(ref __" + f.Name + @");
            }");
                }
                else if (ft._IsList()) {
                    var ftn = ft._GetTypeDecl_Csharp();
                    sb.Append(@"
            dr.Read(ref __" + f.Name + @");");
                }
                else if (!ft._IsNullable() && ft.IsValueType && !ft.IsPrimitive) {
                    sb.Append(@"
            ((xx.IObject)this." + f.Name + ").Deserialize(dr);");
                }
                else {
                    sb.Append(@"
            dr.Read(ref this." + f.Name + ");");
                }
            }
            sb.Append(@"
        }
        public" + (c.IsValueType ? "" : " override") + @" void ToString(xx.ObjectHelper oh) {");
            if (!c.IsValueType) {
                sb.Append(@"
            base.ToString(oh);");
            }
            else {
                sb.Append(@"
            ToStringCore(oh);");
            }
            sb.Append(@"
        }
        public" + (c.IsValueType ? "" : " override") + @" void ToStringCore(xx.ObjectHelper oh) {");
            if (c._HasBaseType()) {
                sb.Append(@"
            base.ToStringCore(oh);");
            }
            if (fs.Count > 0) {
                sb.Append(@"
            var s = oh.sb;");
            }
            foreach (var f in fs) {
                var ft = f.FieldType;
                if (ft._IsExternal() && !ft._GetExternalSerializable()) continue;
                if (ft._IsString()) {
                    sb.Append(@"
            if (" + f.Name + @" != null) s.Append("",\""" + f.Name + @"\"":\"""" + " + f.Name + @".ToString() + ""\"""");
            else s.Append("",\""" + f.Name + @"\"":null"");");
                }
                else if (ft._IsNullable()) {
                    sb.Append(@"
            s.Append("",\""" + f.Name + @"\"":"" + (" + f.Name + @".HasValue ? " + f.Name + @".Value.ToString() : ""nil""));");
                }
                else if (ft._IsWeak() || ft._IsShared()) {
                    sb.Append(@"
            s.Append("",\""" + f.Name + @"\"":"");
            if (" + f.Name + @" == null) s.Append(""null"");
            else {
                " + f.Name + @".ToString(oh);
            }");
                }
                else if (ft._IsList()) {
                    sb.Append(@"
            s.Append("",\""" + f.Name + @"\"":"");
            " + f.Name + @".ToString(oh);");
                }
                else if (ft._IsUserClass()) {
                    throw new Exception("miss Shared<> Weak<>");
                }
                else {
                    sb.Append(@"
            s.Append("", \""" + f.Name + @"\"":"" + " + f.Name + @".ToString());");
                }
            }
            sb.Append(@"
        }
        public" + (c.IsValueType ? "" : " override") + @" void Destruct() {");
            foreach (var f in fs) {
                var ft = f.FieldType;
                // 外部类型是否需要调用 Destruct, 值得商榷
                if (ft._IsExternal() && !ft._GetExternalSerializable()) continue;
                if (ft._IsShared()) {
                    sb.Append(@"
            " + f.Name + @" == null;");
                }
                else if (ft._IsList()) {
                    sb.Append(@"
            " + f.Name + @".Destruct();");
                }
                else if (ft._IsUserClass()) {
                    throw new Exception("miss Shared<> Weak<>");
                }
                else {
                }
            }
            sb.Append(@"
        }
        public override string ToString() {
            throw new Exception(""can't use this func tostring. miss ObjectHelper"");
        }
        #endregion");
            // class /
            sb.Append(@"
    }");

            // namespace }
            if (c.Namespace != null && ((i < cs.Count - 1 && cs[i + 1].Namespace != c.Namespace) || i == cs.Count - 1)) {
                sb.Append(@"
}");
            }

        }

        // template namespace /
        sb.Append(@"
}
");


        sb._WriteToFile(Path.Combine(outDir, templateName + "_class.cs"));
    }
}
