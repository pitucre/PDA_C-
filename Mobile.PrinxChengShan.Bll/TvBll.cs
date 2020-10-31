using DataOperate.Net;
using Mobile.PrinxChengShan.Dal;
using Mobile.PrinxChengShan.Model;
using System;
using System.Web;

namespace Mobile.PrinxChengShan.Bll
{
    public class TVBll
    {
        private TVDal dal = null;
        public TVBll()
        {
            dal = new TVDal();
        }
        /// <summary>
        /// 请求入口
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string ProcessRequest(HttpContext context)
        {
            string action = string.Empty;
            try { action = context.Request["action"].ToString(); }
            catch { action = string.Empty; }
            string returnDate = string.Empty;
            switch (action)
            {
                case "menu":
                    returnDate = GetTVMenu(context);
                    break;
                case "up":
                    returnDate = AutoUpdate(context);
                    break;
                case "half":
                    returnDate = GetInventorySemiFinishedProducts(context);
                    break;
                default:
                    returnDate = JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("404", "请求错误"));
                    break;
            }
            return returnDate;
        }

        private string AutoUpdate(HttpContext context)
        {
            try
            {
                string configFile = HttpContext.Current.Server.MapPath("~/Version/VersionTV.ini");
                ConfigIni conIni = new ConfigIni(configFile);
                return "{\"appid\":\"" + conIni.GetStringValue("appid") + "\",\"title\":\"" + conIni.GetStringValue("title") + "\",\"note\":\"" + conIni.GetStringValue("note") + "\",\"version\":\"" + conIni.GetStringValue("version") + "\",\"url\":\"" + conIni.GetStringValue("url") + "\"}";
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message));
            }
        }
        private string GetInventorySemiFinishedProducts(HttpContext context)
        {
            try
            {
                int pageSize = 9;
                string Index = context.Request["Index"].Trim();
                int firstIndex = 0;
                try { firstIndex = Convert.ToInt32(Index); } catch { }
                string _fac = context.Request["FAC"].Trim();
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", dal.GetCount(_fac.Trim(), pageSize).ToString(), dal.GetInventorySemiFinishedProducts(_fac.Trim(), pageSize, firstIndex)));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }

        private string GetTVMenu(HttpContext context)
        {
            try
            {
                string _IP = context.Request["IP"].Trim();
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", "", dal.GetTVMenu(_IP.Trim())));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
    }
}

