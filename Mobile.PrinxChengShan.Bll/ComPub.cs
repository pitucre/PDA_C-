using DataOperate.Net;
using Mobile.PrinxChengShan.Dal;
using Mobile.PrinxChengShan.Model;
using Mobile.PrinxChengShan.Util;
using System;
using System.Web;

namespace Mobile.PrinxChengShan.Bll
{
    public class ComPub
    {
        private XmlHelper xml = null;
        private ComUtilDal dal = null;
        public ComPub()
        {
            dal = new ComUtilDal();
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
                    returnDate = GetMchidList(context);
                    break;
                case "up":
                    returnDate = Update(context);
                    break;
                default:
                    returnDate = JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("404", xml.ReadLandXml("404")));
                    break;
            }
            return returnDate;
        }
        /// <summary>
        /// 更新工位
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Update(HttpContext context)
        {
            try
            {
                string FAC = context.Request["FAC"].Trim();
                string LOGINNAM = context.Request["LOGINNAM"].Trim();
                string GW = context.Request["GW"].Trim();
                if (new EDC001Dal().Update(GW, LOGINNAM, FAC))
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
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetMchidList(HttpContext context)
        {
            try
            {
                XmlHelper basic = new XmlHelper();
                basic.FilePath = context.Server.MapPath("~/BasicInfo/BasicInfomation.xml");
                string TYPECOD = basic.ReadBasicXml("19");
                string FAC =  context.Request["FAC"].Trim();
                return JsonHelper<Messaging<DropDownListModel>>.EntityToJson(new Messaging<DropDownListModel>("0", xml.ReadLandXml("0"), dal.GetMchidList(TYPECOD, FAC)));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
    }
}
