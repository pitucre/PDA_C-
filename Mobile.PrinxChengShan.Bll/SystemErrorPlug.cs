using System;
using System.Text;
using System.IO;
using System.Web;

namespace Mobile.PrinxChengShan.Bll
{
    /// <summary>
    /// ������־���
    /// </summary>
    public class SystemErrorPlug
    {
        /// <summary>
        /// ������Ϣд��
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
                //throw new Exception("д�������־�����봦���������־�ļ��е�Ȩ��!");
            }
        }
    }
}
