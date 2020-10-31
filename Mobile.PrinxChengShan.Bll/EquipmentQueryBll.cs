using DataOperate.Net;
using Mobile.PrinxChengShan.Dal;
using Mobile.PrinxChengShan.Model;
using Mobile.PrinxChengShan.Util;
using System;
using System.Web;

namespace Mobile.PrinxChengShan.Bll
{
    public class EquipmentQueryBll
    {
        private XmlHelper xml = null;
        private EquipmentQueryDal dal = null;
        public EquipmentQueryBll()
        {
            dal = new EquipmentQueryDal();
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
                    returnDate = GetQueryData(context);
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
        private string GetQueryData(HttpContext context)
        {
            try
            {
                string MCHID = context.Request["MCHID"].Trim();
                return JsonHelper<Messaging<EDC0001>>.EntityToJson(new Messaging<EDC0001>("0", xml.ReadLandXml("0"), new EQA0005Dal().GetByModel(MCHID.Trim()),dal.GetQuery(MCHID.Trim())));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
    }
}
