using System;
namespace PKG {
    #region GenMD5, GenTypes.RegisterTo
    public static class PkgGenMd5 {
        public const string value = "#*MD5<f626e32063a5ce4e89d989e640269d21>*#"; 
    }
    public static class PkgGenTypes {
        public static void RegisterTo(xx.ObjectHelper oh) {
	        oh.Register<Node>(34);
	        oh.Register<Scene>(12);
        }
    }
    #endregion
    public partial class Node : xx.Object {
        #region std::weak_ptr<Node> parent
        Node __parent;
        public Node parent {
            get {
                if (__parent != null && __parent.useCount == 0) {
                    __parent = null;
                }
                return __parent;
            }
            set { __parent = value; }
        }
        #endregion
        #region std::vector<std::shared_ptr<Node>> childs
        xx.List<Node> __childs = new xx.List<Node>();
        public xx.List<Node> childs {
            get {
                return __childs;
            }
        }
        #endregion
        #region IObject impls
        public override ushort GetTypeId() {
            return xx.TypeId<Node>.value;
        }

        public override void Serialize(xx.DataWriter dw) {
            dw.Write(this.parent);
            dw.Write(this.childs);
        }

        public override void Deserialize(xx.DataReader dr) {
            {
                __parent = null;
                dr.Read(ref __parent);
            }
            dr.Read(ref __childs);
        }
        public override void ToString(xx.ObjectHelper oh) {
            base.ToString(oh);
        }
        public override void ToStringCore(xx.ObjectHelper oh) {var s = oh.sb;
            s.Append(",\"parent\":");
            if (parent == null) s.Append("null");
            else {
                parent.ToString(oh);
            }
            s.Append(",\"childs\":");
            childs.ToString(oh);
        }
        public override string ToString() {
            return "";
        }
        #endregion
    }
    public partial class Scene : Node {
        #region IObject impls
        public override ushort GetTypeId() {
            return xx.TypeId<Scene>.value;
        }

        public override void Serialize(xx.DataWriter dw) {
            base.Serialize(dw);
        }

        public override void Deserialize(xx.DataReader dr) {
            base.Deserialize(dr);
        }
        public override void ToString(xx.ObjectHelper oh) {
            base.ToString(oh);
        }
        public override void ToStringCore(xx.ObjectHelper oh) {
            base.ToStringCore(oh);
        }
        public override string ToString() {
            return "";
        }
        #endregion
    }
}
