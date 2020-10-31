using DataOperate.Net;
using Mobile.PrinxChengShan.Dal;
using Mobile.PrinxChengShan.Model;
using Mobile.PrinxChengShan.Util;
using System;
using System.Web;

namespace Mobile.PrinxChengShan.Bll
{
    public class RevokingShelvesBll
    {
        private XmlHelper xml = null;
        private RevokingShelvesDal dal = null;
        public RevokingShelvesBll()
        {
            dal = new RevokingShelvesDal();
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
                case "sel":
                    returnDate = GetSelectData(context);
                    break;
                case "up":
                    returnDate = GetShelvesUndo(context);
                    break;
                default:
                    returnDate = JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("404", xml.ReadLandXml("404")));
                    break;
            }
            return returnDate;
        }
        /// <summary>
        /// 通过条码查询相对应的产品信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetSelectData(HttpContext context)
        {
            try
            {
                string BARCODE = context.Request["BARCODE"].Trim();
                return JsonHelper<Messaging<LTA0001>>.EntityToJson(new Messaging<LTA0001>("0", xml.ReadLandXml("0"), dal.GetSelectData(BARCODE.Trim())));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetShelvesUndo(HttpContext context)
        {
            try
            {
                string LNO = context.Request["LNO"].Trim();
                string LOGINNAM = context.Request["LOGINNAM"].Trim();
                string NAME = context.Request["ENAM"].Trim();
                string Transfercardnumber = context.Request["TF"].Trim();
                if (dal.GetShelvesUndo(LNO, LOGINNAM, NAME, Transfercardnumber))
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
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
    }
}

