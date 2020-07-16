using System;
using System.Runtime.InteropServices;
using System.Text;

namespace xx {
    /// <summary>
    /// 基础数据容器. 数组 + 数据长度. 序列化时当作值类型处理( 直接序列化, 不探索引用关系 )
    /// </summary>
    public class Data : IObject {
        public byte[] buf = new byte[256];
        public int len;
        public int cap { get { return buf.Length; } }

        public void Ensure(int len) {
            if (this.len + len > cap) {
                Reserve(this.len + len);
            }
        }

        public void Reserve(int capacity) {
            if (capacity <= cap) return;
            Array.Resize(ref this.buf, (int)Round2n((uint)(capacity * 2)));
        }

        public void Resize(int len) {
            if (len == this.len) return;
            else if (len < this.len) {
                Array.Clear(buf, len, this.len - len);
            }
            else // len > dataLen
            {
                Reserve(len);
            }
            this.len = len;
        }

        /// <summary>
        /// 直接追加写入一段2进制数据
        /// </summary>
        public void WriteBuf(byte[] buf, int offset, int dataLen) {
            this.Reserve(dataLen + this.len);
            Buffer.BlockCopy(buf, offset, this.buf, this.len, dataLen);
            this.len += dataLen;
        }

        /// <summary>
        /// 直接追加写入一段2进制数据之指针版
        /// </summary>
        public void WriteBuf(IntPtr bufPtr, int len) {
            this.Reserve(this.len + len);
            Marshal.Copy(bufPtr, this.buf, this.len, len);
            this.len += len;
        }

        public ushort GetTypeId() {
            return TypeId<Data>.value;
        }

        public void Serialize(DataWriter dw) {
            var len = this.len;
            dw.WriteLength(len);
            if (len == 0) return;
            dw.Ensure(len);
            Buffer.BlockCopy(buf, 0, dw.buf, dw.len, len);
            dw.len += len;
        }

        public void Deserialize(DataReader dr) {
            int len = dr.ReadLength();
            Resize(len);
            dr.offset = 0;
            if (len == 0) return;
            Buffer.BlockCopy(dr.buf, dr.offset, buf, 0, len);
            dr.offset += len;
        }

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

        public static void DumpAscII(ref StringBuilder s, byte[] buf, int offset, int len) {
            for (int i = offset; i < offset + len; ++i) {
                byte c = buf[i];
                if (c < 32 || c > 126)
                    s.Append('.');
                else
                    s.Append((char)c);
            }
        }

        // write buf's binary dump text to s
        public static void Dump(ref StringBuilder s, byte[] buf, int len = 0) {
            if (buf == null || buf.Length == 0)
                return;
            if (len == 0)
                len = buf.Length;
            s.Append("--------  0  1  2  3 | 4  5  6  7 | 8  9  A  B | C  D  E  F");
            s.Append("   dataLen = " + len);
            int i = 0;
            for (; i < len; ++i) {
                if ((i % 16) == 0) {
                    if (i > 0) {           // refput ascii to the end of the line
                        s.Append("  ");
                        DumpAscII(ref s, buf, i - 16, 16);
                    }
                    s.Append('\n');
                    s.Append(i.ToString("x8"));
                    s.Append("  ");
                }
                else if ((i > 0 && (i % 4 == 0))) {
                    s.Append("  ");
                }
                else
                    s.Append(' ');
                s.Append(BitConverter.ToString(buf, i, 1));
            }
            int left = i % 16;
            if (left == 0) {
                left = 16;
            }
            if (left > 0) {
                len = len + 16 - left;
                for (; i < len; ++i) {
                    if (i > 0 && (i % 4 == 0))
                        s.Append("  ");
                    else
                        s.Append(' ');
                    s.Append("  ");
                }
                s.Append("  ");
                DumpAscII(ref s, buf, i - 16, left);
            }
            s.Append('\n');
        }

        public static string Dump(byte[] buf, int len = 0) {
            var sb = new StringBuilder();
            Dump(ref sb, buf, len);
            return sb.ToString();
        }
    }
}
