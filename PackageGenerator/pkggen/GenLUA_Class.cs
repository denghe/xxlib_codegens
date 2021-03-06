﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;


public static class GenLUA_Class
{
    public static void Gen(Assembly asm, string outDir, string templateName, TemplateLibrary.Filter<TemplateLibrary.LuaFilter> filter)
    {
        var sb = new StringBuilder();

        sb.Append(@"
" + templateName + @"_PkgGenMd5_Value = '" + StringHelpers.MD5PlaceHolder + @"'
");
        var ts = asm._GetTypes();
        var es = ts._GetEnums();
        for (int i = 0; i < es.Count; ++i)
        {
            var e = es[i];
            if (!filter.Contains(e)) continue;
            var en = e._GetTypeDecl_Lua(templateName);
            sb.Append(e._GetDesc()._GetComment_Lua(0) + @"
" + en + @" = {");

            var fs = e._GetEnumFields();
            foreach (var f in fs)
            {
                sb.Append(f._GetDesc()._GetComment_Lua(4) + @"
    " + f.Name + " = " + f._GetEnumValue(e) + ",");
            }
            sb.Length--;

            // enum /
            sb.Append(@"
}");
        }

        // 遍历所有 type 及成员数据类型 生成  typeId

        var typeIds = new TemplateLibrary.TypeIds(asm);
        foreach (var kv in typeIds.types)
        {
            //if (kv.Key._IsString() || kv.Key._IsBBuffer()) continue;
            var c = kv.Key;
            if (!filter.Contains(c)) continue;
            var typeId = (ushort)kv.Value;
            var cn = c._GetTypeDecl_Lua(templateName);
            var o = asm.CreateInstance(c.FullName);
            if (c.FullName.IndexOf("EnterGameCatchFishLevelRoomSit_Success") >= 0)
                Console.WriteLine();
            var fs = c._GetFieldsConsts();

            sb.Append(c._GetDesc()._GetComment_Lua(0) + @"
" + cn + @" = {
    typeName = """ + cn + @""",
    typeId = " + typeId + @",
    Create = function()
        local o = {}
        o.__proto = " + cn + @"
        o.__index = o
        o.__newindex = o
");
            if (!c._IsList())
            {
                sb.Append(@"
");
            }
            foreach (var f in fs)
            {
                var ft = f.FieldType;
                if (o == null)
                {
                    sb.Append(@"
        o." + f.Name + " = null");
                }
                else
                {
                    var v = f.GetValue(f.IsStatic ? null : o);
                    var dv = v._GetDefaultValueDecl_Lua(templateName);
                    sb.Append(f._GetDesc()._GetComment_Lua(8));
                    if (ft._IsWeak() || ft._IsUserStruct())
                    {
                        throw new Exception("LUA does not support weak_ptr or struct");
                    }
                    else if (dv != "")
                    {
                        sb.Append(@"
        o." + f.Name + @" = " + dv);
                    }
                    else
                    {
                        sb.Append(@"
        o." + f.Name + " = null");
                    }
                }
                sb.Append(" -- " + ft._GetTypeDecl_Lua(templateName));
            }

            if (c._HasBaseType())
            {
                var bt = c.BaseType._GetTypeDecl_Lua(templateName);
                sb.Append(@"
        setmetatable( o, " + bt + @".Create() )");
            }
            sb.Append(@"
        return o
    end,
    FromBBuffer = function( bb, o )");
            if (c._HasBaseType())
            {
                var bt = c.BaseType._GetTypeDecl_Lua(templateName);
                sb.Append(@"
        local p = getmetatable( o )
        p.__proto.FromBBuffer( bb, p )");
            }
            var ftns = new Dictionary<string, int>();
            foreach (var f in fs)
            {
                var ft = f.FieldType;
                var ftn = "";
                if (ft._IsWeak() || ft._IsUserStruct())
                {
                    throw new Exception("LUA does not support weak_ptr or struct");
                }
                if (ft._IsNullable())
                {
                    ftn = "Nullable" + ft.GenericTypeArguments[0].Name;
                }
                else
                {
                    ftn = ft.IsEnum ? ft.GetEnumUnderlyingType().Name : ft._IsNumeric() ? ft.Name : "Object";
                    if (ft._IsData() || ft._IsString()) ftn = "Object";
                }
                if (ftns.ContainsKey(ftn)) ftns[ftn]++;
                else ftns.Add(ftn, 1);
            }
            foreach (var kvp in ftns)
            {
                if (kvp.Value > 1)
                {
                    sb.Append(@"
        local Read" + kvp.Key + @" = bb.Read" + kvp.Key);
                }
            }
            foreach (var f in fs)
            {
                var ft = f.FieldType;
                var ftn = "";
                if (ft._IsWeak() || ft._IsUserStruct())
                {
                    throw new Exception("LUA does not support weak_ptr or struct");
                }
                if (ft._IsNullable())
                {
                    ftn = "Nullable" + ft.GenericTypeArguments[0].Name;
                }
                else
                {
                    ftn = ft.IsEnum ? ft.GetEnumUnderlyingType().Name : ft._IsNumeric() ? ft.Name : "Object";
                    if (ft._IsData() || ft._IsString()) ftn = "Object";
                }
                if (ftns[ftn] > 1)
                {

                    sb.Append(@"
        o." + f.Name + @" = Read" + ftn + @"( bb )");
                }
                else
                {
                    sb.Append(@"
        o." + f.Name + @" = bb:Read" + ftn + @"()");
                }
            }
            if (c._IsList())
            {
                var fn = "ReadObject";
                var ct = c.GenericTypeArguments[0];
                if (ct._IsWeak() || ct._IsUserStruct())
                {
                    throw new Exception("LUA does not support weak_ptr or struct");
                }
                if (!ct._IsUserClass() && !ct._IsData() && !ct._IsString())
                {
                    if (ct.IsEnum)
                    {
                        var ctn = ct.GetEnumUnderlyingType().Name;
                        fn = "Read" + ctn;
                    }
                    else
                    {
                        if (ct._IsNullable())
                        {
                            fn = "ReadNullable" + ct.GenericTypeArguments[0].Name;
                        }
                        else
                        {
                            fn = "Read" + ct.Name;
                        }
                    }
                }
                sb.Append(@"
		local len = bb:ReadUInt32()
        local f = BBuffer." + fn + @"
		for i = 1, len do
			o[ i ] = f( bb )
		end");
            }
            sb.Append(@"
    end,
    Serialize = function( bb, o )");
            if (c._HasBaseType())
            {
                var bt = c.BaseType._GetTypeDecl_Lua(templateName);
                sb.Append(@"
        local p = getmetatable( o )
        p.__proto.Serialize( bb, p )");
            }
            foreach (var kvp in ftns)
            {
                if (kvp.Value > 1)
                {
                    sb.Append(@"
        local Write" + kvp.Key + @" = bb.Write" + kvp.Key);
                }
            }
            foreach (var f in fs)
            {
                var ft = f.FieldType;
                var ftn = "";
                if (ft._IsWeak() || ft._IsUserStruct())
                {
                    throw new Exception("LUA does not support weak_ptr or struct");
                }
                if (ft._IsNullable())
                {
                    ftn = "Nullable" + ft.GenericTypeArguments[0].Name;
                }
                else
                {
                    ftn = ft.IsEnum ? ft.GetEnumUnderlyingType().Name : ft._IsNumeric() ? ft.Name : "Object";
                    if (ft._IsData() || ft._IsString()) ftn = "Object";
                }
                if (ftns[ftn] > 1)
                {
                    sb.Append(@"
        Write" + ftn + @"( bb, o." + f.Name + @" )");
                }
                else
                {
                    sb.Append(@"
        bb:Write" + ftn + @"( o." + f.Name + @" )");
                }
            }
            if (c._IsList())
            {
                var fn = "WriteObject";
                var ct = c.GenericTypeArguments[0];
                if (!ct._IsUserClass() && !ct._IsData() && !ct._IsString())
                {
                    if (ct.IsEnum)
                    {
                        var ctn = ct.GetEnumUnderlyingType().Name;
                        fn = "Write" + ctn;
                    }
                    else
                    {
                        var ctn = ct.Name;
                        fn = "Write" + ctn;
                    }

                }
                sb.Append(@"
        local len = #o
		bb:WriteUInt32( len )
        local f = BBuffer." + fn + @"
        for i = 1, len do
			f( bb, o[ i ]" + @" )
		end");
            }
            sb.Append(@"
    end
}
BBuffer.Register( " + cn + @" )");
        }

        // 临时方案
        sb.Replace("`1", "");

        sb._WriteToFile(Path.Combine(outDir, templateName + "_class.lua"));
    }
}
