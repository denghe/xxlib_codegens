using System;
using System.Runtime.InteropServices;

namespace xx {
    /// <summary>
    /// 基础数据容器. 数组 + 数据长度. 序列化时当作值类型处理( 直接序列化, 不探索引用关系 )
    /// </summary>
    public class Data : IObject {
        /// <summary>
        /// 指向数据容器
        /// </summary>
        public byte[] buf;

        /// <summary>
        /// 数据容器中的数据长度
        /// </summary>
        public int len;

        /// <summary>
        /// 数据容器长度上限
        /// </summary>
        public int cap {
            get {
                return buf.Length;
            }
        }

        /// <summary>
        /// 可输入一串数字来初始化 buf 的内容 以方便临时调试
        /// </summary>
        public Data(params byte[] args) {
            if (args != null) {
                buf = args;
            }
            else {
                buf = new byte[0];
            }
        }

        /// <summary>
        /// 预申请 buf 空间
        /// </summary>
        public void Reserve(int capacity) {
            if (capacity <= cap) return;
            Array.Resize(ref this.buf, (int)Round2n((uint)(capacity)));
        }

        /// <summary>
        /// 在当前数据已有长度的基础上确保至少还有 len 字节可用
        /// </summary>
        public void Ensure(int len) {
            Reserve(this.len + len);
        }

        /// <summary>
        /// 直接设置数据长
        /// </summary>
        public void Resize(int len) {
            Reserve(len);
            this.len = len;
        }

        /// <summary>
        /// 清理数据
        /// </summary>
        public void Clear() {
            len = 0;
        }

        /// <summary>
        /// 直接追加写入一段2进制数据
        /// </summary>
        public void WriteBuf(byte[] buf, int offset, int len) {
            Reserve(len + this.len);
            Buffer.BlockCopy(buf, offset, this.buf, this.len, len);
            this.len += len;
        }

        /// <summary>
        /// 直接追加写入一段2进制数据之指针版
        /// </summary>
        public void WriteBuf(IntPtr bufPtr, int len) {
            Reserve(this.len + len);
            Marshal.Copy(bufPtr, buf, this.len, len);
            this.len += len;
        }


        /// <summary>
        /// 返回 typeId
        /// </summary>
        public ushort GetTypeId() {
            return TypeId<Data>.value;
        }

        /// <summary>
        /// 序列化. 先写入长度，再写入数据
        /// </summary>
        public void Serialize(DataWriter dw) {
            dw.WriteLength(len);
            if (len == 0) return;
            dw.Ensure(len);
            Buffer.BlockCopy(buf, 0, dw.buf, dw.len, len);
            dw.len += len;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        public void Deserialize(DataReader dr) {
            int len = dr.ReadLength();
            Resize(len);
            if (len == 0) return;
            Buffer.BlockCopy(dr.buf, dr.offset, buf, 0, len);
            dr.offset += len;
        }

        /// <summary>
        /// 配合输出 json 长相
        /// </summary>
        public void ToString(ObjectHelper oh) {
            var sb = oh.sb;
            sb.Append('[');
            for (int i = 0; i < len; ++i) {
                sb.Append(buf[i].ToString());
                sb.Append(',');
            }
            if (len > 0) {
                sb[sb.Length - 1] = ']';
            }
            else {
                sb.Append(']');
            }
        }

        public void ToStringCore(ObjectHelper oh) {
        }

        /// <summary>
        /// 统计有多少个 1
        /// </summary>
        public static int Popcnt(uint x) {
            x -= ((x >> 1) & 0x55555555);
            x = (((x >> 2) & 0x33333333) + (x & 0x33333333));
            x = (((x >> 4) + x) & 0x0f0f0f0f);
            x += (x >> 8);
            x += (x >> 16);
            return (int)(x & 0x0000003f);
        }

        /// <summary>
        /// 统计高位有多少个 0
        /// </summary>
        public static int Clz(uint x) {
            x |= (x >> 1);
            x |= (x >> 2);
            x |= (x >> 4);
            x |= (x >> 8);
            x |= (x >> 16);
            return (int)(32 - Popcnt(x));
        }

        /// <summary>
        /// 求大于 v 的 2^n 值
        /// </summary>
        public static uint Round2n(uint v) {
            int bits = 31 - Clz(v);
            var rtv = (uint)(1u << bits);
            if (rtv == v) return v;
            return rtv << 1;
        }

    }

    /// <summary>
    /// 用于浮点到各长度整型的快速转换 
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 8, CharSet = CharSet.Ansi)]
    public struct FloatingInteger {
        [FieldOffset(0)] public double d;
        [FieldOffset(0)] public ulong ul;
        [FieldOffset(0)] public float f;
        [FieldOffset(0)] public uint u;
        [FieldOffset(0)] public byte b0;
        [FieldOffset(1)] public byte b1;
        [FieldOffset(2)] public byte b2;
        [FieldOffset(3)] public byte b3;
        [FieldOffset(4)] public byte b4;
        [FieldOffset(5)] public byte b5;
        [FieldOffset(6)] public byte b6;
        [FieldOffset(7)] public byte b7;
    }
}
