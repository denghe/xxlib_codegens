using System;
using System.Collections.Generic;

namespace xx {
    // 抄自 MS 代码并删除小改加料( 支持多级序列化. 当元素派生至 xx.Object 时自动 Hold / Unhold. 如果不需要 Hold / Unhold 就用 Weak 包裹 )
    // Add Remove 数值类型性能减半( is Object 判断导致 ), 检索性能不变
    public class List<T> : Object {
        public T[] buf;
        public int len;
        public int cap { get { return buf.Length; } }

        static readonly T[] emptyBuf = new T[0];
        public List() {
            buf = emptyBuf;
        }

        // 从 System.Collections.Generic.List 转
        public List(System.Collections.Generic.List<T> list) {
            if (typeof(IObject).IsAssignableFrom(typeof(T))) throw new Exception("not support this type");
            list.CopyTo(buf);
        }

        // 转为 System.Collections.Generic.List
        public System.Collections.Generic.List<T> ToGenericList() {
            if (typeof(IObject).IsAssignableFrom(typeof(T))) throw new Exception("not support this type");
            var list = new System.Collections.Generic.List<T>();
            for (int i = 0; i < len; i++) {
                list.Add(buf[i]);
            }
            return list;
        }

        public T this[int index] {
            get {
                if (!(index >= 0 && index < len)) throw new Exception("out of range");
                return buf[index];
            }
            set {
                if (!(index >= 0 && index < len)) throw new Exception("out of range");
                if (buf[index] is Object o2) {
                    o2.Unhold();
                }
                if (value is Object o) {
                    o.Hold();
                }
                buf[index] = value;
            }
        }

        public void Add(T item) {
            if (len == buf.Length) {
                Reserve(len + 1);
            }
            if (item is Object o) {
                o.Hold();
            }
            buf[len++] = item;
        }

        public void Clear() {
            if (len > 0) {
                for (int i = 0; i < len; i++) {
                    if (buf[i] is Object o) {
                        o.Unhold();
                    }
                }
                Array.Clear(buf, 0, len);
                len = 0;
            }
        }

        public void Reserve(int min) {
            if (buf.Length < min) {
                int newCapacity = buf.Length == 0 ? 4 : buf.Length * 2;
                if (newCapacity < min) newCapacity = min;
                if (newCapacity < buf.Length) return;
                T[] newItems = new T[newCapacity];
                if (len > 0) {
                    Array.Copy(buf, 0, newItems, 0, len);
                }
                buf = newItems;
            }
        }

        public int Find(Predicate<T> match) {
            if (!(match != null)) throw new Exception("match is null");
            for (int i = 0; i < len; i++) {
                if (match(buf[i])) {
                    return i;
                }
            }
            return -1;
        }

        public int Find(T item) {
            if ((object)item == null) {
                for (int i = 0; i < len; i++)
                    if ((object)buf[i] == null)
                        return i;
            }
            else {
                var c = EqualityComparer<T>.Default;
                for (int i = 0; i < len; i++) {
                    if (c.Equals(buf[i], item)) return i;
                }
            }
            return -1;
        }

        public void ForEach(Action<T> action) {
            if (!(action != null)) throw new Exception("action is null");
            for (int i = 0; i < len; i++) {
                action(buf[i]);
            }
        }

        public void ForEachReverse(Action<T> action) {
            if (!(action != null)) throw new Exception("action is null");
            for (int i = len - 1; i >= 0; --i) {
                action(buf[i]);
            }
        }

        public void Insert(int index, T item) {
            if (!((uint)index <= (uint)len)) throw new Exception("out of range");
            if (len == buf.Length) Reserve(len + 1);
            if (index < len) {
                Array.Copy(buf, index, buf, index + 1, len - index);
            }
            if (item is Object o) {
                o.Hold();
            }
            buf[index] = item;
            len++;
        }

        public bool Remove(T item) {
            int index = Find(item);
            if (index >= 0) {
                RemoveAt(index);
                return true;
            }
            return false;
        }

        public void RemoveAt(int index) {
            if (!((uint)index < (uint)len)) throw new Exception("out of range");
            if (buf[index] is Object o) {
                o.Unhold();
            }
            len--;
            if (index < len) {
                Array.Copy(buf, index + 1, buf, index, len - index);
            }
            buf[len] = default(T);
        }

        public void Sort() {
            Sort(0, len, null);
        }

        public void Sort(IComparer<T> comparer) {
            Sort(0, len, comparer);
        }

        public void Sort(int index, int count, IComparer<T> comparer) {
            if (!(index >= 0)) throw new Exception("out of range");
            if (!(count >= 0)) throw new Exception("out of range");
            if (!(len - index >= count)) throw new Exception("out of range");

            Array.Sort<T>(buf, index, count, comparer);
        }


        /****************************************************************/
        // 为方便加的料
        /****************************************************************/

        public void Resize(int len) {
            if (len == this.len) return;
            else if (len < this.len) {
                for (int i = len; i < this.len; i++) {
                    if (buf[i] is Object o) {
                        o.Unhold();
                    }
                }
                Array.Clear(buf, len, this.len - len);
            }
            else // len > dataLen
            {
                Reserve(len);
            }
            this.len = len;
        }

        /// <summary>
        /// 将最后一个覆盖至当前 index, 并令 dataLen--
        /// </summary>
        public void SwapRemoveAt(int index) {
            if (index + 1 < len) {
                buf[index] = buf[len - 1];
            }
            len--;
            if (buf[len] is Object o) {
                o.Unhold();
            }
            buf[len] = default(T);
        }


        /// <summary>
        /// 查找所有满足条件的元素塞到新 List 并返回. 未找到返回元素个数为 0 的 List
        /// </summary>
        public List<T> Where(Predicate<T> match) {
            var rtv = new List<T>();
            if (!(match != null)) throw new Exception("match is null");
            for (int i = 0; i < len; i++) {
                if (match(buf[i])) {
                    rtv.Add(buf[i]);
                }
            }
            return rtv;
        }

        /// <summary>
        /// 查找首个满足条件的元素并返回. 未返回将返回 T 的默认值
        /// </summary>
        public T FirstOrDefault(Predicate<T> match) {
            if (!(match != null)) throw new Exception("match is null");
            for (int i = 0; i < len; i++) {
                if (match(buf[i])) {
                    return buf[i];
                }
            }
            return default(T);
        }

        // 禁掉原本的 ToString 实现( 拿不到 oh )
        public override string ToString() {
            throw new Exception("can't use this func tostring. miss ObjectHelper");
        }

        /****************************************************************/
        // IObject 相关适配
        /****************************************************************/

        public override ushort GetTypeId() {
            return TypeId<List<T>>.value;
        }

        public override void Deserialize(DataReader dr) {
            ListIObjectImpl<T>.instance.From(dr, this);
        }

        public override void Serialize(DataWriter dw) {
            ListIObjectImpl<T>.instance.To(dw, this);
        }

        public override void ToString(ObjectHelper oh) {
            ListIObjectImpl<T>.instance.ToString(oh, this);
        }
    }

    #region ListIObjectImpl

    public abstract partial class ListIObjectImpl<T> {
        public abstract void To(DataWriter dw, List<T> vs);
        public abstract void From(DataReader dr, List<T> vs);
        public virtual void ToString(ObjectHelper oh, List<T> vs) {
            var sb = oh.sb;
            sb.Append('[');
            if (vs.len > 0) {
                for (int i = 0; i < vs.len; ++i) {
                    sb.Append(vs[i]);
                    sb.Append(',');
                }
                sb[sb.Length - 1] = ']';
            }
            else {
                sb.Append(']');
            }
        }

        public static ListIObjectImpl<T> instance;

        static ListIObjectImpl() {
            var t = typeof(T);
            if (t.IsGenericType && t.IsValueType)   // Nullable<T>
            {
                switch (Type.GetTypeCode(t.GetGenericArguments()[0])) {
                    //case TypeCode.Empty:
                    //case TypeCode.Object:
                    //case TypeCode.DBNull:
                    //case TypeCode.Char:
                    //case TypeCode.Decimal:
                    //case TypeCode.DateTime:
                    //case TypeCode.String:

                    case TypeCode.Boolean:
                        instance = new ListIObjectImpl_NullableBoolean() as ListIObjectImpl<T>;
                        break;
                    case TypeCode.SByte:
                        instance = new ListIObjectImpl_NullableSByte() as ListIObjectImpl<T>;
                        break;
                    case TypeCode.Byte:
                        instance = new ListIObjectImpl_NullableByte() as ListIObjectImpl<T>;
                        break;
                    case TypeCode.Int16:
                        instance = new ListIObjectImpl_NullableInt16() as ListIObjectImpl<T>;
                        break;
                    case TypeCode.UInt16:
                        instance = new ListIObjectImpl_NullableUInt16() as ListIObjectImpl<T>;
                        break;
                    case TypeCode.Int32:
                        instance = new ListIObjectImpl_NullableInt32() as ListIObjectImpl<T>;
                        break;
                    case TypeCode.UInt32:
                        instance = new ListIObjectImpl_NullableUInt32() as ListIObjectImpl<T>;
                        break;
                    case TypeCode.Int64:
                        instance = new ListIObjectImpl_NullableInt64() as ListIObjectImpl<T>;
                        break;
                    case TypeCode.UInt64:
                        instance = new ListIObjectImpl_NullableUInt64() as ListIObjectImpl<T>;
                        break;
                    case TypeCode.Single:
                        instance = new ListIObjectImpl_NullableSingle() as ListIObjectImpl<T>;
                        break;
                    case TypeCode.Double:
                        instance = new ListIObjectImpl_NullableDouble() as ListIObjectImpl<T>;
                        break;
                    default:
                        instance = new ListIObjectImpl_Weak<T>() as ListIObjectImpl<T>;
                        break;// throw new NotSupportedException();
                }
            }
            else if (t.IsEnum) {
                var et = Enum.GetUnderlyingType(t);
                if (et == typeof(byte)) instance = new ListIObjectImpl_Enum_Byte<T>() as ListIObjectImpl<T>;
                else if (et == typeof(sbyte)) instance = new ListIObjectImpl_Enum_SByte<T>() as ListIObjectImpl<T>;
                else if (et == typeof(ushort)) instance = new ListIObjectImpl_Enum_UInt16<T>() as ListIObjectImpl<T>;
                else if (et == typeof(short)) instance = new ListIObjectImpl_Enum_Int16<T>() as ListIObjectImpl<T>;
                else if (et == typeof(uint)) instance = new ListIObjectImpl_Enum_UInt32<T>() as ListIObjectImpl<T>;
                else if (et == typeof(int)) instance = new ListIObjectImpl_Enum_Int32<T>() as ListIObjectImpl<T>;
                else if (et == typeof(ulong)) instance = new ListIObjectImpl_Enum_UInt64<T>() as ListIObjectImpl<T>;
                else if (et == typeof(long)) instance = new ListIObjectImpl_Enum_Int64<T>() as ListIObjectImpl<T>;
            }
            else if (typeof(IObject).IsAssignableFrom(t)) {
                instance = new ListIObjectImpl_IObject<T>() as ListIObjectImpl<T>;
            }
            else {
                switch (Type.GetTypeCode(t)) {
                    //case TypeCode.Empty:
                    //case TypeCode.Object:
                    //case TypeCode.DBNull:
                    //case TypeCode.Char:
                    //case TypeCode.Decimal:

                    case TypeCode.Boolean:
                        instance = new ListIObjectImpl_Boolean() as ListIObjectImpl<T>;
                        break;
                    case TypeCode.SByte:
                        instance = new ListIObjectImpl_SByte() as ListIObjectImpl<T>;
                        break;
                    case TypeCode.Byte:
                        instance = new ListIObjectImpl_Byte() as ListIObjectImpl<T>;
                        break;
                    case TypeCode.Int16:
                        instance = new ListIObjectImpl_Int16() as ListIObjectImpl<T>;
                        break;
                    case TypeCode.UInt16:
                        instance = new ListIObjectImpl_UInt16() as ListIObjectImpl<T>;
                        break;
                    case TypeCode.Int32:
                        instance = new ListIObjectImpl_Int32() as ListIObjectImpl<T>;
                        break;
                    case TypeCode.UInt32:
                        instance = new ListIObjectImpl_UInt32() as ListIObjectImpl<T>;
                        break;
                    case TypeCode.Int64:
                        instance = new ListIObjectImpl_Int64() as ListIObjectImpl<T>;
                        break;
                    case TypeCode.UInt64:
                        instance = new ListIObjectImpl_UInt64() as ListIObjectImpl<T>;
                        break;
                    case TypeCode.Single:
                        instance = new ListIObjectImpl_Single() as ListIObjectImpl<T>;
                        break;
                    case TypeCode.Double:
                        instance = new ListIObjectImpl_Double() as ListIObjectImpl<T>;
                        break;
                    case TypeCode.String:
                        instance = new ListIObjectImpl_String() as ListIObjectImpl<T>;
                        break;
                    default:
                        break;// throw new NotSupportedException(); // 似乎可以在运行时手工初始化 instance 以实现 xx.List< enum type > 的支持
                }
            }
        }
    }

    #region ListIObjectImpl_Enum
    public partial class ListIObjectImpl_Enum_Byte<T> : ListIObjectImpl<T> {
        public override void To(DataWriter dw, List<T> vs) {
            var vsLen = vs.len;
            dw.WriteLength(vsLen);
            if (vsLen == 0) return;

            dw.Reserve(dw.len + vsLen);
            var bbBuf = dw.buf;
            var bbDataLen = dw.len;
            for (int i = 0; i < vsLen; i++) {
                bbBuf[bbDataLen + i] = (byte)(object)vs[i];
            }
            dw.len += vsLen;
        }
        public override void From(DataReader dr, List<T> vs) {
            int len = dr.ReadLength();
            vs.Resize(len);
            if (len == 0) return;

            var bbBuf = dr.buf;
            var vsBuf = vs.buf;
            var bbOffset = dr.offset;
            for (int i = 0; i < len; i++) {
                vsBuf[i] = (T)(object)bbBuf[bbOffset + i];
            }
            dr.offset += len;
        }
        public override void ToString(ObjectHelper oh, List<T> vs) {
            var sb = oh.sb;
            sb.Append('[');
            if (vs.len > 0) {
                for (int i = 0; i < vs.len; ++i) {
                    sb.Append((byte)(object)vs[i]);
                    sb.Append(',');
                }
                sb[sb.Length - 1] = ']';
            }
            else {
                sb.Append(']');
            }
        }
    }
    public partial class ListIObjectImpl_Enum_SByte<T> : ListIObjectImpl<T> {
        public override void To(DataWriter dw, List<T> vs) {
            var vsLen = vs.len;
            dw.WriteLength(vsLen);
            if (vsLen == 0) return;

            dw.Reserve(dw.len + vsLen);
            var bbBuf = dw.buf;
            var bbDataLen = dw.len;
            for (int i = 0; i < vsLen; i++) {
                bbBuf[bbDataLen + i] = (byte)(sbyte)(object)vs[i];
            }
            dw.len += vsLen;
        }
        public override void From(DataReader dr, List<T> vs) {
            int len = dr.ReadLength();
            vs.Resize(len);
            if (len == 0) return;

            var bbBuf = dr.buf;
            var vsBuf = vs.buf;
            var bbOffset = dr.offset;
            for (int i = 0; i < len; i++) {
                vsBuf[i] = (T)(object)(sbyte)bbBuf[bbOffset + i];
            }
            dr.offset += len;
        }
        public override void ToString(ObjectHelper oh, List<T> vs) {
            var sb = oh.sb;
            sb.Append('[');
            if (vs.len > 0) {
                for (int i = 0; i < vs.len; ++i) {
                    sb.Append((sbyte)(object)vs[i]);
                    sb.Append(',');
                }
                sb[sb.Length - 1] = ']';
            }
            else {
                sb.Append(']');
            }
        }
    }
    public partial class ListIObjectImpl_Enum_UInt16<T> : ListIObjectImpl<T> {
        public override void To(DataWriter dw, List<T> vs) {
            var vsLen = vs.len;
            dw.WriteLength(vsLen);
            if (vsLen == 0) return;

            dw.Reserve(dw.len + vsLen * 3);
            var bbBuf = dw.buf;
            var vsBuf = vs.buf;
            for (int i = 0; i < vsLen; i++) {
                DataWriter.Bit7Write(bbBuf, ref dw.data.len, (ushort)(object)vsBuf[i]);
            }
        }
        public override void From(DataReader dr, List<T> vs) {
            int len = dr.ReadLength();
            vs.Resize(len);
            if (len == 0) return;

            ushort tmp = 0;
            var vsBuf = vs.buf;
            for (int i = 0; i < len; i++) {
                dr.Read(ref tmp);
                vsBuf[i] = (T)(object)tmp;
            }
        }
        public override void ToString(ObjectHelper oh, List<T> vs) {
            var sb = oh.sb;
            sb.Append('[');
            if (vs.len > 0) {
                for (int i = 0; i < vs.len; ++i) {
                    sb.Append((ushort)(object)vs[i]);
                    sb.Append(',');
                }
                sb[sb.Length - 1] = ']';
            }
            else {
                sb.Append(']');
            }
        }
    }
    public partial class ListIObjectImpl_Enum_Int16<T> : ListIObjectImpl<T> {
        public override void To(DataWriter dw, List<T> vs) {
            var vsLen = vs.len;
            dw.WriteLength(vsLen);
            if (vsLen == 0) return;

            dw.Reserve(dw.len + vsLen * 3);
            var bbBuf = dw.buf;
            var vsBuf = vs.buf;
            for (int i = 0; i < vsLen; i++) {
                DataWriter.Bit7Write(bbBuf, ref dw.data.len, DataWriter.ZigZagEncode((short)(object)vsBuf[i]));
            }
        }
        public override void From(DataReader dr, List<T> vs) {
            int len = dr.ReadLength();
            vs.Resize(len);
            if (len == 0) return;

            short tmp = 0;
            var vsBuf = vs.buf;
            for (int i = 0; i < len; i++) {
                dr.Read(ref tmp);
                vsBuf[i] = (T)(object)tmp;
            }
        }
        public override void ToString(ObjectHelper oh, List<T> vs) {
            var sb = oh.sb;
            sb.Append('[');
            if (vs.len > 0) {
                for (int i = 0; i < vs.len; ++i) {
                    sb.Append((short)(object)vs[i]);
                    sb.Append(',');
                }
                sb[sb.Length - 1] = ']';
            }
            else {
                sb.Append(']');
            }
        }
    }
    public partial class ListIObjectImpl_Enum_UInt32<T> : ListIObjectImpl<T> {
        public override void To(DataWriter dw, List<T> vs) {
            var vsLen = vs.len;
            dw.WriteLength(vsLen);
            if (vsLen == 0) return;

            dw.Reserve(dw.len + vsLen * 5);
            var bbBuf = dw.buf;
            var vsBuf = vs.buf;
            for (int i = 0; i < vsLen; i++) {
                DataWriter.Bit7Write(bbBuf, ref dw.data.len, (uint)(object)vsBuf[i]);
            }
        }
        public override void From(DataReader dr, List<T> vs) {
            int len = dr.ReadLength();
            vs.Resize(len);
            if (len == 0) return;

            uint tmp = 0;
            var vsBuf = vs.buf;
            for (int i = 0; i < len; i++) {
                dr.Read(ref tmp);
                vsBuf[i] = (T)(object)tmp;
            }
        }
        public override void ToString(ObjectHelper oh, List<T> vs) {
            var sb = oh.sb;
            sb.Append('[');
            if (vs.len > 0) {
                for (int i = 0; i < vs.len; ++i) {
                    sb.Append((uint)(object)vs[i]);
                    sb.Append(',');
                }
                sb[sb.Length - 1] = ']';
            }
            else {
                sb.Append(']');
            }
        }
    }
    public partial class ListIObjectImpl_Enum_Int32<T> : ListIObjectImpl<T> {
        public override void To(DataWriter dw, List<T> vs) {
            var vsLen = vs.len;
            dw.WriteLength(vsLen);
            if (vsLen == 0) return;

            dw.Reserve(dw.len + vsLen * 5);
            var bbBuf = dw.buf;
            var vsBuf = vs.buf;
            for (int i = 0; i < vsLen; i++) {
                DataWriter.Bit7Write(bbBuf, ref dw.data.len, DataWriter.ZigZagEncode((int)(object)vs[i]));
            }
        }
        public override void From(DataReader dr, List<T> vs) {
            int len = dr.ReadLength();
            vs.Resize(len);
            if (len == 0) return;

            int tmp = 0;
            var vsBuf = vs.buf;
            for (int i = 0; i < len; i++) {
                dr.Read(ref tmp);
                vsBuf[i] = (T)(object)tmp;
            }
        }
        public override void ToString(ObjectHelper oh, List<T> vs) {
            var sb = oh.sb;
            sb.Append('[');
            if (vs.len > 0) {
                for (int i = 0; i < vs.len; ++i) {
                    sb.Append((int)(object)vs[i]);
                    sb.Append(',');
                }
                sb[sb.Length - 1] = ']';
            }
            else {
                sb.Append(']');
            }
        }
    }
    public partial class ListIObjectImpl_Enum_UInt64<T> : ListIObjectImpl<T> {
        public override void To(DataWriter dw, List<T> vs) {
            var vsLen = vs.len;
            dw.WriteLength(vsLen);
            if (vsLen == 0) return;

            dw.Reserve(dw.len + vsLen * 9);
            var bbBuf = dw.buf;
            var vsBuf = vs.buf;
            for (int i = 0; i < vsLen; i++) {
                DataWriter.Bit7Write(bbBuf, ref dw.data.len, (ulong)(object)vsBuf[i]);
            }
        }
        public override void From(DataReader dr, List<T> vs) {
            int len = dr.ReadLength();
            vs.Resize(len);
            if (len == 0) return;

            ulong tmp = 0;
            var vsBuf = vs.buf;
            for (int i = 0; i < len; i++) {
                dr.Read(ref tmp);
                vsBuf[i] = (T)(object)tmp;
            }
        }
        public override void ToString(ObjectHelper oh, List<T> vs) {
            var sb = oh.sb;
            sb.Append('[');
            if (vs.len > 0) {
                for (int i = 0; i < vs.len; ++i) {
                    sb.Append((ulong)(object)vs[i]);
                    sb.Append(',');
                }
                sb[sb.Length - 1] = ']';
            }
            else {
                sb.Append(']');
            }
        }
    }
    public partial class ListIObjectImpl_Enum_Int64<T> : ListIObjectImpl<T> {
        public override void To(DataWriter dw, List<T> vs) {
            var vsLen = vs.len;
            dw.WriteLength(vsLen);
            if (vsLen == 0) return;

            dw.Reserve(dw.len + vsLen * 9);
            var bbBuf = dw.buf;
            var vsBuf = vs.buf;
            for (int i = 0; i < vsLen; i++) {
                DataWriter.Bit7Write(bbBuf, ref dw.data.len, DataWriter.ZigZagEncode((long)(object)vsBuf[i]));
            }
        }
        public override void From(DataReader dr, List<T> vs) {
            int len = dr.ReadLength();
            vs.Resize(len);
            if (len == 0) return;

            long tmp = 0;
            var vsBuf = vs.buf;
            for (int i = 0; i < len; i++) {
                dr.Read(ref tmp);
                vsBuf[i] = (T)(object)tmp;
            }
        }
        public override void ToString(ObjectHelper oh, List<T> vs) {
            var sb = oh.sb;
            sb.Append('[');
            if (vs.len > 0) {
                for (int i = 0; i < vs.len; ++i) {
                    sb.Append((long)(object)vs[i]);
                    sb.Append(',');
                }
                sb[sb.Length - 1] = ']';
            }
            else {
                sb.Append(']');
            }
        }
    }
    #endregion

    public partial class ListIObjectImpl_IObject<T> : ListIObjectImpl<T> {
        public override void To(DataWriter dw, List<T> vs) {
            var vsLen = vs.len;
            dw.WriteLength(vsLen);
            if (vsLen == 0) return;

            if (typeof(T).IsValueType) {
                for (int i = 0; i < vsLen; i++) {
                    ((IObject)vs[i]).Serialize(dw);
                }
            }
            else {
                for (int i = 0; i < vsLen; i++) {
                    dw.Write((IObject)vs[i]);
                }
            }
        }
        public override void From(DataReader dr, List<T> vs) {
            int len = dr.ReadLength();
            vs.Clear();
            vs.Resize(len);
            if (len == 0) return;

            if (typeof(T).IsValueType) {
                for (int i = 0; i < len; i++) {
                    var tmp = (IObject)default(T);
                    tmp.Deserialize(dr);
                    vs[i] = (T)tmp;
                }
            }
            else {
                for (int i = 0; i < len; i++) {
                    IObject tmp = null;
                    dr.Read(ref tmp);
                    vs[i] = (T)tmp;
                }
            }
        }
        public override void ToString(ObjectHelper oh, List<T> vs) {
            var sb = oh.sb;
            sb.Append('[');
            if (vs.len > 0) {
                for (int i = 0; i < vs.len; ++i) {
                    if (vs[i] == null) {
                        sb.Append("null");
                    }
                    else {
                        ((IObject)vs[i]).ToString(oh);
                    }
                    sb.Append(',');
                }
                sb[sb.Length - 1] = ']';
            }
            else {
                sb.Append(']');
            }
        }
    }
    public partial class ListIObjectImpl_Weak<T> : ListIObjectImpl<T> {
        public override void To(DataWriter dw, List<T> vs) {
            var vsLen = vs.len;
            dw.WriteLength(vsLen);
            if (vsLen == 0) return;

            for (int i = 0; i < vsLen; i++) {
                dw.Write(((IWeak)vs[i]).Lock());
            }
        }
        public override void From(DataReader dr, List<T> vs) {
            int len = dr.ReadLength();
            vs.Clear();
            vs.Resize(len);
            if (len == 0) return;

            for (int i = 0; i < len; i++) {
                Object tmp = null;
                dr.Read(ref tmp);
                var v = (IWeak)vs[i];
                v.Reset(tmp);
                vs[i] = (T)v;
            }
        }
        public override void ToString(ObjectHelper oh, List<T> vs) {
            var sb = oh.sb;
            sb.Append('[');
            if (vs.len > 0) {
                for (int i = 0; i < vs.len; ++i) {
                    var o = ((IWeak)vs[i]).Lock();
                    if (o == null) {
                        sb.Append("null");
                    }
                    else {
                        o.ToString(oh);
                    }
                    sb.Append(',');
                }
                sb[sb.Length - 1] = ']';
            }
            else {
                sb.Append(']');
            }
        }
    }

    #region ListIObjectImpl_ value types
    public partial class ListIObjectImpl_Boolean : ListIObjectImpl<bool> {
        public override void To(DataWriter dw, List<bool> vs) {
            var vsLen = vs.len;
            dw.WriteLength(vsLen);
            if (vsLen == 0) return;

            dw.Reserve(dw.len + vsLen);
            var vsBuf = vs.buf;
            for (int i = 0; i < vsLen; i++) {
                dw.Write(vsBuf[i]);     // 先用着这个不优化的方案
            }
        }
        public override void From(DataReader dr, List<bool> vs) {
            int len = dr.ReadLength();
            vs.Resize(len);
            if (len == 0) return;

            bool tmp = false;
            var vsBuf = vs.buf;
            for (int i = 0; i < len; i++) {
                dr.Read(ref tmp);
                vsBuf[i] = tmp;
            }
        }
    }
    public partial class ListIObjectImpl_SByte : ListIObjectImpl<sbyte> {
        public override void To(DataWriter dw, List<sbyte> vs) {
            var vsLen = vs.len;
            dw.WriteLength(vsLen);
            if (vsLen == 0) return;

            dw.Reserve(dw.len + vsLen * 1);
            Buffer.BlockCopy(vs.buf, 0, dw.buf, dw.len, vsLen);
            dw.len += vsLen;
        }
        public override void From(DataReader dr, List<sbyte> vs) {
            int len = dr.ReadLength();
            vs.Resize(len);
            if (len == 0) return;

            Buffer.BlockCopy(dr.buf, dr.offset, vs.buf, 0, len);
            dr.offset += len;
        }
    }
    public partial class ListIObjectImpl_Byte : ListIObjectImpl<byte> {
        public override void To(DataWriter dw, List<byte> vs) {
            var vsLen = vs.len;
            dw.WriteLength(vsLen);
            if (vsLen == 0) return;

            dw.Reserve(dw.len + vsLen * 1);
            Buffer.BlockCopy(vs.buf, 0, dw.buf, dw.len, vsLen);
            dw.len += vsLen;
        }
        public override void From(DataReader dr, List<byte> vs) {
            int len = dr.ReadLength();
            vs.Resize(len);
            if (len == 0) return;

            Buffer.BlockCopy(dr.buf, dr.offset, vs.buf, 0, len);
            dr.offset += len;
        }
    }
    public partial class ListIObjectImpl_Int16 : ListIObjectImpl<short> {
        public override void To(DataWriter dw, List<short> vs) {
            var vsLen = vs.len;
            dw.WriteLength(vsLen);
            if (vsLen == 0) return;

            dw.Reserve(dw.len + vsLen * 3);
            var bbBuf = dw.buf;
            var vsBuf = vs.buf;
            for (int i = 0; i < vsLen; i++) {
                DataWriter.Bit7Write(bbBuf, ref dw.data.len, DataWriter.ZigZagEncode(vsBuf[i]));
            }
        }
        public override void From(DataReader dr, List<short> vs) {
            int len = dr.ReadLength();
            vs.Resize(len);
            if (len == 0) return;

            short tmp = 0;
            var vsBuf = vs.buf;
            for (int i = 0; i < len; i++) {
                dr.Read(ref tmp);
                vsBuf[i] = tmp;
            }
        }
    }
    public partial class ListIObjectImpl_UInt16 : ListIObjectImpl<ushort> {
        public override void To(DataWriter dw, List<ushort> vs) {
            var vsLen = vs.len;
            dw.WriteLength(vsLen);
            if (vsLen == 0) return;

            dw.Reserve(dw.len + vsLen * 3);
            var bbBuf = dw.buf;
            var vsBuf = vs.buf;
            for (int i = 0; i < vsLen; i++) {
                DataWriter.Bit7Write(bbBuf, ref dw.data.len, vsBuf[i]);
            }
        }
        public override void From(DataReader dr, List<ushort> vs) {
            int len = dr.ReadLength();
            vs.Resize(len);
            if (len == 0) return;

            ushort tmp = 0;
            var vsBuf = vs.buf;
            for (int i = 0; i < len; i++) {
                dr.Read(ref tmp);
                vsBuf[i] = tmp;
            }
        }
    }
    public partial class ListIObjectImpl_Int32 : ListIObjectImpl<int> {
        public override void To(DataWriter dw, List<int> vs) {
            var vsLen = vs.len;
            dw.WriteLength(vsLen);
            if (vsLen == 0) return;

            dw.Reserve(dw.len + vsLen * 5);
            var bbBuf = dw.buf;
            var vsBuf = vs.buf;
            for (int i = 0; i < vsLen; i++) {
                DataWriter.Bit7Write(bbBuf, ref dw.data.len, DataWriter.ZigZagEncode(vsBuf[i]));
            }
        }
        public override void From(DataReader dr, List<int> vs) {
            int len = dr.ReadLength();
            vs.Resize(len);
            if (len == 0) return;

            int tmp = 0;
            var vsBuf = vs.buf;
            for (int i = 0; i < len; i++) {
                dr.Read(ref tmp);
                vsBuf[i] = tmp;
            }
        }
    }
    public partial class ListIObjectImpl_UInt32 : ListIObjectImpl<uint> {
        public override void To(DataWriter dw, List<uint> vs) {
            var vsLen = vs.len;
            dw.WriteLength(vsLen);
            if (vsLen == 0) return;

            dw.Reserve(dw.len + vsLen * 5);
            var bbBuf = dw.buf;
            var vsBuf = vs.buf;
            for (int i = 0; i < vsLen; i++) {
                DataWriter.Bit7Write(bbBuf, ref dw.data.len, vsBuf[i]);
            }
        }
        public override void From(DataReader dr, List<uint> vs) {
            int len = dr.ReadLength();
            vs.Resize(len);
            if (len == 0) return;

            uint tmp = 0;
            var vsBuf = vs.buf;
            for (int i = 0; i < len; i++) {
                dr.Read(ref tmp);
                vsBuf[i] = tmp;
            }
        }
    }
    public partial class ListIObjectImpl_Int64 : ListIObjectImpl<long> {
        public override void To(DataWriter dw, List<long> vs) {
            var vsLen = vs.len;
            dw.WriteLength(vsLen);
            if (vsLen == 0) return;

            dw.Reserve(dw.len + vsLen * 9);
            var bbBuf = dw.buf;
            var vsBuf = vs.buf;
            for (int i = 0; i < vsLen; i++) {
                DataWriter.Bit7Write(bbBuf, ref dw.data.len, DataWriter.ZigZagEncode(vsBuf[i]));
            }
        }
        public override void From(DataReader dr, List<long> vs) {
            int len = dr.ReadLength();
            vs.Resize(len);
            if (len == 0) return;

            long tmp = 0;
            var vsBuf = vs.buf;
            for (int i = 0; i < len; i++) {
                dr.Read(ref tmp);
                vsBuf[i] = tmp;
            }
        }
    }
    public partial class ListIObjectImpl_UInt64 : ListIObjectImpl<ulong> {
        public override void To(DataWriter dw, List<ulong> vs) {
            var vsLen = vs.len;
            dw.WriteLength(vsLen);
            if (vsLen == 0) return;

            dw.Reserve(dw.len + vsLen * 9);
            var bbBuf = dw.buf;
            var vsBuf = vs.buf;
            for (int i = 0; i < vsLen; i++) {
                DataWriter.Bit7Write(bbBuf, ref dw.data.len, vsBuf[i]);
            }
        }
        public override void From(DataReader dr, List<ulong> vs) {
            int len = dr.ReadLength();
            vs.Resize(len);
            if (len == 0) return;

            ulong tmp = 0;
            var vsBuf = vs.buf;
            for (int i = 0; i < len; i++) {
                dr.Read(ref tmp);
                vsBuf[i] = tmp;
            }
        }
    }
    public partial class ListIObjectImpl_Single : ListIObjectImpl<float> {
        public override void To(DataWriter dw, List<float> vs) {
            var vsLen = vs.len;
            dw.WriteLength(vsLen);
            if (vsLen == 0) return;

            dw.Reserve(dw.len + vsLen * 4);
            Buffer.BlockCopy(vs.buf, 0, dw.buf, dw.len, vsLen * 4);
            dw.len += vsLen * 4;
        }
        public override void From(DataReader dr, List<float> vs) {
            int len = dr.ReadLength();
            vs.Resize(len);
            if (len == 0) return;

            Buffer.BlockCopy(dr.buf, dr.offset, vs.buf, 0, len * 4);
            dr.offset += len * 4;
        }
    }
    public partial class ListIObjectImpl_Double : ListIObjectImpl<double> {
        public override void To(DataWriter dw, List<double> vs) {
            var vsLen = vs.len;
            dw.WriteLength(vsLen);
            if (vsLen == 0) return;

            dw.Reserve(dw.len + vsLen * 9);
            for (int i = 0; i < vsLen; i++) {
                dw.WriteDirect(vs[i]);
            }
        }
        public override void From(DataReader dr, List<double> vs) {
            int len = dr.ReadLength();
            vs.Resize(len);
            if (len == 0) return;

            double tmp = 0;
            var vsBuf = vs.buf;
            for (int i = 0; i < len; i++) {
                dr.Read(ref tmp);
                vsBuf[i] = tmp;
            }
        }
    }
    public partial class ListIObjectImpl_String : ListIObjectImpl<string> {
        public override void To(DataWriter dw, List<string> vs) {
            var vsLen = vs.len;
            dw.WriteLength(vsLen);
            if (vsLen == 0) return;

            for (int i = 0; i < vsLen; i++) {
                dw.Write(vs[i]);
            }
        }
        public override void From(DataReader dr, List<string> vs) {
            int len = dr.ReadLength();
            vs.Resize(len);
            if (len == 0) return;

            string tmp = null;
            var vsBuf = vs.buf;
            for (int i = 0; i < len; i++) {
                dr.Read(ref tmp);
                vsBuf[i] = tmp;
            }
        }
        public override void ToString(ObjectHelper oh, List<string> vs) {
            var sb = oh.sb;
            sb.Append('[');
            if (vs.len > 0) {
                for (int i = 0; i < vs.len; ++i) {
                    sb.Append('\"');
                    sb.Append(vs[i]);    // todo: 转义?
                    sb.Append('\"');
                    sb.Append(',');
                }
                sb[sb.Length - 1] = ']';
            }
            else {
                sb.Append(']');
            }
        }
    }
    // todo: _Data
    #endregion

    public partial class ListIObjectImpl_NullableBoolean : ListIObjectImpl<bool?> {
        public override void To(DataWriter dw, List<bool?> vs) {
            var vsLen = vs.len;
            dw.WriteLength(vsLen);
            if (vsLen == 0) return;

            dw.Reserve(dw.len + vsLen * 2);
            var vsBuf = vs.buf;
            for (int i = 0; i < vsLen; i++) {
                dw.Write(vsBuf[i]);
            }
        }
        public override void From(DataReader dr, List<bool?> vs) {
            int len = dr.ReadLength();
            vs.Resize(len);
            if (len == 0) return;

            bool? tmp = null;
            var vsBuf = vs.buf;
            for (int i = 0; i < len; i++) {
                dr.Read(ref tmp);
                vsBuf[i] = tmp;
            }
        }
        public override void ToString(ObjectHelper oh, List<bool?> vs) {
            var sb = oh.sb;
            sb.Append('[');
            if (vs.len > 0) {
                for (int i = 0; i < vs.len; ++i) {
                    sb.Append(vs[i].HasValue ? vs[i].ToString() : "null");
                    sb.Append(',');
                }
                sb[sb.Length - 1] = ']';
            }
            else {
                sb.Append(']');
            }
        }
    }
    public partial class ListIObjectImpl_NullableSByte : ListIObjectImpl<sbyte?> {
        public override void To(DataWriter dw, List<sbyte?> vs) {
            var vsLen = vs.len;
            dw.WriteLength(vsLen);
            if (vsLen == 0) return;

            dw.Reserve(dw.len + vsLen * 2);
            var vsBuf = vs.buf;
            for (int i = 0; i < vsLen; i++) {
                dw.Write(vsBuf[i]);
            }
        }
        public override void From(DataReader dr, List<sbyte?> vs) {
            int len = dr.ReadLength();
            vs.Resize(len);
            if (len == 0) return;

            sbyte? tmp = null;
            var vsBuf = vs.buf;
            for (int i = 0; i < len; i++) {
                dr.Read(ref tmp);
                vsBuf[i] = tmp;
            }
        }
        public override void ToString(ObjectHelper oh, List<sbyte?> vs) {
            var sb = oh.sb;
            sb.Append('[');
            if (vs.len > 0) {
                for (int i = 0; i < vs.len; ++i) {
                    sb.Append(vs[i].HasValue ? vs[i].ToString() : "null");
                    sb.Append(',');
                }
                sb[sb.Length - 1] = ']';
            }
            else {
                sb.Append(']');
            }
        }
    }
    public partial class ListIObjectImpl_NullableByte : ListIObjectImpl<byte?> {
        public override void To(DataWriter dw, List<byte?> vs) {
            var vsLen = vs.len;
            dw.WriteLength(vsLen);
            if (vsLen == 0) return;

            dw.Reserve(dw.len + vsLen * 2);
            var vsBuf = vs.buf;
            for (int i = 0; i < vsLen; i++) {
                dw.Write(vsBuf[i]);
            }
        }
        public override void From(DataReader dr, List<byte?> vs) {
            int len = dr.ReadLength();
            vs.Resize(len);
            if (len == 0) return;

            byte? tmp = null;
            var vsBuf = vs.buf;
            for (int i = 0; i < len; i++) {
                dr.Read(ref tmp);
                vsBuf[i] = tmp;
            }
        }
        public override void ToString(ObjectHelper oh, List<byte?> vs) {
            var sb = oh.sb;
            sb.Append('[');
            if (vs.len > 0) {
                for (int i = 0; i < vs.len; ++i) {
                    sb.Append(vs[i].HasValue ? vs[i].ToString() : "null");
                    sb.Append(',');
                }
                sb[sb.Length - 1] = ']';
            }
            else {
                sb.Append(']');
            }
        }
    }
    public partial class ListIObjectImpl_NullableInt16 : ListIObjectImpl<short?> {
        public override void To(DataWriter dw, List<short?> vs) {
            var vsLen = vs.len;
            dw.WriteLength(vsLen);
            if (vsLen == 0) return;

            dw.Reserve(dw.len + vsLen * 4);
            var vsBuf = vs.buf;
            for (int i = 0; i < vsLen; i++) {
                dw.Write(vsBuf[i]);
            }
        }
        public override void From(DataReader dr, List<short?> vs) {
            int len = dr.ReadLength();
            vs.Resize(len);
            if (len == 0) return;

            short? tmp = null;
            var vsBuf = vs.buf;
            for (int i = 0; i < len; i++) {
                dr.Read(ref tmp);
                vsBuf[i] = tmp;
            }
        }
        public override void ToString(ObjectHelper oh, List<short?> vs) {
            var sb = oh.sb;
            sb.Append('[');
            if (vs.len > 0) {
                for (int i = 0; i < vs.len; ++i) {
                    sb.Append(vs[i].HasValue ? vs[i].ToString() : "null");
                    sb.Append(',');
                }
                sb[sb.Length - 1] = ']';
            }
            else {
                sb.Append(']');
            }
        }
    }
    public partial class ListIObjectImpl_NullableUInt16 : ListIObjectImpl<ushort?> {
        public override void To(DataWriter dw, List<ushort?> vs) {
            var vsLen = vs.len;
            dw.WriteLength(vsLen);
            if (vsLen == 0) return;

            dw.Reserve(dw.len + vsLen * 4);
            var vsBuf = vs.buf;
            for (int i = 0; i < vsLen; i++) {
                dw.Write(vsBuf[i]);
            }
        }
        public override void From(DataReader dr, List<ushort?> vs) {
            int len = dr.ReadLength();
            vs.Resize(len);
            if (len == 0) return;

            ushort? tmp = null;
            var vsBuf = vs.buf;
            for (int i = 0; i < len; i++) {
                dr.Read(ref tmp);
                vsBuf[i] = tmp;
            }
        }
        public override void ToString(ObjectHelper oh, List<ushort?> vs) {
            var sb = oh.sb;
            sb.Append('[');
            if (vs.len > 0) {
                for (int i = 0; i < vs.len; ++i) {
                    sb.Append(vs[i].HasValue ? vs[i].ToString() : "null");
                    sb.Append(',');
                }
                sb[sb.Length - 1] = ']';
            }
            else {
                sb.Append(']');
            }
        }
    }
    public partial class ListIObjectImpl_NullableInt32 : ListIObjectImpl<int?> {
        public override void To(DataWriter dw, List<int?> vs) {
            var vsLen = vs.len;
            dw.WriteLength(vsLen);
            if (vsLen == 0) return;

            dw.Reserve(dw.len + vsLen * 6);
            var vsBuf = vs.buf;
            for (int i = 0; i < vsLen; i++) {
                dw.Write(vsBuf[i]);
            }
        }
        public override void From(DataReader dr, List<int?> vs) {
            int len = dr.ReadLength();
            vs.Resize(len);
            if (len == 0) return;

            int? tmp = null;
            var vsBuf = vs.buf;
            for (int i = 0; i < len; i++) {
                dr.Read(ref tmp);
                vsBuf[i] = tmp;
            }
        }
        public override void ToString(ObjectHelper oh, List<int?> vs) {
            var sb = oh.sb;
            sb.Append('[');
            if (vs.len > 0) {
                for (int i = 0; i < vs.len; ++i) {
                    sb.Append(vs[i].HasValue ? vs[i].ToString() : "null");
                    sb.Append(',');
                }
                sb[sb.Length - 1] = ']';
            }
            else {
                sb.Append(']');
            }
        }
    }
    public partial class ListIObjectImpl_NullableUInt32 : ListIObjectImpl<uint?> {
        public override void To(DataWriter dw, List<uint?> vs) {
            var vsLen = vs.len;
            dw.WriteLength(vsLen);
            if (vsLen == 0) return;

            dw.Reserve(dw.len + vsLen * 6);
            var vsBuf = vs.buf;
            for (int i = 0; i < vsLen; i++) {
                dw.Write(vsBuf[i]);
            }
        }
        public override void From(DataReader dr, List<uint?> vs) {
            int len = dr.ReadLength();
            vs.Resize(len);
            if (len == 0) return;

            uint? tmp = null;
            var vsBuf = vs.buf;
            for (int i = 0; i < len; i++) {
                dr.Read(ref tmp);
                vsBuf[i] = tmp;
            }
        }
        public override void ToString(ObjectHelper oh, List<uint?> vs) {
            var sb = oh.sb;
            sb.Append('[');
            if (vs.len > 0) {
                for (int i = 0; i < vs.len; ++i) {
                    sb.Append(vs[i].HasValue ? vs[i].ToString() : "null");
                    sb.Append(',');
                }
                sb[sb.Length - 1] = ']';
            }
            else {
                sb.Append(']');
            }
        }
    }
    public partial class ListIObjectImpl_NullableInt64 : ListIObjectImpl<long?> {
        public override void To(DataWriter dw, List<long?> vs) {
            var vsLen = vs.len;
            dw.WriteLength(vsLen);
            if (vsLen == 0) return;

            dw.Reserve(dw.len + vsLen * 10);
            var vsBuf = vs.buf;
            for (int i = 0; i < vsLen; i++) {
                dw.Write(vsBuf[i]);
            }
        }
        public override void From(DataReader dr, List<long?> vs) {
            int len = dr.ReadLength();
            vs.Resize(len);
            if (len == 0) return;

            long? tmp = null;
            var vsBuf = vs.buf;
            for (int i = 0; i < len; i++) {
                dr.Read(ref tmp);
                vsBuf[i] = tmp;
            }
        }
        public override void ToString(ObjectHelper oh, List<long?> vs) {
            var sb = oh.sb;
            sb.Append('[');
            if (vs.len > 0) {
                for (int i = 0; i < vs.len; ++i) {
                    sb.Append(vs[i].HasValue ? vs[i].ToString() : "null");
                    sb.Append(',');
                }
                sb[sb.Length - 1] = ']';
            }
            else {
                sb.Append(']');
            }
        }
    }
    public partial class ListIObjectImpl_NullableUInt64 : ListIObjectImpl<ulong?> {
        public override void To(DataWriter dw, List<ulong?> vs) {
            var vsLen = vs.len;
            dw.WriteLength(vsLen);
            if (vsLen == 0) return;

            dw.Reserve(dw.len + vsLen * 10);
            var vsBuf = vs.buf;
            for (int i = 0; i < vsLen; i++) {
                dw.Write(vsBuf[i]);
            }
        }
        public override void From(DataReader dr, List<ulong?> vs) {
            int len = dr.ReadLength();
            vs.Resize(len);
            if (len == 0) return;

            ulong? tmp = null;
            var vsBuf = vs.buf;
            for (int i = 0; i < len; i++) {
                dr.Read(ref tmp);
                vsBuf[i] = tmp;
            }
        }
        public override void ToString(ObjectHelper oh, List<ulong?> vs) {
            var sb = oh.sb;
            sb.Append('[');
            if (vs.len > 0) {
                for (int i = 0; i < vs.len; ++i) {
                    sb.Append(vs[i].HasValue ? vs[i].ToString() : "null");
                    sb.Append(',');
                }
                sb[sb.Length - 1] = ']';
            }
            else {
                sb.Append(']');
            }
        }
    }
    public partial class ListIObjectImpl_NullableSingle : ListIObjectImpl<float?> {
        public override void To(DataWriter dw, List<float?> vs) {
            var vsLen = vs.len;
            dw.WriteLength(vsLen);
            if (vsLen == 0) return;

            dw.Reserve(dw.len + vsLen * 10);
            var vsBuf = vs.buf;
            for (int i = 0; i < vsLen; i++) {
                dw.Write(vsBuf[i]);
            }
        }
        public override void From(DataReader dr, List<float?> vs) {
            int len = dr.ReadLength();
            vs.Resize(len);
            if (len == 0) return;

            float? tmp = null;
            var vsBuf = vs.buf;
            for (int i = 0; i < len; i++) {
                dr.Read(ref tmp);
                vsBuf[i] = tmp;
            }
        }
        public override void ToString(ObjectHelper oh, List<float?> vs) {
            var sb = oh.sb;
            sb.Append('[');
            if (vs.len > 0) {
                for (int i = 0; i < vs.len; ++i) {
                    sb.Append(vs[i].HasValue ? vs[i].ToString() : "null");
                    sb.Append(',');
                }
                sb[sb.Length - 1] = ']';
            }
            else {
                sb.Append(']');
            }
        }
    }
    public partial class ListIObjectImpl_NullableDouble : ListIObjectImpl<double?> {
        public override void To(DataWriter dw, List<double?> vs) {
            var vsLen = vs.len;
            dw.WriteLength(vsLen);
            if (vsLen == 0) return;

            dw.Reserve(dw.len + vsLen * 10);
            var vsBuf = vs.buf;
            for (int i = 0; i < vsLen; i++) {
                dw.Write(vsBuf[i]);
            }
        }
        public override void From(DataReader dr, List<double?> vs) {
            int len = dr.ReadLength();
            vs.Resize(len);
            if (len == 0) return;

            double? tmp = null;
            var vsBuf = vs.buf;
            for (int i = 0; i < len; i++) {
                dr.Read(ref tmp);
                vsBuf[i] = tmp;
            }
        }
        public override void ToString(ObjectHelper oh, List<double?> vs) {
            var sb = oh.sb;
            sb.Append('[');
            if (vs.len > 0) {
                for (int i = 0; i < vs.len; ++i) {
                    sb.Append(vs[i].HasValue ? vs[i].ToString() : "null");
                    sb.Append(',');
                }
                sb[sb.Length - 1] = ']';
            }
            else {
                sb.Append(']');
            }
        }
    }

    #endregion
}
