using DataOperate.Net;
using Mobile.PrinxChengShan.Dal;
using Mobile.PrinxChengShan.Model;
using System;
using System.Web;
using Mobile.PrinxChengShan.Util;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.IO;



namespace Mobile.PrinxChengShan.Bll
{
   public class FixedAssetInfoBll
    {
        private XmlHelper xml = null;
        private FixedAssetInfoDal dal = null;
        public FixedAssetInfoBll()
        {
            dal = new FixedAssetInfoDal();
            xml = new XmlHelper();

        }
        /// <summary>
        /// 请求入口
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string ProcessRequest(HttpContext context)
        {

            //string _lang = "CHN";
            //try
            //{
            //    string lang = context.Request["lang"] as string;
            //    if (!string.IsNullOrEmpty(lang))
            //    {
            //        _lang = lang;
            //    }
            //}
            //catch { _lang = "CHN"; }
            //xml.FilePath = context.Server.MapPath(string.Format("~/Language/{0}.xml", _lang));
            string strAction = string.Empty;
            try { strAction = context.Request["action"].ToString(); }
            catch { strAction = string.Empty; }
            string returnDate = string.Empty;
            switch (strAction)
            {
                case "download":
                    returnDate = Download(context);
                    break;
                case "downloadjson":
                    returnDate = DownloadJson(context);
                    break;
                case "downloadmysql":
                    returnDate = DownloadMySql(context);
                    break;
                case "upload":
                    returnDate = Upload(context);
                    break;

                default:
                    returnDate = JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("404", xml.ReadLandXml("404")));
                    break;
            }
            return returnDate;

        }
        /// <summary>
        /// 获取菜单权限
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Download(HttpContext context)
        {
            try
            {
                string strDepartment = context.Request["ln"] as string;
                string strIndex = context.Request["test"] as string;
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", "",DataTableToString(dal.GetFixedAssetInfo(strDepartment,strIndex))));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
        private string DownloadJson(HttpContext context)
        {
            try
            {
                string strDepartment = context.Request["ln"] as string;
                string strIndex = context.Request["test"] as string;
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", "", (dal.GetFixedAssetInfo(strDepartment, strIndex))));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }


        private string DownloadMySql(HttpContext context)
        {
            try
            {
                string strDepartment = context.Request["ln"] as string;
                string strIndex = context.Request["test"] as string;
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", "", (dal.GetFixedAssetInfoMysql(strDepartment))));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
        private string Upload(HttpContext context)
        {
            try
            {
                string jsList = context.Request["dataList"];

                List<FixedAssetInfoModel> list = new ComUtilDal().StringToModel(jsList);

                FixedAssetInfoModel model = new FixedAssetInfoModel();
                {
                    //FAC = context.Request["FAC"],
                    //DIV = context.Request["DIV"],
                    //LOTNO = context.Request["BARCODE"],
                    //ENAM = context.Request["ENAM"],
                    //LOGINNAM = context.Request["LOGINNAM"],
                    //RESULT = context.Request["RESULT"]
                };
                //DataTable dtComUtil = new ComUtilDal().GetShift();
                //model.WDATE = DateTime.Parse(dtComUtil.Rows[0]["WDATE"].ToString());
                //model.SHIFT = dtComUtil.Rows[0]["SHT"].ToString();

                if (dal.Upload(model, list))
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", xml.ReadLandXml("0")));
                }
                else
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("1", xml.ReadLandXml("1")));
                }
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Replace("\r\n", "")));
            }
        }


        /// <summary>
        /// DataTable 到 string
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableToString(DataTable dt)
        {
            //!@&,#$%,^&*为字段的拼接字符串
            //为了防止连接字符串不在DataTable数据中存在，特意将拼接字符串写成特殊的字符！
            StringBuilder strData = new StringBuilder();
            //StringWriter sw = new StringWriter();            

            ////DataTable 的当前数据结构以 XML 架构形式写入指定的流
            //dt.WriteXmlSchema(sw);
            //strData.Append(sw.ToString());
            //sw.Close();
            //strData.Append("@&@");
            for (int i = 0; i < dt.Rows.Count; i++)           //遍历dt的行
            {
                DataRow row = dt.Rows[i];
                if (i > 0)                                    //从第二行数据开始，加上行的连接字符串
                {
                    strData.Append("$$");
                }
                for (int j = 0; j < dt.Columns.Count; j++)    //遍历row的列
                {
                    if (j > 0)                                //从第二个字段开始，加上字段的连接字符串
                    {
                        strData.Append("|");
                    }
                    strData.Append(Convert.ToString(row[j])); //取数据
                }
            }

            return strData.ToString();
        }


    }
}
