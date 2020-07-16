using System;
using System.Runtime.InteropServices;
using System.Text;

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

        public virtual void Serialize(DataWriter bb) { }
        public virtual void Deserialize(DataReader bb) { }

        public bool __toStringing;
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

    /// <summary>
    /// 类似 C++ 的 std::pair. 支持值比较, 做 key
    /// </summary>
    public struct Pair<First, Second> {
        public First first;
        public Second second;

        static System.Collections.Generic.IEqualityComparer<First> comparerFirst = System.Collections.Generic.EqualityComparer<First>.Default;
        static System.Collections.Generic.IEqualityComparer<Second> comparerSecond = System.Collections.Generic.EqualityComparer<Second>.Default;
        public static bool operator ==(Pair<First, Second> a, Pair<First, Second> b) {
            return comparerFirst.Equals(a.first, b.first) && comparerSecond.Equals(a.second, b.second);
        }
        public static bool operator !=(Pair<First, Second> a, Pair<First, Second> b) {
            return !comparerFirst.Equals(a.first, b.first) || !comparerSecond.Equals(a.second, b.second);
        }
        public override bool Equals(object obj) {
            return this == (Pair<First, Second>)obj;
        }
        public override int GetHashCode() {
            return first.GetHashCode() ^ second.GetHashCode();
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

    public static class Utils {
        /// <summary>
        /// 得到当前时间点的 epoch (精度为秒后 7 个 0)
        /// </summary>
        public static long NowEpoch10m() {
            return DateTime.UtcNow.Ticks - 621355968000000000;
        }

        /// <summary>
        /// 时间( Local或Utc )转 epoch (精度为秒后 7 个 0)
        /// </summary>
        public static long DateTimeToEpoch10m(this DateTime dt) {
            return dt.ToUniversalTime().Ticks - 621355968000000000;
        }

        /// <summary>
        /// epoch (精度为秒后 7 个 0) 转为 Utc 时间
        /// </summary>
        public static DateTime Epoch10mToUtcDateTime(long epoch10m) {
            return new DateTime(epoch10m + 621355968000000000, DateTimeKind.Utc);
        }

        /// <summary>
        /// epoch (精度为秒后 7 个 0) 转为 本地 时间
        /// </summary>
        public static DateTime Epoch10mToLocalDateTime(long epoch10m) {
            var dt = new DateTime(epoch10m + 621355968000000000, DateTimeKind.Utc);
            return dt.ToLocalTime();
        }


        // 下面是精度为 秒 的版本.

        public static int NowEpoch() {
            return (int)(DateTime.Now.ToUniversalTime().Ticks - 621355968000000000 / 10000000);
        }

        public static int DateTimeToEpoch(this DateTime dt) {
            return (int)((dt.ToUniversalTime().Ticks - 621355968000000000) / 10000000);
        }

        public static DateTime EpochToUtcDateTime(int epoch) {
            return new DateTime((long)epoch * 10000000 + 621355968000000000, DateTimeKind.Utc);
        }
        public static DateTime EpochToLocalDateTime(int epoch) {
            var dt = new DateTime((long)epoch * 10000000 + 621355968000000000, DateTimeKind.Utc);
            return dt.ToLocalTime();
        }



        /// <summary>
        /// 以北京时间返回"今日" 0时0分0秒的 epoch10m
        /// </summary>
        public static long BeiJingTodayEpoch10m() {
            var t = DateTime.UtcNow.AddHours(+8);
            t = new DateTime(t.Year, t.Month, t.Day, 0, 0, 0, DateTimeKind.Utc);
            return xx.Utils.DateTimeToEpoch10m(t.AddHours(-8));
        }

        /// <summary>
        /// 以北京时间"今日" 0时0分0秒的 epoch10m 为基础, 按天偏移
        /// </summary>
        public static long BeiJingTodayEpoch10m(long days) {
            return BeiJingTodayEpoch10m() + days * 864000000000L;
        }

        /// <summary>
        /// 按天偏移传入的 epoch10m 值
        /// </summary>
        public static long Epoch10mAddDays(long value, long days) {
            return value + days * 864000000000L;
        }



        public static string Epoch10mDurationToString(long epoch10mDuration) {
            const double oneMicrosecond = 10;
            const double oneMillisecond = oneMicrosecond * 1000;
            const double oneSecond = oneMillisecond * 1000;
            const double oneMinute = oneSecond * 60;
            const double oneHour = oneMinute * 60;
            const double oneDay = oneHour * 60;
            if (epoch10mDuration > oneDay) {
                return (epoch10mDuration / oneDay) + "D";
            }
            else if (epoch10mDuration > oneHour) {
                return (epoch10mDuration / oneHour) + "h";
            }
            else if (epoch10mDuration > oneMinute) {
                return (epoch10mDuration / oneMinute) + "m";
            }
            else if (epoch10mDuration > oneSecond) {
                return (epoch10mDuration / oneSecond) + "s";
            }
            else if (epoch10mDuration > oneMillisecond) {
                return (epoch10mDuration / oneMillisecond) + "ms";
            }
            else if (epoch10mDuration > oneMicrosecond) {
                return (epoch10mDuration / oneMicrosecond) + "us";
            }
            else {
                return (epoch10mDuration) + "00ns";
            }
        }

    }
}