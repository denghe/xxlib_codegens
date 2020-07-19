using System;

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

        void Destruct();
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
            if(_useCount == 0) {
                Destruct();
            }
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

        public virtual void Destruct() { }

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

    // 可作为类的临时容器
    public struct Shared<T> where T : Object {
        T __t;
        public T value {
            get { return __t; }
            set {
                if (__t != null) __t.Unhold();
                if (value != null) value.Hold();
                __t = value;
            }
        }
        public void Init(T v) {
            __t = v;
        }
    }

    /// <summary>
    /// 存放类型到 typeid 的映射 for 序列化
    /// </summary>
    public sealed class TypeId<T> {
        public static ushort value = 0;
    }
}
