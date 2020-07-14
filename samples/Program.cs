using System;

namespace xx {
    class Object {
        public int useCount { get; private set; }
        public void Hold() {
            ++useCount;
        }
        public void Unhold() {
            --useCount;
        }
    }

    // 意思意思
    class List<T> where T : Object, new() {
        System.Collections.Generic.List<T> items = new System.Collections.Generic.List<T>();
        public void Add(T v) {
            v?.Hold();
            items.Add(v);

        }
        public void RemoveAt(int idx) {
            items[idx]?.Unhold();
            items.RemoveAt(idx);
        }
        public T this[int idx] {
            get { return items[idx]; }
            set {
                items[idx]?.Unhold();
                value.Hold();
                items[idx] = value;
            }
        }
    }
}

class Node : xx.Object {
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
}

class Env {
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
        n.childs.Add(new Node());
        Console.WriteLine(n.useCount + ", " + n.childs[0].useCount);

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
    }
}
