using System;
using System.Runtime.InteropServices;
using System.Text;

namespace xx {
    public static class Chrono {
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
            return DateTimeToEpoch10m(t.AddHours(-8));
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