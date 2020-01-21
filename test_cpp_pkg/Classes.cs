#pragma warning disable 0169, 0414
using TemplateLibrary;

// 规则：
// class 代表继承自 xx::Object
// struct 代表类似 pod 类型的一般结构体，没有 typeId 分配
// 发送只能直接发 class. 不管是 shared_ptr<class> 还是 class 值类型，以同样的编码格式发送
// 接收时统统认为是 shared_ptr<class>.
// 不可写 Shared<struct> 或 Weak<struct>.
// List 是 struct / 值类型
// todo: 生成时的合法性检测
// 不标记 Shared<> 或 Weak<> 默认都认为是值类型方式使用
// 值类型使用模式下，没有 typeId. 只能存放本体，不可以存放派生类


namespace TestNamespace
{
    // 继承自 xx::Object
    class Foo1
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
        xx.Random _random;
        xx.Pos _pos;
    }

    [Struct]
    class A
    {
        Nullable<xx.Pos> _pos;
    }

    [Struct]
    class B : A
    {
        Nullable<float> _float;
    }
}

class Bar
{
    List<TestNamespace.Foo1> foo1s_v;
    List<Shared<TestNamespace.Foo1>> foo1s_s;

    List<TestNamespace.B> bs_v;
}

class Node
{
    Shared<Node> node_s;
    Weak<Node> node_w;
}
