using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

public static class GenTypeId
{
    public static bool Gen(Assembly asm, string outDir, string templateName)
    {
        var typeIds = new TemplateLibrary.TypeIds(asm);
        if (typeIds.typeIdMappingsExists && !typeIds.hasNewMappings) return true;

        var sb = new StringBuilder();
        sb.Append(@"#pragma warning disable 0169, 0414
using TemplateLibrary;

[TypeIdMappings]
interface ITypeIdMappings
{");
        int count = 0;
        foreach (var kv in typeIds.types)
        {
            var c = kv.Key;
            if (!c._IsUserClass()) continue;
            var typeId = kv.Value;
            var cn = c._GetTypeDecl_GenTypeIdTemplate();
            sb.Append(@"
    " + cn + @" _" + typeId + @" { get; }
");
            ++count;
        }
        sb.Append(@"
}
");
        if (count == 0) return true;
        sb._WriteToFile(Path.Combine(outDir, templateName + "_TypeIdMappings.cs"));
        return false;
    }

    //public static bool CheckTypeId(ushort[] typeIdRanges, int typeId)
    //{
    //    // typeId基础类型
    ////    if (typeId <= 2) return true;
    //    var len = typeIdRanges.Length / 2;
    //    for (int i = 0; i < len; ++i)
    //    {
    //        int s = typeIdRanges[i << 1];
    //        int e = typeIdRanges[(i << 1) + 1];
    //        if (typeId >= s && typeId < e) return true;
    //    }
    //    return false;
    //}

    //public static void CheckAllTypeIds(string path, string searchPattern)
    //{
    //    var typeIdSet = new HashSet<ushort>();
    //    var allRanges = new List<ushort>();
    //    // 校验所有模块typeId是否有重复
    //    foreach (var fn in Directory.GetFiles(path, searchPattern))
    //    {
    //        var asm = Assembly.LoadFile(fn);
    //        var range = TemplateLibrary.TypeIds.GetTypeIdRangeInfo(asm);
    //        foreach (var idx in range)
    //        {
    //            allRanges.Add(idx);
    //        }

    //        var ts = asm._GetTypes();
    //        var typeIds = new TemplateLibrary.TypeIds(asm);

    //        foreach (var item in typeIds.dict)
    //        {
    //            var id = item.Value;
    //            if (id <= 2) continue;
    //            if (typeIdSet.Contains(id))
    //            {
    //                throw new Exception("重复的typeId:" + item);
    //            }
    //            if (!CheckTypeId(range, id))
    //            {
    //                throw new Exception("当前typeId不在配置范围内:" + item);
    //            }
    //            typeIdSet.Add(id);
    //        }
    //    }

    //    // 检查typeId范围是否有交叉
    //    var ranges = allRanges.ToArray();
    //    var len = ranges.Length >> 1;
    //    for (int i = 0; i < len; ++i)
    //    {
    //        int s1 = ranges[i << 1];
    //        int e1 = ranges[(i << 1) + 1];
    //        if (e1 <= s1)
    //        {
    //            throw new Exception(String.Format(
    //                "typeId范围结束值小于其实值:{0}, {1}.", s1, e1));
    //        }
    //        for (int j = i + 1; j < len; ++j)
    //        {
    //            int s2 = ranges[j << 1];
    //            int e2 = ranges[(j << 1) + 1];

    //            if (!(s1 >= e2 || s2 >= e1))
    //            {
    //                throw new Exception(String.Format(
    //                    "typeId范围有交叉:s1={0}, e1={1}, s2={2}, e2={3}.", s1, e1, s2, e2));
    //            }

    //        }
    //    }
    //}

}
