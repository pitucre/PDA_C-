using DataOperate.Net;
using Mobile.PrinxChengShan.Dal;
using Mobile.PrinxChengShan.Model;
using Mobile.PrinxChengShan.Util;
using System;
using System.Data;
using System.Web;

namespace Mobile.PrinxChengShan.Bll
{
    /// <summary>
    /// 条码查询
    /// </summary>
    public class BarcodeQueryBll
    {
        private XmlHelper xml = null;
        private BarcodeQueryDal dal = null;
        public BarcodeQueryBll()
        {
            dal = new BarcodeQueryDal();
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
                case "by":
                    returnDate = GetBarcodeQueryData(context);
                    break;
                case "byEP":
                    returnDate = GetBarcodeQueryDataEP(context);
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
        private string GetBarcodeQueryData(HttpContext context)
        {
            try
            {
                string _alarm = "正常";
                string _BARCODE = context.Request["BARCODE"] as string;
                string _FAC = context.Request["FAC"] as string;
                DataTable dt = dal.GetBarcodeQueryData(_BARCODE.Trim());
                if (_FAC == "01")
                {
                    if (dt != null)
                    {
                        if (dt.Rows.Count != 0)
                        {
                            if (!string.IsNullOrWhiteSpace(dt.Rows[0]["TYRENO"].ToString()))
                            {
                                string str = new LogisticsLaunchVerificationDal().ALARM(dt.Rows[0]["TYRENO"].ToString());
                                if (!string.IsNullOrWhiteSpace(str))
                                {
                                    _alarm = "报警胎";
                                }
                                else
                                {
                                    _alarm = "正常";
                                }
                            }
                        }
                    }
                }
                return JsonHelper<Messaging<LTA0001>>.EntityToJson(new Messaging<LTA0001>("0", _alarm, dt));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
        private string GetBarcodeQueryDataEP(HttpContext context)
        {
            try
            {
                string _alarm = string.Empty;
                string BARCODE = context.Request["BARCODE"].Trim();
                DataTable dt = dal.GetBarcodeQueryDataEP(BARCODE.Trim());
                return JsonHelper<Messaging<LTA0001>>.EntityToJson(new Messaging<LTA0001>("0", "", dt));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
    }
}
