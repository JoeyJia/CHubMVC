using System;

namespace CHubCommon
{
    public class LogHelper
    {
        public static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");

        public static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");

        public static void WriteLog(string info)
        {

            if (loginfo.IsInfoEnabled)
            {
                loginfo.Info(info);
            }
        }

        //public static async void WriteLogAsync(string info)
        //{
        //    if (loginfo.IsInfoEnabled)
        //    {
        //        await loginfo.Info(info);
        //    }
        //}

        public static void WriteLog(string info, Exception se)
        {
            if (logerror.IsErrorEnabled)
            {
                logerror.Error(info, se);
            }
        }

        public static void WriteErrorLog(string info)
        {
            if (logerror.IsErrorEnabled)
            {
                logerror.Error(info);
            }
        }
    }
}
