using DataOperate.Net;
using Mobile.PrinxChengShan.Dal;
using Mobile.PrinxChengShan.Model;
using Mobile.PrinxChengShan.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace Mobile.PrinxChengShan.Bll
{
    public class OnSiteInspectionBll
    {
        private XmlHelper xml = null;
        private OnSiteInspectionDal dal = null;
        public OnSiteInspectionBll()
        {
            dal = new OnSiteInspectionDal();
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
                    returnDate = GetTemplate(context);
                    break;
                case "selHALF":
                    returnDate = GetHALF(context);
                    break;
                case "selGT":
                    returnDate = GetGT(context);
                    break;
                case "lstGT":
                    returnDate = GetQMA1000List_LTA0001(context);
                    break;
                case "lstHALF":
                    returnDate = GetQMA1000List_LTC0001(context);
                    break;
                case "addHALF":
                    returnDate = CreateHALF(context);
                    break;
                case "addGT":
                    returnDate = CreateGT(context);
                    break;
                default:
                    returnDate = JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("404", xml.ReadLandXml("404")));
                    break;
            }
            return returnDate;
        }
        private string CreateHALF(HttpContext context)
        {
            try
            {
                string jsList = context.Request["dataList"];
                List<PublicEntity> list = new ComUtilDal().StringToModel(jsList);
                QMA1001 model = new QMA1001
                {
                    FAC = context.Request["FAC"],
                    DIV = context.Request["DIV"],
                    LOTNO = context.Request["BARCODE"],
                    ENAM = context.Request["ENAM"],
                    LOGINNAM = context.Request["LOGINNAM"],
                    RESULT = context.Request["RESULT"]
                };
                DataTable dtComUtil = new ComUtilDal().GetShift();
                model.WDATE = DateTime.Parse(dtComUtil.Rows[0]["WDATE"].ToString());
                model.SHIFT = dtComUtil.Rows[0]["SHT"].ToString();

                if (dal.CreateHALF(model, list))
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
        private string CreateGT(HttpContext context)
        {
            try
            {
                string jsList = context.Request["dataList"];
                List<PublicEntity> list = new ComUtilDal().StringToModel(jsList);
                QMA1001 model = new QMA1001
                {
                    FAC = context.Request["FAC"],
                    DIV = context.Request["DIV"],
                    LOTNO = context.Request["BARCODE"],
                    ENAM = context.Request["ENAM"],
                    LOGINNAM = context.Request["LOGINNAM"],
                    RESULT = context.Request["RESULT"]
                };
                DataTable dtComUtil = new ComUtilDal().GetShift();
                model.WDATE = DateTime.Parse(dtComUtil.Rows[0]["WDATE"].ToString());
                model.SHIFT = dtComUtil.Rows[0]["SHT"].ToString();
              
                if (dal.CreateGT(model,list))
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
        private string GetGT(HttpContext context)
        {
            try
            {
                string _BARCODE = context.Request["BARCODE"] as string;
                return JsonHelper<Messaging<LTA0001>>.EntityToJson(new Messaging<LTA0001>("0", "", new LTA0001Dal().GetByModel(_BARCODE.Trim())));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
        private string GetHALF(HttpContext context)
        {
            try
            {
                string _LOTID = context.Request["LOTID"] as string;
                return JsonHelper<Messaging<LTC0001>>.EntityToJson(new Messaging<LTC0001>("0", "", new LTC0001Dal().GetByModel(_LOTID.Trim())));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
        private string GetQMA1000List_LTC0001(HttpContext context)
        {
            try
            {
                string _DIV = context.Request["DIV"] as string;
                string _LOTID = context.Request["LOTID"] as string;
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", "", dal.GetQMA1000List_LTC0001(_DIV.Trim(), _LOTID.Trim())));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
        private string GetQMA1000List_LTA0001(HttpContext context)
        {
            try
            {
                string _DIV = context.Request["DIV"] as string;
                string _BARCODE = context.Request["BARCODE"] as string;
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", "", dal.GetQMA1000List_LTA0001(_DIV.Trim(), _BARCODE.Trim())));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
        private string GetTemplate(HttpContext context)
        {
            try
            {
                string _TYPE = context.Request["TYPE"] as string;
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", "", dal.GetTemplate(_TYPE.Trim())));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
    }
}

