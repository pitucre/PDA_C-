using DataOperate.Net;
using Mobile.PrinxChengShan.Dal;
using Mobile.PrinxChengShan.Model;
using Mobile.PrinxChengShan.Util;
using System;
using System.Web;

namespace Mobile.PrinxChengShan.Bll
{
    public class ProductWithdrawalBll
    {
        private XmlHelper xml = null;
        private ProductWithdrawalDal dal = null;
        public ProductWithdrawalBll()
        {
            dal = new ProductWithdrawalDal();
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
                    returnDate = GetRefundNUMList(context);
                    break;
                case "by":
                    returnDate = GetByModel(context);
                    break;
                case "subl":
                    returnDate = GetSubList(context);
                    break;
                case "sca":
                    returnDate = ScanningInsert(context);
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Create(HttpContext context)
        {
            try
            {
                string _RefundNUM = context.Request["RefundNUM"] as string;
                string _LOGINNAME = context.Request["LOGINNAME"] as string;
                if (dal.Create(_RefundNUM, _LOGINNAME))
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
        private string ScanningInsert(HttpContext context)
        {
            try
            {
                string _BARCODE = context.Request["BARCODE"] as string;
                string _RefundNUM = context.Request["RefundNUM"] as string;
                string _ENAM = context.Request["ENAM"] as string;
                string _LOGINNAME = context.Request["LOGINNAME"] as string;
                if (!dal.CheckUserBarcode(_BARCODE))
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "改条码信息的轮胎正在扫描退库中！"));
                }
                if (dal.ExistStock(_BARCODE))
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "改条码信息的轮胎已经在库存中！"));
                }
                QMB0101 qmb0101 = new QMB0101Dal().GetByModel(_BARCODE.Trim());
                STC0028 stc0028 = new STC0028
                {
                    BARCODE = _BARCODE.Trim(),
                    RefundNUM = _RefundNUM.Trim(),
                    Grade = qmb0101.QCSTATE,
                    ENAM = _ENAM.Trim(),
                    LOGINNAM = _LOGINNAME.Trim(),
                    DBRES = qmb0101.DBRES
                };
                if (dal.ScanningInsert(stc0028))
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
                string RefundNUM = context.Request["RefundNUM"] as string;
                return JsonHelper<Messaging<STC0027>>.EntityToJson(new Messaging<STC0027>("0", xml.ReadLandXml("0"), dal.GetSubList(RefundNUM.Trim())));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
        /// <summary>
        /// 获取详细信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetByModel(HttpContext context)
        {
            try
            {
                string RefundNUM = context.Request["RefundNUM"] as string;
                return JsonHelper<Messaging<STC0027>>.EntityToJson(new Messaging<STC0027>("0", xml.ReadLandXml("0"), dal.GetByModel(RefundNUM.Trim())));
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
        private string GetRefundNUMList(HttpContext context)
        {
            try
            {
                return JsonHelper<Messaging<LTA0001>>.EntityToJson(new Messaging<LTA0001>("0", xml.ReadLandXml("0"), dal.GetRefundNUMList()));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
    }
}

