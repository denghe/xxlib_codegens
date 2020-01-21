using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

public static class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            TipsAndExit(@"
*.cs -> codes 生成器使用提示：

缺参数：
    要生成啥,    根命名空间,    输出目录,    源码文件或目录清单

生成类型：
" + GenTypesToString() + @"

要生成啥:
    就是把对应的 生成类型 int 值加起来

输出目录:
    如果缺失，默认为 工作目录.必须已存在, 不会帮忙创建

源码文件或目录清单:
    如果缺失，默认为 工作目录下 *.cs

具体的生成选项藏于 *.cs 中
");
        }

        int genTypes;
        if (!int.TryParse(args[0], out genTypes))
        {
            TipsAndExit("参数1 错误：要生成啥 是个 int");
        }
        if (genTypes == 0 || genTypes >= (int)GenTypes.MaxValue)
        {
            TipsAndExit("参数1 错误：无效的值");
        }

        var rootNamespace = args[1];
        // todo: 针对 namespace 规范的非法字符检查

        var outPath = ".";
        var inPaths = new List<string>();

        if (args.Length > 2)
        {
            if (!Directory.Exists(args[2]))
            {
                TipsAndExit("参数3 错误：目录不存在");
            }
            outPath = args[2];
        }

        if (args.Length < 4)
        {
            inPaths.Add(Environment.CurrentDirectory);
        }
        else
        {
            for (int i = 3; i < args.Length; ++i)
            {
                inPaths.Add(args[i]);
            }
        }

        var fileNames = new HashSet<string>();
        foreach (var path in inPaths)
        {
            if (Directory.Exists(path))
            {
                foreach (var f in Directory.GetFiles(path, "*.cs"))
                {
                    fileNames.Add(f);
                }
            }
            else if (File.Exists(path))
            {
                fileNames.Add(path);
            }
            else
            {
                TipsAndExit($"参数错误：目录或文件不存在: " + path);
            }
        }

        if (fileNames.Count == 0)
        {
            Console.WriteLine("当前目录：" + Environment.CurrentDirectory);
            TipsAndExit("生成失败: 找不到任何可供生成的 .cs 文件");
        }

        Assembly asm = null;
        try
        {
            asm = GenUtils.GetAssembly(fileNames.ToArray());
        }
        catch (Exception ex)
        {
            TipsAndExit("生成失败: *.cs 编译错误。\r\n" + ex);
        }

        Console.WriteLine("开始生成");
#if !DEBUG
        try
        {
#endif
            Gen((GenTypes)genTypes, rootNamespace, outPath, asm);
#if !DEBUG
        }
        catch (Exception ex)
        {
            TipsAndExit("生成失败: " + ex.Message + "\r\n" + ex.StackTrace);
        }
#endif
        TipsAndExit("生成完毕");
    }

    [Flags]
    public enum GenTypes : int
    {
        CppClass = 1 << 0,
        CppClassFilter = 1 << 1,
        CppSqlite = 1 << 2,
        CSharpClass = 1 << 3,
        CSharpMySql = 1 << 4,
        LuaClassFilter = 1 << 5,
        MaxValue = 1 << 6
    }
    public static string GenTypesToString()
    {
        var sb = new StringBuilder();
        var t = typeof(GenTypes);
        foreach (int v in Enum.GetValues(typeof(GenTypes)))
        {
            var name = Enum.GetName(typeof(GenTypes), v);
            if (name == "MaxValue") continue;
            sb.Append("    " + Enum.GetName(typeof(GenTypes), v) + " = " + v + "\r\n");
        }
        return sb.ToString();
    }

    public static void Gen(GenTypes genTypes, string rootNamespace, string outPath, Assembly asm)
    {
        // 提示：生成时, md5 部分内容用 GenUtils.MD5PlaceHolder 占位. 存盘时会被替换为真正 md5

        if (!GenTypeId.Gen(asm, outPath, rootNamespace))
        {
            TipsAndExit("生成停止：" + rootNamespace + "_TypeIdMappings.cs 已生成. 请将其放入 源码文件或目录清单 并再次生成. ");
        }

        var gt = (int)genTypes;
        if ((gt & (int)GenTypes.CppClass) > 0)
        {
            GenCPP_Class.Gen(asm, outPath, rootNamespace);
        }
        if ((gt & (int)GenTypes.CppClassFilter) > 0)
        {
            GenCPP_Class.Gen(asm, outPath, rootNamespace, new TemplateLibrary.Filter<TemplateLibrary.CppFilter>(asm));
        }
        if ((gt & (int)GenTypes.CppSqlite) > 0)
        {
            GenCPP_SQLite.Gen(asm, outPath, rootNamespace);
        }
        if ((gt & (int)GenTypes.CSharpClass) > 0)
        {
            GenCS_Class.Gen(asm, outPath, rootNamespace);
        }
        if ((gt & (int)GenTypes.CSharpMySql) > 0)
        {
            GenCS_MySql.Gen(asm, outPath, rootNamespace);
        }
        if ((gt & (int)GenTypes.LuaClassFilter) > 0)
        {
            GenLUA_Class.Gen(asm, outPath, rootNamespace, new TemplateLibrary.Filter<TemplateLibrary.LuaFilter>(asm));
        }
        // ...
    }

    static void TipsAndExit(string msg, int exitCode = 0)
    {
        Console.WriteLine(msg);
        Console.WriteLine("按任意键退出");
        Console.ReadKey();
        Environment.Exit(exitCode);
    }
}
