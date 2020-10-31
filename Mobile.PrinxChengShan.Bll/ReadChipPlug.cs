using System;
using System.Globalization;
using System.Text;

namespace Mobile.PrinxChengShan.Bll
{
    public class ReadChipPlug
    {
        public static string ToHexString(string hex)
        {
            if (24 != hex.Length)
            {
                throw new Exception("RFID芯片识别到多个芯片，请扫描一个芯片操作！");
            }
            try
            {
                if (hex.Length % 2 != 0)
                {
                    hex += "20";
                }
                string result = string.Empty;
                byte[] bytes = new byte[hex.Length / 2];
                for (int i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = byte.Parse(hex.Substring(i * 2, 2), NumberStyles.HexNumber);
                }
                Encoding chs = Encoding.GetEncoding("UTF-8");
                string str = chs.GetString(bytes);
                if (str.Substring(0, 5).Equals("00000"))
                {
                    return str.Replace("00000", "").Trim();
                }
                else
                {
                    return str;
                }
            }
            catch
            {
                throw new Exception("输入的16进制字符串的格式不正确！");
            }
        }
    }
}
