﻿namespace Agebull.ZeroNet.Core
{
    /// <summary>
    /// 站点状态
    /// </summary>
    public class StationState
    {
        /// <summary>
        /// 无，刚构造
        /// </summary>
        public const int None = 0;

        /// <summary>
        /// 需要重启
        /// </summary>
        public const int ConfigError = 1;

        /// <summary>
        /// 错误状态
        /// </summary>
        public const int Failed = 2;

        /// <summary>
        /// 已初始化
        /// </summary>
        public const int Initialized = 3;

        /// <summary>
        /// 正在启动
        /// </summary>
        public const int Start = 4;

        /// <summary>
        /// 正在运行
        /// </summary>
        public const int BeginRun = 5;

        /// <summary>
        /// 正在运行
        /// </summary>
        public const int Run = 6;

        /// <summary>
        /// 已暂停
        /// </summary>
        public const int Pause = 7;

        /// <summary>
        /// 将要关闭
        /// </summary>
        public const int Closing = 8;

        /// <summary>
        /// 已关闭
        /// </summary>
        public const int Closed = 9;

        /// <summary>
        /// 已销毁，析构已调用
        /// </summary>
        public const int Destroy = 10;

        /// <summary>
        /// 已销毁，析构已调用
        /// </summary>
        public const int Disposed = 11;

        /// <summary>
        /// 返回状态文本
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static string Text(int state)
        {
            switch (state)
            {
                case None:
                    return "None";
                case ConfigError:
                    return "ConfigError";
                case Failed:
                    return "Failed";
                case Start: // 4;
                    return "Start";
                case BeginRun: // 5;
                    return "BeginRun";
                case Run: // 5;
                    return "Run";
                case Pause: // 6;
                    return "Pause";
                case Closing: // 7;
                    return "Closing";
                case Closed: // 8;
                    return "Closed";
                case Destroy: // 9;
                    return "Destroy";
                default:
                    return "Undefine";
            }
        }
    }
}