#pragma warning disable 0169, 0414
using TemplateLibrary;

namespace Test
{

    [AttachInclude, CustomInitCascade, Desc("Class1")]
    class Class1
    {
        [Desc("int")]
        int int32;

        [Desc("float")]
        float float32;

        [Desc("double")]
        double float64;

        [Desc("string")]
        string str;
    }

    [AttachInclude, CustomInitCascade, Desc("Class2")]
    class Class2
    {
        [Desc("List<int>")]
        List<int> list1;

        [Desc("List<List<int>>")]
        List<List<int>> list2;

        [Desc("List<List<int>>")]
        List<List<List<int>>> list3;


        [Desc("List<Class1>")]
        List<Class1> list4;

        [Desc("List<List<Class1>>")]
        List<List<Class1>> list5;

        [Desc("List<List<List<string>>>")]
        List<List<List<string>>> list6;
    }

    [Desc("TestStruct1")]
    struct TestStruct1
    {
        [Desc("x")]
        float x;

        [Desc("y")]
        float y;

        [Desc("角度")]
        float angle;
    }

    [Desc("TestStruct2")]
    struct TestStruct2
    {
        [Desc("x")]
        float x;

        [Desc("y")]
        float y;

        [Desc("TestStruct1")]
        TestStruct1 struct1;
    }
}
