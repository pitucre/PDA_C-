using DataOperate.Net;
using Mobile.PrinxChengShan.Dal;
using Mobile.PrinxChengShan.Model;
using Mobile.PrinxChengShan.Util;
using System;
using System.Web;

namespace Mobile.PrinxChengShan.Bll
{
    public class IFPDASCANDATA_Bll
    {
        private XmlHelper xml = null;
        private IFPDASCANDATA_Dal dal = null;
        public IFPDASCANDATA_Bll()
        {
            dal = new IFPDASCANDATA_Dal();
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
                case "up":
                    returnDate = UpdateInterBarCode(context);
                    break;
                case "eqList":
                    returnDate = GetMchidList(context);
                    break;
                default:
                    returnDate = JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("404", xml.ReadLandXml("404")));
                    break;
            }
            return returnDate;
        }

        private string GetMchidList(HttpContext context)
        {
            try
            {
                string _TYPECOD = "XG001";
                string _FAC = context.Request["FAC"].Trim();
                return JsonHelper<Messaging<EDC0001>>.EntityToJson(new Messaging<EDC0001>("0", xml.ReadLandXml("0"), new ComUtilDal().GetMchidList(_TYPECOD, _FAC)));
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
        private string UpdateInterBarCode(HttpContext context)
        {
            try
            {
                if (dal.UpdateInterBarCode(context.Request["BARCODE"].Trim(), context.Request["LOGINNAM"].Trim(), context.Request["FAC"].Trim(), context.Request["MCHID"].Trim()) > 0)
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", xml.ReadLandXml("0") + "-" + DateTime.Now.ToString("HH:mm:ss")));
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

