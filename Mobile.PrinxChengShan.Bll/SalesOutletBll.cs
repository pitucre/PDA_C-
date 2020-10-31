using DataOperate.Net;
using Mobile.PrinxChengShan.Dal;
using Mobile.PrinxChengShan.Model;
using Mobile.PrinxChengShan.Util;
using System;
using System.Data;
using System.Web;

namespace Mobile.PrinxChengShan.Bll
{
    public class SalesOutletBll
    {
        private XmlHelper xml = null;
        private SalesOutletDal dal = null;
        public SalesOutletBll()
        {
            dal = new SalesOutletDal();
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
                    returnDate = GetOutNumberList(context);
                    break;
                case "by":
                    returnDate = GetByModel(context);
                    break;
                case "deta":
                    returnDate = GetDetailedList(context);
                    break;
                case "subls":
                    returnDate = GetSubList(context);
                    break;
                case "sel":
                    returnDate = ScanningInsert(context);
                    break;
                case "emp":
                    returnDate = EmptyBarcode(context);
                    break;
                case "un":
                    returnDate = UnloadBarcode(context);
                    break;
                case "add":
                    returnDate = Create(context);
                    break;
                default:
                    returnDate = JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("404", xml.ReadLandXml("404")));
                    break;
            }
            return returnDate;
        }
        private string Create(HttpContext context)
        {
            try
            {
                string OUTNUMBER = context.Request["OUTNUMBER"] as string;
                string LOGINNAME = context.Request["LOGINNAME"] as string;
                if (dal.Create(OUTNUMBER.Trim(), LOGINNAME.Trim()))
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
        private string UnloadBarcode(HttpContext context)
        {
            try
            {
                string OUTNUMBER = context.Request["OUTNUMBER"] as string;
                string BARCODE = context.Request["BARCODE"] as string;
                if (dal.UnloadBarcode(OUTNUMBER.Trim(), BARCODE.Trim()))
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
        private string EmptyBarcode(HttpContext context)
        {
            try
            {
                string OUTNUMBER = context.Request["OUTNUMBER"] as string;
                if (dal.EmptyBarcode(OUTNUMBER.Trim()))
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
        private string ScanningInsert(HttpContext context)
        {
            try
            {
                string OUTNUMBER = context.Request["OUTNUMBER"] as string;
                string BARCODE = context.Request["BARCODE"] as string;
                string ENAM = context.Request["ENAM"] as string;
                string LOGINNAM = context.Request["LOGINNAM"] as string;
                if (!dal.BarcodeExist(BARCODE.Trim()))
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "该条码信息的轮胎不在库存中！"));
                }
                if (!dal.CheckBarcode(OUTNUMBER.Trim(), BARCODE.Trim()))
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "该条码信息的轮胎已经扫描过！"));
                }
                if (!dal.CheckUserBarcode(BARCODE.Trim()))
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "该条码信息的轮胎已经被别人扫描过！"));
                }
                STC0025 stc0025 = dal.GetByModel(OUTNUMBER.Trim());
                QMB0101 qmb0101 = new QMB0101Dal().GetByModel(BARCODE.Trim());
                //检查扫描的规格是否和SAP发货规格相同
                DataTable dt = dal.GetDetailedList(OUTNUMBER.Trim());
                int countNum = (int)dt.Compute("Count(*)", string.Format(@"ITNBR='{0}'", qmb0101.ITNBR));
                ComUtilDal.SqlRecord(countNum.ToString());
                if (0 == countNum)
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "扫描的轮胎规格和SAP出库单规格不符！"));
                }
                //END
                if ("1" == stc0025.DIV)
                {
                    if ("1" != qmb0101.QCSTATE)
                    {
                        return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "合格品出库单不能扫描出库废次品轮胎！"));
                    }
                }
                else
                {
                    if ("1" == qmb0101.QCSTATE)
                    {
                        return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "废次品出库单不能扫描出库合格品轮胎！"));
                    }
                }
                if (dal.ScanningInsert(OUTNUMBER.Trim(), BARCODE.Trim(), ENAM.Trim(), LOGINNAM.Trim()))
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
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetSubList(HttpContext context)
        {
            try
            {
                string OUTNUMBER = context.Request["OUTNUMBER"].Trim();
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", "", dal.GetSubList(OUTNUMBER)));
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
        private string GetDetailedList(HttpContext context)
        {
            try
            {
                string OUTNUMBER = context.Request["OUTNUMBER"] as string;
                return JsonHelper<Messaging<LTA0001>>.EntityToJson(new Messaging<LTA0001>("0", xml.ReadLandXml("0"), dal.GetDetailedList(OUTNUMBER.Trim())));
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
        private string GetByModel(HttpContext context)
        {
            try
            {
                string OUTNUMBER = context.Request["OUTNUMBER"] as string;
                return JsonHelper<Messaging<STC0025>>.EntityToJson(new Messaging<STC0025>("0", xml.ReadLandXml("0"), dal.GetByModel(OUTNUMBER.Trim()), dal.GetDetailedList(OUTNUMBER.Trim())));
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
        private string GetOutNumberList(HttpContext context)
        {
            try
            {
                string OUTNUMBER = context.Request["OUTNUMBER"] as string;
                return JsonHelper<Messaging<LTA0001>>.EntityToJson(new Messaging<LTA0001>("0", xml.ReadLandXml("0"), dal.GetOutNumberList(OUTNUMBER.Trim())));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
    }
}

