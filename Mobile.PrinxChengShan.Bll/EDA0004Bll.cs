using DataOperate.Net;
using Mobile.PrinxChengShan.Dal;
using Mobile.PrinxChengShan.Model;
using Mobile.PrinxChengShan.Util;
using System;
using System.Data;
using System.Web;

namespace Mobile.PrinxChengShan.Bll
{
    public class EDA0004Bll
    {
        private XmlHelper xml = null;
        private EDA0004Dal dal = null;
        public EDA0004Bll()
        {
            dal = new EDA0004Dal();
            xml = new XmlHelper();
        }
        /// <summary>
        /// 请求入口
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string ProcessRequest(HttpContext context)
        {
            string _lang = "CHN";
            try
            {
                string lang = context.Request["lang"] as string;
                if (!string.IsNullOrEmpty(lang))
                {
                    _lang = lang;
                }
            }
            catch { _lang = "CHN"; }
            xml.FilePath = context.Server.MapPath(string.Format("~/Language/{0}.xml", _lang));
            string action = string.Empty;
            try { action = context.Request["action"].ToString(); }
            catch { action = string.Empty; }
            string returnDate = string.Empty;
            switch (action)
            {
                case "sht":
                    returnDate = GetSht(context);
                    break;
                case "sel":
                    returnDate = GetSelectData(context);
                    break;
                case "selParam":
                    returnDate = GetSelectDataParam(context);
                    break;
                default:
                    returnDate = JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("404", xml.ReadLandXml("404")));
                    break;
            }
            return returnDate;
        }
        private string GetSelectDataParam(HttpContext context)
        {
            try
            {
                string DIV = context.Request["DIV"];
                DataTable dataTable = new DataTable();               
                dataTable = dal.GetBaseDataList(DIV);
                dataTable.Rows.Add(new object[] { "0", "不统计" });
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", xml.ReadLandXml("0"), dataTable));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Replace("\r\n", "")));
            }
        }
        /// <summary>
        /// 通过DIV获取数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetSelectData(HttpContext context)
        {
            try
            {
                string DIV = context.Request["DIV"].Trim();
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", xml.ReadLandXml("0"), dal.GetBaseDataList(DIV)));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Replace("\r\n", "")));
            }
        }
        /// <summary>
        /// 获取班
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetSht(HttpContext context)
        {
            try
            {
                string DIV = context.Request["DIV"].Trim();
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", xml.ReadLandXml("0"), dal.GetBaseDataList(DIV)));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Replace("\r\n", "")));
            }
        }
    }
}

