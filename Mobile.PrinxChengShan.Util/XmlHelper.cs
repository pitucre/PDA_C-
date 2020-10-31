using System.Xml;

namespace Mobile.PrinxChengShan.Util
{
    /// <summary>
    /// XML操作基类
    /// </summary>
    public class XmlHelper
    {
        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }
        /// <summary>
        /// 根据语言类型以及代码选择语言信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string ReadLandXml(string key)
        {
            string lang = string.Empty;
            XmlDocument doc = new XmlDocument();
            //加载XML文件
            doc.Load(FilePath);
            //通过key获取信息
            XmlNode node = doc.SelectSingleNode(string.Format("//Languages/Language[@key='{0}']", key));
            if (node != null)
            {
                lang = node.Attributes["value"].Value;
            }
            return lang;
        }
        /// <summary>
        /// 根据代码选择基础数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string ReadBasicXml(string key)
        {
            string lang = string.Empty;
            XmlDocument doc = new XmlDocument();
            //加载XML文件
            doc.Load(FilePath);
            //通过key获取信息
            XmlNode node = doc.SelectSingleNode(string.Format("//Basic/Data[@key='{0}']", key));
            if (node != null)
            {
                lang = node.Attributes["value"].Value;
            }
            return lang;
        }
    }
}
