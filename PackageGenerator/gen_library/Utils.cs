using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

public static class GenUtils
{
    public static string MD5PlaceHolder = "#*MD5<>*#";
    public static string MD5PlaceHolder_Left = "#*MD5<";
    public static string MD5PlaceHolder_Right = ">*#";

    public static string GetMD5(byte[] data)
    {
        var md5 = new MD5CryptoServiceProvider();
        var bytes = md5.ComputeHash(data);
        var sb = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            sb.Append(bytes[i].ToString("x2"));
        }
        return sb.ToString();
    }

    public static string GetMD5(string txt)
    {
        return GetMD5(Encoding.UTF8.GetBytes(txt));
    }

    public static string GetMD5(StringBuilder sb)
    {
        return GetMD5(Encoding.UTF8.GetBytes(sb.ToString()));
    }


    public static Assembly GetAssembly(string[] fileNames)
    {
        Console.WriteLine(typeof(GenUtils).Assembly.Location);
        var c = CodeDomProvider.CreateProvider("CSharp");
        var p = new CompilerParameters(new string[] {
            typeof(GenUtils).Assembly.Location
        })
        {
            GenerateExecutable = false,
            GenerateInMemory = true
        };
        var r = c.CompileAssemblyFromFile(p, fileNames);
        if (r.Errors.Count == 0)
        {
            return r.CompiledAssembly;
        }

        var sb = new StringBuilder();
        foreach (var s in r.Errors)
        {
            sb.Append(s.ToString() + "\r\n");
        }
        throw new Exception(sb.ToString());
    }
}
