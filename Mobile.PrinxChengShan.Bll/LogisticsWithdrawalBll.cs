using DataOperate.Net;
using Mobile.PrinxChengShan.Dal;
using Mobile.PrinxChengShan.Model;
using Mobile.PrinxChengShan.Util;
using System;
using System.Web;

namespace Mobile.PrinxChengShan.Bll
{
    public class LogisticsWithdrawalBll
    {
        private XmlHelper xml = null;
        private LogisticsWithdrawalDal dal = null;
        public LogisticsWithdrawalBll()
        {
            dal = new LogisticsWithdrawalDal();
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
                case "list":
                    returnDate = GetReturnnumber(context);
                    break;
                case "by":
                    returnDate = GetByModel(context);
                    break;
                case "ref":
                    returnDate = RefreshData(context);
                    break;
                case "sca":
                    returnDate = ScanningInsert(context);
                    break;
                case "sel":
                    returnDate = GetSTC0020Model(context);
                    break;
                case "add":
                    returnDate = ConfirmDocuments(context);
                    break;
                default:
                    returnDate = JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("404", xml.ReadLandXml("404")));
                    break;
            }
            return returnDate;
        }
        private string GetSTC0020Model(HttpContext context)
        {
            try
            {
                string _BARCODE = context.Request["BARCODE"] as string;
                return JsonHelper<Messaging<LTA0001>>.EntityToJson(new Messaging<LTA0001>("0", "", dal.GetSTC0020Model(_BARCODE.Trim())));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }

        public string ScanningInsert(HttpContext context)
        {
            string Returnnumber = context.Request["Returnnumber"].Trim();
            string BARCODE = context.Request["BARCODE"].Trim();
            string ENAM = context.Request["ENAM"].Trim();
            string LOGINNAME = context.Request["LOGINNAME"].Trim();
            if (dal.Exist(Returnnumber, BARCODE))
            {
                if (dal.ScanningRetreatCheck(Returnnumber, BARCODE))
                {
                    if (dal.ScanningInsert(Returnnumber, BARCODE))
                    {
                        return JsonHelper<Messaging<STC0029>>.EntityToJson(new Messaging<STC0029>("0", xml.ReadLandXml("0"), dal.GetSTC0029Model(BARCODE)));
                    }
                    else
                    {
                        return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("1", xml.ReadLandXml("1")));
                    }
                }
                else
                {
                    string sta = "0";
                    if (dal.ScanningHistoryCheck(BARCODE))
                    {
                        sta = "1";
                    }
                    else
                    {
                        sta = "2";
                    }
                    if (dal.ScanningUnusualInsert(Returnnumber, BARCODE, sta, ENAM, LOGINNAME))
                    {
                        return JsonHelper<Messaging<STC0029>>.EntityToJson(new Messaging<STC0029>("4", "警告：扫描的轮胎无信息报警处理！"));
                    }
                    else
                    {
                        return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("1", xml.ReadLandXml("1")));
                    }
                }
            }
            else
            {
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("1", "条码已经扫描使用过！"));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string ConfirmDocuments(HttpContext context)
        {
            try
            {
                string Returnnumber = context.Request["Returnnumber"].Trim();
                string ENAM = context.Request["ENAM"].Trim();
                string LOGINNAME = context.Request["LOGINNAME"].Trim();
                if (dal.ConfirmDocuments(Returnnumber, ENAM, LOGINNAME))
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
        /// <summary>
        /// 刷新退库单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string RefreshData(HttpContext context)
        {
            try
            {
                if (dal.RefreshData())
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
        /// <summary>
        /// 获取退库单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetReturnnumber(HttpContext context)
        {
            try
            {
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", xml.ReadLandXml("0"), dal.GetReturnnumber()));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
        /// <summary>
        /// 获取退库单单个信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetByModel(HttpContext context)
        {
            try
            {
                string Returnnumber = context.Request["Returnnumber"].Trim();
                return JsonHelper<Messaging<STC0020>>.EntityToJson(new Messaging<STC0020>("0", xml.ReadLandXml("0"), dal.GetByModel(Returnnumber)));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
    }
}
