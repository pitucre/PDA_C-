using DataOperate.Net;
using Mobile.PrinxChengShan.Dal;
using Mobile.PrinxChengShan.Model;
using Mobile.PrinxChengShan.Util;
using System;
using System.Web;

namespace Mobile.PrinxChengShan.Bll
{
    public class FormingBarcodeChangeBll
    {
        private XmlHelper xml = null;
        private FormingBarcodeChangeDal dal = null;
        public FormingBarcodeChangeBll()
        {
            dal = new FormingBarcodeChangeDal();
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
                    returnDate = GetUpdateVulBarcodeChange(context);
                    break;
                default:
                    returnDate = JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("404", xml.ReadLandXml("404")));
                    break;
            }
            return returnDate;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetUpdateVulBarcodeChange(HttpContext context)
        {
            try
            {
                string oldCode = context.Request["oldCode"].Trim();
                string newCode = context.Request["newCode"].Trim();
                string name = context.Request["name"].Trim();
                string login = context.Request["login"].Trim();
                WIP0002 wip0002 = new WIP0002Dal().GetEntityByModel(oldCode);
                if (wip0002.IS_LOCK == "Y")
                {                    
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("1", xml.ReadLandXml("4")));
                }
                if (wip0002.STA == "4")
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("1", xml.ReadLandXml("5")));
                }
                if (dal.GetUpdateFormingBarcodeChange(oldCode, newCode, name, login) > 0)
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