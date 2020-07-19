using System;

public class Test {
    xx.ObjectHelper oh = new xx.ObjectHelper();
    public Test() {
        PKG.PkgGenTypes.RegisterTo(oh);
    }

    #region std::shared_ptr<Node> n
    PKG.Node __n;
    public PKG.Node n {
        get { return __n; }
        set {
            if (__n != null) __n.Unhold();
            if (value != null) value.Hold();
            __n = value;
        }
    }
    #endregion

    #region std::shared_ptr<Node> n2
    PKG.Node __n2;
    public PKG.Node n2 {
        get { return __n2; }
        set {
            if (__n2 != null) __n2.Unhold();
            if (value != null) value.Hold();
            __n2 = value;
        }
    }
    #endregion

    public void Run() {
        n = new PKG.Node();
        n.childs.Add(new PKG.Node { parent = n });
        n.childs.Add(new PKG.Node { parent = n });
        Console.WriteLine(oh.ToString(n));

        var data = new xx.Data();
        oh.WriteTo(data, n);
        Console.WriteLine(oh.ToString(data));

        oh.ReadFrom(data, ref __n2);
        Console.WriteLine(oh.ToString(n2));
    }
}

class Program {
    static void Main(string[] args) {
        var t = new Test();
        t.Run();

        Console.ReadLine();
        return;
    }
}
