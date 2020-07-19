using System;

/*
    // shared_ptr 变量的写法示例

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
*/

namespace xx {
    /// <summary>
    /// 序列化基础接口. 主要为兼容 struct
    /// </summary>
    public interface IObject {
        // 获取包编号( 不能用 get, 无法虚覆盖 )
        ushort GetTypeId();

        void Serialize(DataWriter dw);
        void Deserialize(DataReader dr);

        // ToString 相关
        void ToString(ObjectHelper oh);
        void ToStringCore(ObjectHelper oh);
    }

    /// <summary>
    /// 类似 C++ 下的 xx::Object
    /// </summary>
    public class Object : IObject {
        protected int _useCount = 0;
        public int useCount { get { return _useCount; } }
        public void Hold() {
            ++_useCount;
        }
        public void Unhold() {
            --_useCount;
        }

        public virtual ushort GetTypeId() { return 0; }

        public virtual void Serialize(DataWriter dw) { }
        public virtual void Deserialize(DataReader dr) { }

        public virtual void ToString(ObjectHelper oh) {
            var sb = oh.sb;
            var iter = oh.objOffsets.Find(this);
            if (iter != -1) {
                sb.Append(oh.objOffsets.ValueAt(iter));
                return;
            }
            else {
                oh.objOffsets[this] = (uint)sb.Length;
            }
            sb.Append("{\"#\":" + GetTypeId());
            ToStringCore(oh);
            sb.Append('}');
        }
        public virtual void ToStringCore(ObjectHelper oh) { }

        // 作为接收变量默认值比较方便
        public static readonly Object instance = new Object();
    }



    // List 模板适配时需要
    public interface IWeak {
        bool NotNull();
        IObject Lock();
        void Reset(IObject o);
    }

    // 放入 List 才会用到. 避免 List<Object> 自动 Hold Unhold
    public struct Weak<T> : IWeak where T : Object {
        public T pointer;

        public IObject Lock() {
            return NotNull() ? pointer : null;
        }

        public void Reset(IObject o = null) {
            pointer = (T)o;
        }

        public bool NotNull() {
            if (pointer != null) {
                if (pointer.useCount == 0) {
                    pointer = null;
                    return false;
                }
                return true;
            }
            return false;
        }

        public static implicit operator Weak<T>(T p) {
            return new Weak<T> { pointer = p };
        }

        public override string ToString() {
            throw new Exception("can't call this tostring");
        }
    }


    /// <summary>
    /// 存放类型到 typeid 的映射 for 序列化
    /// </summary>
    public sealed class TypeId<T> {
        public static ushort value = 0;
    }
}
