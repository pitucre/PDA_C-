using DataOperate.Net;
using Mobile.PrinxChengShan.Dal;
using Mobile.PrinxChengShan.Model;
using System;
using System.Data;
using System.Web;
using Mobile.PrinxChengShan.Util;

namespace Mobile.PrinxChengShan.Bll
{
    /// <summary>
    /// MDC0002-模具出库记录
    /// </summary>
    public class MDC0002Bll
    {
		  private XmlHelper xml = null;
        private MDC0002Dal dal = null;
        public MDC0002Bll()
        {
            dal = new MDC0002Dal();
			  xml = new XmlHelper ();
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
                case "add":
                    returnDate = Create(context);
                    break;
                default:
                    returnDate = JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("404",  xml.ReadLandXml("404")));
                    break;
            }
            return returnDate;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Create(HttpContext context)
        {
            try
            {
                //获取工厂时间
                DataTable dtComUtil = new ComUtilDal().GetShift();
                //获取实体类
                MDC0002 model = new MDC0002()
                {
                    WDATE = DateTime.Parse(dtComUtil.Rows[0]["WDATE"].ToString()),
                    LOCATIONID = context.Request["LOCATIONID"].ToString(),
                    LOCATIONNAM = context.Request["LOCATIONNAM"].ToString(),
                    ENAM = context.Request["ENAM"].ToString(),
                    LOGINNAM = context.Request["LOGINNAM"].ToString(),
                    MCHID = context.Request["MCHID"].ToString(),
                    OUTDIV = context.Request["OUTDIV"].ToString()
                };
                string cavidList = context.Request["cavidList"].ToString();
                if (dal.Create(model, cavidList) > 0)
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0",  xml.ReadLandXml("0")));
                }
                else
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("1",  xml.ReadLandXml("1")));
                }
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Replace("\r\n", "")));
            }
        }
    }
}




