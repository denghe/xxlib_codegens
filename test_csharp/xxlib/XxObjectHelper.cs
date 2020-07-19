using System;
using System.Runtime.InteropServices;
using System.Text;

namespace xx {
    /// <summary>
    /// 对象辅助工具. 针对一批类型 辅助完成序列化, ToString 等功能. 提供上下文支持. 如需跨线程, 则每个线程准备一个 ( ThreadLocalStore ? )
    /// </summary>
    public class ObjectHelper {
        // 用于 len / offset 备份
        public int bak;

        // 反序列化时的创建函数类型
        public delegate IObject CreateFunc();

        // 创建函数数组
        public CreateFunc[] createFuncs = new CreateFunc[ushort.MaxValue + 1];

        // 创建相应 typeId 的创建函数( 通常调用该函数的函数体位于序列化生成物 )
        public void Register<T>(ushort typeId) where T : IObject, new() {
            TypeId<T>.value = typeId;
            createFuncs[typeId] = () => { return new T(); };
        }

        // 根据 typeId 来创建对象
        public IObject CreateByTypeId(ushort typeId) {
            return createFuncs[typeId]();
        }

        // 通过 obj 定位 offset
        public Dict<object, uint> objOffsets = new Dict<object, uint>();

        // 通过 offset 定位 obj
        public Dict<uint, object> offsetObjs = new Dict<uint, object>();

        // 将一个东西写入 data
        public void WriteTo<T>(Data data, T v) where T : IObject {
            var dw = new DataWriter(data, this);
            dw.WriteOnce(v);
        }

        // 从 data 读出一个东西. 注意: 会自动 Hold 根
        public void ReadFrom<T>(Data data, ref T v) where T : IObject {
            var dr = new DataReader(data, this);
            dr.ReadOnce(ref v);
        }

        // 为方便直接返回 new T. 通常返回 null 表示解析失败.
        public T TryReadFrom<T>(Data data) where T : IObject {
            T t = default(T);
            try {
                ReadFrom(data, ref t);
            }
            catch {
                return default(T);
            }
            return t;
        }

        // ToString 用 sb
        public StringBuilder sb = new StringBuilder();

        // 将 v 转为 string( json 长相 )
        public string ToString<T>(T v) where T : IObject {
            objOffsets.Clear();
            sb.Clear();
            if (v == null) {
                sb.Append("null");
            }
            else {
                v.ToString(this);
            }
            return sb.ToString();
        }

        // 比较两个东西的数据是否相同. 利用 ToString. 如果相同，返回 null. 不同则返回具体描述
        string Compare(IObject a, IObject b) {
            throw new NotImplementedException();
        }

        // Clone 用 data
        xx.Data tmp = new Data();

        // 深度拷贝一份 o. 利用序列化 + 反序列化. // todo: 要先备份所有弱引用, 清空, 然后开始序列化. 之后看序列化的字典, 如果和备份有交集, 则交集部分应该在反序列化后 弱引用指向新创建的交集部分的对象。否则指向老的
        T Clone<T>(T o) where T : IObject, new() {
            throw new NotImplementedException();
        }
    }
}
