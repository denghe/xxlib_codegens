using System;
using System.Diagnostics;

public class Node : xx.Object {
    #region std::weak_ptr<Node> parent
    Node _parent;
    public Node parent {
        get {
            if (_parent?.useCount == 0) {
                _parent = null;
            }
            return _parent;
        }
        set { _parent = value; }
    }
    #endregion
    #region std::vector<std::shared_ptr<Node>> childs
    xx.List<Node> _childs = new xx.List<Node>();
    public xx.List<Node> childs {
        get { return _childs; }
    }
    #endregion
    #region std::vector<std::weak_ptr<Node>> weakChilds
    xx.List<xx.Weak<Node>> _weakChilds = new xx.List<xx.Weak<Node>>();
    public xx.List<xx.Weak<Node>> weakChilds {
        get { return _weakChilds; }
    }
    #endregion
}

public class Env {
    #region std::shared_ptr<Node> n
    Node _n;
    public Node n {
        get { return _n; }
        set {
            _n?.Unhold();
            _n = value;
            _n?.Hold();
        }
    }
    #endregion
    #region std::shared_ptr<Node> n1
    Node _n1;
    public Node n1 {
        get { return _n1; }
        set {
            _n1?.Unhold();
            value?.Hold();
            _n1 = value;
        }
    }
    #endregion
    #region std::weak_ptr<Node> n2
    Node _n2;
    public Node n2 {
        get {
            if (_n2?.useCount == 0) {
                _n2 = null;
            }
            return _n2;
        }
        set { _n2 = value; }
    }
    #endregion
    public void Test() {
        n = new Node();
        n.parent = n;
        var c = new Node();
        n.childs.Add(c);
        n.childs.Add(c);
        n.childs.Add(c);
        n.weakChilds.Add(c);
        n.weakChilds.Add(c);
        n.weakChilds.Add(c);
        Console.WriteLine(n.useCount + ", " + c.useCount);
        n.childs.Clear();
        Console.WriteLine(n.useCount + ", " + c.useCount);

        n1 = n;
        n2 = n;
        Console.WriteLine(n2?.useCount);
        n = null;
        Console.WriteLine(n2?.useCount);
        n1 = null;
        Console.WriteLine(n2?.useCount);
    }
}

class Program {
    static void Main(string[] args) {
        var env = new Env();
        env.Test();

        var data = new xx.Data();
        var oh = new xx.ObjectHelper();
        var dw = new xx.DataWriter(data, oh);
        var L = new xx.List<int>();
        L.Add(1);
        L.Add(2);
        dw.Write(L);
        Console.WriteLine(oh.ToString(data));
        Console.ReadLine();
        return;

        long counter = 0;
        var sw = Stopwatch.StartNew();
        {
            counter = 0;
            sw.Restart();
            var list = new System.Collections.Generic.List<int>();
            for (int i = 0; i < 10000000; i++) {
                list.Add(i);
                counter += list[0];
                //list.Clear();
            }
            Console.WriteLine(sw.ElapsedMilliseconds);
            Console.WriteLine(counter);

            counter = 0;
            sw.Restart();
            for (int i = 0; i < 50000; i++) {
                if (list.Find(o => o == i) != -1) {
                    ++counter;
                }
            }
            Console.WriteLine(sw.ElapsedMilliseconds);
            Console.WriteLine(counter);
        }

        {
            counter = 0;
            sw.Restart();
            var list = new xx.List<int>();
            for (int i = 0; i < 10000000; i++) {
                list.Add(i);
                counter += list[0];
                //list.Clear();
            }
            Console.WriteLine(sw.ElapsedMilliseconds);
            Console.WriteLine(counter);

            counter = 0;
            sw.Restart();
            for (int i = 0; i < 50000; i++) {
                if (list.Find(o => o == i) != -1) {
                    ++counter;
                }
            }
            Console.WriteLine(sw.ElapsedMilliseconds);
            Console.WriteLine(counter);
        }

        Console.ReadLine();
    }
}
