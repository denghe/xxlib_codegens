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
        [Nan(0), Infinity(0)]
        float _float;
        [Nan(0), Infinity(0)]
        double _double;
        bool _bool;
        [Limit(16)]
        string _string;
        [Limit(32)]
        byte[] _bbuffer;
    }
}

[Desc("测试可空值类型")]
[Struct]
class A : NS1.A
{
    int? nullable_int;
    Nullable<string> nullable_string;
    Nullable<byte[]> nullable_bbuffer;
}

namespace NS3.NS4
{
    [Desc("测试可空值类型数组")]
    [Struct]
    class A : global::A
    {
        List<int?> list_nullable_int;
        List<Nullable<string>> list_nullable_string;
        List<Nullable<byte[]>> list_nullable_bbuffer;
    }
}

[Desc("测试可空可空值类型数组")]
[Struct]
class B : NS3.NS4.A
{
    Nullable<List<int?>> _int;
    Nullable<List<Nullable<string>>> _string;
    Nullable<List<Nullable<byte[]>>> _bbuffer;
}

[Desc("包含结构体 B 用于收发. 测试多层 List + Limit")]
class Foo
{
    [Limit(1), Limit(3)]
    List<List<Nullable<B>>> bs;
}

[Desc("测试 Weak 递归引用")]
class Node
{
    Weak<Node> parent;
}

[Desc("测试 Unique, Shared")]
class NodeContainer
{
    Shared<Node> node;
    Unique<Foo> foo;
}

// todo: 测试 External Include Custom
