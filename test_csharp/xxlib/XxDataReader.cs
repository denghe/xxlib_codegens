using System;
using System.Runtime.InteropServices;
using System.Text;

namespace xx {
    public class DataReader {
        public ObjectHelper oh;

        public Data data;
        public byte[] buf { get { return data.buf; } }
        public int len { get { return data.len; } set { data.len = value; } }
        public int offset;

        public DataReader(Data data, ObjectHelper oh) {
            this.data = data;
            this.oh = oh;
        }

        #region byte
        public void Read(ref byte v) {
            v = buf[offset++];
        }
        public void Read(ref byte? v) {
            byte hasValue = 0;
            Read(ref hasValue);
            if (hasValue == 1) {
                var tmp = default(byte);
                Read(ref tmp);
                v = tmp;
            }
            else {
                v = null;
            }
        }
        #endregion

        #region sbyte
        public void Read(ref sbyte v) {
            v = (sbyte)buf[offset++];
        }
        public void Read(ref sbyte? v) {
            byte hasValue = 0;
            Read(ref hasValue);
            if (hasValue == 1) {
                var tmp = default(sbyte);
                Read(ref tmp);
                v = tmp;
            }
            else {
                v = null;
            }
        }
        #endregion

        #region ushort
        public void Read(ref ushort v) {
            Bit7Read(ref v, buf, ref offset, len);
        }
        public void Read(ref ushort? v) {
            byte hasValue = 0;
            Read(ref hasValue);
            if (hasValue == 1) {
                var tmp = default(ushort);
                Read(ref tmp);
                v = tmp;
            }
            else {
                v = null;
            }
        }
        #endregion

        #region short
        public void Read(ref short v) {
            ushort tmp = 0;
            Bit7Read(ref tmp, buf, ref offset, len);
            v = ZigZagDecode(tmp);
        }
        public void Read(ref short? v) {
            byte hasValue = 0;
            Read(ref hasValue);
            if (hasValue == 1) {
                var tmp = default(short);
                Read(ref tmp);
                v = tmp;
            }
            else {
                v = null;
            }
        }
        #endregion

        #region uint
        public void Read(ref uint v) {
            Bit7Read(ref v, buf, ref offset, len);
        }
        public void Read(ref uint? v) {
            byte hasValue = 0;
            Read(ref hasValue);
            if (hasValue == 1) {
                var tmp = default(uint);
                Read(ref tmp);
                v = tmp;
            }
            else {
                v = null;
            }
        }
        public void ReadFixed(ref uint v) {
            if (len < offset + 4) {
                throw new Exception("out of range");
            }
            v = (uint)(buf[offset] | (buf[offset + 1] << 8) | (buf[offset + 2] << 16) | (buf[offset + 3] << 24));
            offset += 4;
        }
        #endregion

        #region int
        public void Read(ref int v) {
            uint tmp = 0;
            Bit7Read(ref tmp, buf, ref offset, len);
            v = ZigZagDecode(tmp);
        }
        public void Read(ref int? v) {
            byte hasValue = 0;
            Read(ref hasValue);
            if (hasValue == 1) {
                var tmp = default(int);
                Read(ref tmp);
                v = tmp;
            }
            else {
                v = null;
            }
        }
        public int ReadLength() {
            uint tmp = 0;
            Bit7Read(ref tmp, buf, ref offset, len);
            return (int)tmp;
        }
        #endregion

        #region ulong
        public void Read(ref ulong v) {
            Bit7Read(ref v, buf, ref offset, len);
        }
        public void Read(ref ulong? v) {
            byte hasValue = 0;
            Read(ref hasValue);
            if (hasValue == 1) {
                var tmp = default(ulong);
                Read(ref tmp);
                v = tmp;
            }
            else {
                v = null;
            }
        }
        #endregion

        #region long
        public void Read(ref long v) {
            ulong tmp = 0;
            Bit7Read(ref tmp, buf, ref offset, len);
            v = ZigZagDecode(tmp);
        }
        public void Read(ref long? v) {
            byte hasValue = 0;
            Read(ref hasValue);
            if (hasValue == 1) {
                var tmp = default(long);
                Read(ref tmp);
                v = tmp;
            }
            else {
                v = null;
            }
        }
        #endregion

        #region float
        public void Read(ref float v) {
            var fu = new FloatingInteger {
                b0 = buf[offset + 0],
                b1 = buf[offset + 1],
                b2 = buf[offset + 2],
                b3 = buf[offset + 3]
            };
            v = fu.f;
            offset += 4;
        }
        public void Read(ref float? v) {
            byte hasValue = 0;
            Read(ref hasValue);
            if (hasValue == 1) {
                var tmp = default(float);
                Read(ref tmp);
                v = tmp;
            }
            else {
                v = null;
            }
        }
        #endregion

        #region double
        public void Read(ref double v) {
            switch (buf[offset++]) {
                case 0:
                    v = 0;
                    break;
                case 1:
                    v = double.NaN;
                    break;
                case 2:
                    v = double.NegativeInfinity;
                    break;
                case 3:
                    v = double.PositiveInfinity;
                    break;
                case 4:
                    uint tmp = 0;
                    Bit7Read(ref tmp, buf, ref offset, len);
                    v = ZigZagDecode(tmp);
                    break;
                case 5:
                    var du = new FloatingInteger {
                        b0 = buf[offset + 0],
                        b1 = buf[offset + 1],
                        b2 = buf[offset + 2],
                        b3 = buf[offset + 3],
                        b4 = buf[offset + 4],
                        b5 = buf[offset + 5],
                        b6 = buf[offset + 6],
                        b7 = buf[offset + 7]
                    };
                    v = du.d;
                    offset += 8;
                    break;
                default:
                    throw new NotSupportedException();
            }
        }
        public void Read(ref double? v) {
            byte hasValue = 0;
            Read(ref hasValue);
            if (hasValue == 1) {
                var tmp = default(double);
                Read(ref tmp);
                v = tmp;
            }
            else {
                v = null;
            }
        }
        #endregion

        #region bool
        public void Read(ref bool v) {
            v = buf[offset++] == 1;
        }
        public void Read(ref bool? v) {
            byte hasValue = 0;
            Read(ref hasValue);
            if (hasValue == 1) {
                var tmp = default(bool);
                Read(ref tmp);
                v = tmp;
            }
            else {
                v = null;
            }
        }
        #endregion

        #region string
        public void Read(ref string v) {
            uint len = 0;
            Read(ref len);
            if (len == 0) {
                v = "";
            }
            else {
                v = Encoding.UTF8.GetString(buf, offset, (int)len);
                offset += (int)len;
            }
        }
        //public void Read(ref xx.Optional<string> v) {
        //    byte hasValue = 0;
        //    Read(ref hasValue);
        //    if(hasValue == 1) {
        //        string s = null;
        //        Read(ref s);
        //        v.value = s;
        //    }
        //    else {
        //        v.Clear();
        //    }
        //}
        #endregion

        public void Read<T>(ref List<T> v) {
            v.Deserialize(this);
        }

        public void Read<T>(ref T v) where T : IObject {
            var typeId = (ushort)0;
            Read(ref typeId);
            if (typeId == 0) {
                v = default(T);
                return;
            }
            uint ptr_offset = 0, bb_offset_bak = (uint)(offset - oh.bak);
            Read(ref ptr_offset);
            if (ptr_offset == bb_offset_bak) {
                v = (T)oh.CreateByTypeId(typeId);
                if (v == null) throw new Exception("create type failed. typeId = " + typeId);

                // 自动加持第一个
                if (oh.offsetObjs.Count == 0) {
                    ((Object)(object)v).Hold();
                }
                oh.offsetObjs.Add(ptr_offset, v);
                v.Deserialize(this);
            }
            else {
                int idx = oh.offsetObjs.Find(ptr_offset);
                if (idx == -1) throw new Exception("ptr_offset is not found");
                v = (T)oh.offsetObjs.ValueAt(idx);

                if (v == null) throw new Exception("idxStore not found: " + idx);
            }
        }
        public void ReadOnce<T>(ref T v) where T : IObject {
            oh.offsetObjs.Clear();
            oh.bak = offset;
            Read(ref v);
        }

        #region utils

        // 等效代码： if( (v & 1) > 0 ) return -(v + 1) / 2; else return v / 2;

        public static short ZigZagDecode(ushort v) { return (short)((short)(v >> 1) ^ (-(short)(v & 1))); }

        public static int ZigZagDecode(uint v) { return (int)(v >> 1) ^ (-(int)(v & 1)); }

        public static long ZigZagDecode(ulong v) { return (long)(v >> 1) ^ (-(long)(v & 1)); }

        public static void Bit7Read(ref ulong v, byte[] buf, ref int offset, int dataLen) {
            v = 0;
            for (int shift = 0; shift < /*sizeof(T)*/8 * 8; shift += 7) {
                if (offset == dataLen) throw new OverflowException();
                ulong b = buf[offset++];
                v |= (b & 0x7Fu) << shift;
                if ((b & 0x80) == 0) return;
            }
            throw new OverflowException();
        }

        public static void Bit7Read(ref uint v, byte[] buf, ref int offset, int dataLen) {
            v = 0;
            for (int shift = 0; shift < /*sizeof(T)*/4 * 8; shift += 7) {
                if (offset == dataLen) throw new OverflowException();
                uint b = buf[offset++];
                v |= (b & 0x7Fu) << shift;
                if ((b & 0x80) == 0) return;
            }
            throw new OverflowException();
        }

        public static void Bit7Read(ref ushort v, byte[] buf, ref int offset, int dataLen) {
            v = 0;
            for (int shift = 0; shift < /*sizeof(T)*/2 * 8; shift += 7) {
                if (offset == dataLen) throw new OverflowException();
                uint b = buf[offset++];
                v |= (ushort)((b & 0x7Fu) << shift);
                if ((b & 0x80) == 0) return;
            }
            throw new OverflowException();
        }

        #endregion
    }
}
