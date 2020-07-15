using System;
using System.Runtime.InteropServices;
using System.Text;

namespace xx {
    public class DataWriter {
        public ObjectHelper oh;

        public Data data;
        public byte[] buf { get { return data.buf; } }
        public int len { get { return data.len; } set { data.len = value; } }
        public int cap { get { return data.buf.Length; } }

        public DataWriter(Data data, ObjectHelper oh) {
            this.data = data;
            this.oh = oh;
        }
        public void Ensure(int len) {
            data.Ensure(len);
        }
        public void Reserve(int cap) {
            data.Reserve(cap);
        }

        #region byte
        public void Write(byte v) {
            if (len + 1 > cap) {
                Reserve(len + 1);
            }
            buf[len++] = v;
        }
        public void Write(byte? v) {
            if (v.HasValue) {
                Write((byte)1);
                Write(v.Value);
            }
            else {
                Write((byte)0);
            }
        }
        #endregion

        #region sbyte
        public void Write(sbyte v) {
            Write((byte)v);
        }
        public void Write(sbyte? v) {
            if (v.HasValue) {
                Write((byte)1);
                Write(v.Value);
            }
            else {
                Write((byte)0);
            }
        }
        #endregion

        #region ushort
        public void Write(ushort v) {
            if (len + 3 > cap) {
                Reserve(len + 3);
            }
            Bit7Write(buf, ref data.len, v);
        }
        public void Write(ushort? v) {
            if (v.HasValue) {
                Write((byte)1);
                Write(v.Value);
            }
            else {
                Write((byte)0);
            }
        }
        #endregion

        #region short
        public void Write(short v) {
            if (len + 3 > cap) {
                Reserve(len + 3);
            }
            Bit7Write(buf, ref data.len, ZigZagEncode(v));
        }
        public void Write(short? v) {
            if (v.HasValue) {
                Write((byte)1);
                Write(v.Value);
            }
            else {
                Write((byte)0);
            }
        }
        #endregion

        #region uint
        public void Write(uint v) {
            if (len + 5 > cap) {
                Reserve(len + 5);
            }
            Bit7Write(buf, ref data.len, v);
        }
        public void Write(uint? v) {
            if (v.HasValue) {
                Write((byte)1);
                Write(v.Value);
            }
            else {
                Write((byte)0);
            }
        }
        public void WriteFixed(uint v) {
            if (len + 4 > cap) {
                Reserve(len + 4);
            }
            buf[len] = (byte)v;
            buf[len + 1] = (byte)(v >> 8);
            buf[len + 2] = (byte)(v >> 16);
            buf[len + 3] = (byte)(v >> 24);
            len += 4;
        }
        public void WriteFixed(int v) {
            if (len + 4 > cap) {
                Reserve(len + 4);
            }
            buf[len] = (byte)v;
            buf[len + 1] = (byte)(v >> 8);
            buf[len + 2] = (byte)(v >> 16);
            buf[len + 3] = (byte)(v >> 24);
            len += 4;
        }
        #endregion

        #region int
        public void Write(int v) {
            if (len + 5 > cap) {
                Reserve(len + 5);
            }
            Bit7Write(buf, ref data.len, ZigZagEncode(v));
        }
        public void Write(int? v) {
            if (v.HasValue) {
                Write((byte)1);
                Write(v.Value);
            }
            else {
                Write((byte)0);
            }
        }
        public void WriteLength(int len) {
            Write((uint)len);
        }
        #endregion

        #region ulong
        public void Write(ulong v) {
            if (len + 10 > cap) {
                Reserve(len + 10);
            }
            Bit7Write(buf, ref data.len, v);
        }
        public void Write(ulong? v) {
            if (v.HasValue) {
                Write((byte)1);
                Write(v.Value);
            }
            else {
                Write((byte)0);
            }
        }
        #endregion

        #region long
        public void Write(long v) {
            if (len + 10 > cap) {
                Reserve(len + 10);
            }
            Bit7Write(buf, ref data.len, ZigZagEncode(v));
        }
        public void Write(long? v) {
            if (v.HasValue) {
                Write((byte)1);
                Write(v.Value);
            }
            else {
                Write((byte)0);
            }
        }
        #endregion

        #region float
        public void WriteDirect(float v) {
            var fu = new FloatingInteger { f = v };
            buf[len + 0] = fu.b0;
            buf[len + 1] = fu.b1;
            buf[len + 2] = fu.b2;
            buf[len + 3] = fu.b3;
            len += 4;
        }
        public void Write(float v) {
            if (len + 4 > cap) {
                Reserve(len + 4);
            }
            WriteDirect(v);
        }
        public void Write(float? v) {
            if (v.HasValue) {
                Write((byte)1);
                Write(v.Value);
            }
            else {
                Write((byte)0);
            }
        }
        #endregion

        #region double
        public void WriteDirect(double v) {
            if (v == 0) {
                buf[len++] = 0;
            }
            else {
                if (double.IsNaN(v)) {
                    buf[len++] = 1;
                }
                else if (double.IsNegativeInfinity(v)) {
                    buf[len++] = 2;
                }
                else if (double.IsPositiveInfinity(v)) {
                    buf[len++] = 3;
                }
                else {
                    int intv = (int)v;
                    if (v == (double)intv) {
                        buf[len++] = 4;
                        Bit7Write(buf, ref data.len, ZigZagEncode(intv));    // Write(intv);
                    }
                    else {
                        buf[len++] = 5;
                        var du = new FloatingInteger { d = v };
                        buf[len + 0] = du.b0;
                        buf[len + 1] = du.b1;
                        buf[len + 2] = du.b2;
                        buf[len + 3] = du.b3;
                        buf[len + 4] = du.b4;
                        buf[len + 5] = du.b5;
                        buf[len + 6] = du.b6;
                        buf[len + 7] = du.b7;
                        len += 8;
                    }
                }
            }
        }
        public void Write(double v) {
            if (len + 10 > cap) {
                Reserve(len + 10);
            }
            WriteDirect(v);
        }
        public void Write(double? v) {
            if (v.HasValue) {
                Write((byte)1);
                Write(v.Value);
            }
            else {
                Write((byte)0);
            }
        }
        #endregion

        #region bool
        public void Write(bool v) {
            Write(v ? (byte)1 : (byte)0);
        }
        public void Write(bool? v) {
            if (v.HasValue) {
                Write((byte)1);
                Write(v.Value);
            }
            else {
                Write((byte)0);
            }
        }
        #endregion

        #region string
        public void Write(string v) {
            var sbuf = Encoding.UTF8.GetBytes(v);
            int sbufLen = sbuf.Length;
            if (len + 5 + sbufLen > cap) {
                Reserve(len + 5 + sbufLen);
            }
            Bit7Write(buf, ref data.len, (uint)sbufLen);
            Buffer.BlockCopy(sbuf, 0, buf, len, sbufLen);
            len += sbufLen;
        }
        //public void Write(xx.Optional<string> v) {
        //    if(v.HasValue) {
        //        Write((byte)1);
        //        Write(v.Value);
        //    }
        //    else {
        //        Write((byte)0);
        //    }
        //}
        #endregion

        public void Write<T>(List<T> v) {
            if (v == null) throw new Exception("v is null");
            v.Serialize(this);
        }
        //public void Write(xx.Optional<List<T>> v) {
        //    if(v.HasValue) {
        //        Write((byte)1);
        //        Write(v.Value);
        //    }
        //    else {
        //        Write((byte)0);
        //    }
        //}

        public void Write<T>(T v) where T : IObject {
            if (v == null) {
                Write((ushort)0);
            }
            else {
                var typeId = v.GetTypeId();
                if (typeId == 0) throw new Exception("no register typeId? type: " + typeof(T).Name);
                Write(v.GetTypeId());
                if (oh.objOffsets != null) {
                    var rtv = oh.objOffsets.Add(v, (uint)(len - oh.bak));          // 试将 v 和 相对offset 放入字典, 得到下标和是否成功
                    Write(oh.objOffsets.ValueAt(rtv.index));                            // 取 offset ( 不管是否成功 )
                    if (!rtv.success) return;                                           // 如果首次出现就序列化类本体
                }
                v.Serialize(this);
            }
        }
        public void WriteOnce<T>(T v) where T : IObject {
            oh.objOffsets.Clear();
            oh.bak = len;
            Write(v);
        }

        #region utils
        // 负转正：利用单数来存负数，双数来存正数
        // 等效代码： if( v < 0 ) return -v * 2 - 1; else return v * 2;

        public static ushort ZigZagEncode(short v) { return (ushort)((v << 1) ^ (v >> 15)); }

        public static uint ZigZagEncode(int v) { return (uint)((v << 1) ^ (v >> 31)); }

        public static ulong ZigZagEncode(long v) { return (ulong)((v << 1) ^ (v >> 63)); }

        // need ensure 5
        public static void Bit7Write(byte[] buf, ref int offset, uint v) {
            while (v >= 1 << 7) {
                buf[offset++] = (byte)(v & 0x7fu | 0x80u);
                v >>= 7;
            };
            buf[offset++] = (byte)v;
        }

        // 同样的实现两份是考虑到 32位 cpu 用 ulong 操作没效率
        // need ensure 10
        public static void Bit7Write(byte[] buf, ref int offset, ulong v) {
            while (v >= 1 << 7) {
                buf[offset++] = (byte)(v & 0x7fu | 0x80u);
                v >>= 7;
            };
            buf[offset++] = (byte)v;
        }
        #endregion
    }
}
