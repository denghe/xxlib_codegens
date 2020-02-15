#pragma warning disable 0169, 0414
using TemplateLibrary;

namespace NS1
{
    [Desc("测试传统值类型")]
    [Struct]
    class A
    {
        byte _byte;
        sbyte _sbyte;
        ushort _ushort;
        short _short;
        uint _uint;
        int _int;
        ulong _ulong;
        long _long;
        float _float;
        double _double;
        bool _bool;
        [Limit(16)]
        string _string;
        [Limit(32)]
        byte[] _data;
    }
}

[Desc("测试可空值类型")]
[Struct]
class A : NS1.A
{
    int? nullable_int;
    Nullable<string> nullable_string;
    Nullable<byte[]> nullable_data;
}

namespace NS3.NS4
{
    [Desc("测试可空值类型数组")]
    [Struct]
    class A : global::A
    {
        List<int?> list_nullable_int;
        List<Nullable<string>> list_nullable_string;
        List<Nullable<byte[]>> list_nullable_data;
    }
}

[Desc("测试可空可空值类型数组")]
[Struct]
class B : NS3.NS4.A
{
    Nullable<List<int?>> nullable_list_nullable_int;
    Nullable<List<Nullable<string>>> nullable_list_nullable_string;
    Nullable<List<Nullable<byte[]>>> nullable_list_nullable_data;
}

[Desc("测试多层 List + Limit")]
class Foo
{
    [Limit(1), Limit(3)]
    List<List<Nullable<B>>> list_list_nullable_b;

    [Limit(1), Limit(1), Limit(4)]
    List<List<string>> list_list_string;

    [Limit(1), Limit(1), Limit(4)]
    Nullable<List<Nullable<List<Nullable<string>>>>> nullable_list_nullable_list_nullable_string;

    [Limit(1), Limit(1), Limit(4)]
    List<List<byte[]>> list_list_data;
}

[Desc("测试 Weak 递归引用")]
class Node
{
    Weak<Node> parent;
}

[Desc("测试 Shared")]
class NodeContainer
{
    Shared<Node> node;
}

// todo: 测试 External Include Custom
