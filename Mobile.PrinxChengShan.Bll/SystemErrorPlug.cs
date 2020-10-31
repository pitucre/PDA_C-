using System;
using System.Text;
using System.IO;
using System.Web;

namespace Mobile.PrinxChengShan.Bll
{
    /// <summary>
    /// 错误日志插件
    /// </summary>
    public class SystemErrorPlug
    {
        /// <summary>
        /// 错误信息写出
        /// </summary>
        /// <param name="exception"></param>
        public static void ErrorRecord(string exception)
        {
            try
            {
                string mess = "\r\n----------------------------------------------------------\r\n";
                string fileLogPath = HttpContext.Current.Server.MapPath("~/Logs/");
                if (!Directory.Exists(fileLogPath))
                {
                    Directory.CreateDirectory(fileLogPath);
                }
                fileLogPath = Path.Combine(fileLogPath, DateTime.Now.ToString("yyyy-MM-dd") + "ErrorLog.log");
                mess += "ErrorTime:[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]\r\n ErrorMessage:\r\n" + exception;
                File.AppendAllText(fileLogPath, mess, Encoding.UTF8);
            }
            catch
            {
                //throw new Exception("写入错误日志错误，请处理服务器日志文件夹的权限!");
            }
        }
    }
}
