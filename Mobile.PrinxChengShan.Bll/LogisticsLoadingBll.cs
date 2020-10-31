using DataOperate.Net;
using Mobile.PrinxChengShan.Dal;
using Mobile.PrinxChengShan.Model;
using Mobile.PrinxChengShan.Util;
using System;
using System.Data;
using System.Web;

namespace Mobile.PrinxChengShan.Bll
{
    public class LogisticsLoadingBll
    {
        private XmlHelper xml = null;
        private LogisticsLoadingDal dal = null;
        public LogisticsLoadingBll()
        {
            dal = new LogisticsLoadingDal();
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
                    returnDate = GetLoadingList(context);
                    break;
                case "by":
                    returnDate = GetByModel(context);
                    break;
                case "del":
                    returnDate = DelOddNumbers(context);
                    break;
                case "new":
                    returnDate = NewlyBuild(context);
                    break;
                case "subls":
                    returnDate = GetSubList(context);
                    break;
                case "acc":
                    returnDate = AddCargoCage(context);
                    break;
                case "down":
                    returnDate = UpDownCargoCage(context);
                    break;
                case "ok":
                    returnDate = ConfirmCargoCage(context);
                    break;
                default:
                    returnDate = JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("404", xml.ReadLandXml("404")));
                    break;
            }
            return returnDate;
        }

        private string ConfirmCargoCage(HttpContext context)
        {
            try
            {
                string Warehousereceipt = context.Request["wh"].Trim();
                if (dal.ConfirmCargoCage(Warehousereceipt))
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
        private string UpDownCargoCage(HttpContext context)
        {
            try
            {
                string _Warehousereceipt = context.Request["wh"] as string;
                string _ShelfNumber = context.Request["num"] as string;

                string _ENAM = context.Request["ENAM"] as string;
                string _LOGINNAM = context.Request["LOGINNAM"] as string;
                if (dal.UpDownCargoCage(_Warehousereceipt.Trim(), _ShelfNumber.Trim(), _ENAM.Trim(), _LOGINNAM.Trim()))
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", xml.ReadLandXml("0"), dal.GetSubList(_Warehousereceipt.Trim())));
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
        private string AddCargoCage(HttpContext context)
        {
            try
            {
                string _ENAM = context.Request["ENAM"] as string;
                string _LOGINNAM = context.Request["LOGINNAM"] as string;
                string _Warehousereceipt = context.Request["wh"] as string;
                if (string.IsNullOrEmpty(_Warehousereceipt))
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("1", "请选择装车单！"));
                }
                string _ShelfNumber = context.Request["num"].Trim();
                string _FAC = context.Request["FAC"].Trim();
                STC0018 mds = dal.GetByModels(_Warehousereceipt.Trim());
                STC0017 model = dal.CheckCargoCage(_ShelfNumber, _FAC);
                DataTable stc17Tb = new STC0017Dal().GetModel(_ShelfNumber);
                string mess17 = string.Format(@"<br/>库位:[{0}]<br/>规格:[{1}]<br/>数量：[{2}]条。", model.Storehouse, model.ITDSC, model.Number);
                if (mds.STATE == "3")
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("1", "装车单已经确认入库无法操作！"));
                }
                if (!string.IsNullOrEmpty(model.ShelfNumber))
                {
                    if (dal.AddCargoCage(_Warehousereceipt.Trim(), model.Transfercardnumber, model.ShelfNumber, _ENAM, _LOGINNAM))
                    {
                        return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", "装车完成!" + mess17, dal.GetSubList(_Warehousereceipt.Trim())));
                    }
                    else
                    {
                        return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("1", xml.ReadLandXml("1")));
                    }
                }
                else
                {
                    string mes = dal.CheckCargoCageExis(_ShelfNumber, _FAC);
                    if (!string.IsNullOrEmpty(mes))
                    {
                        if (mes == "4")
                        {
                            string mess19 = string.Format(@"<br/>库位:[{0}]<br/>规格:[{1}]<br/>数量：[{2}]条。", stc17Tb.Rows[0]["Storehouse"], stc17Tb.Rows[0]["ITDSC"], stc17Tb.Rows[0]["Number"]);
                            return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", "已装车！装车单:【" + dal.GetWarehousereceipt(stc17Tb.Rows[0]["Transfercardnumber"] as string, stc17Tb.Rows[0]["ShelfNumber"] as string) + "】" + mess19));
                        }
                        else
                        {
                            return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", "货架已入库！"));
                        }
                    }
                    else
                    {
                        return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "未查询到货笼信息！"));
                    }
                }
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
        private string GetSubList(HttpContext context)
        {
            try
            {
                string Warehousereceipt = context.Request["wh"].Trim();
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", "", dal.GetSubList(Warehousereceipt)));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
        private string NewlyBuild(HttpContext context)
        {
            try
            {
                STC0018 model = new STC0018
                {
                    STATE = "0",
                    Disknumber = 0,
                    total = 0,
                    Consignor = context.Request["ENAM"].Trim(),
                    FAC = context.Request["FAC"].Trim(),
                    ENAM = context.Request["ENAM"].Trim(),
                    LOGINNAM = context.Request["LOGINNAM"].Trim(),
                    Typestrans = "入库"
                };
                if (dal.NewlyBuild(model))
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

        private string DelOddNumbers(HttpContext context)
        {
            try
            {
                string Warehousereceipt = context.Request["wh"].Trim();
                int totalNum = dal.GetTotalLoading(Warehousereceipt.Trim());
                if (totalNum == 0)
                {
                    if (dal.DelOddNumbers(Warehousereceipt.Trim()))
                    {
                        return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", xml.ReadLandXml("0")));
                    }
                    else
                    {
                        return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("1", xml.ReadLandXml("1")));
                    }
                }
                else
                {
                    string mess = string.Format("[{0}]装车单下还有[{1}]架货笼,<br/>请先将货笼卸车再删除单据！", Warehousereceipt, totalNum);
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("1", mess));
                }
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }

        /// <summary>
        /// 获取装车单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetLoadingList(HttpContext context)
        {
            try
            {
                string FAC = context.Request["FAC"].Trim();
                return JsonHelper<Messaging<LTA0001>>.EntityToJson(new Messaging<LTA0001>("0", xml.ReadLandXml("0"), dal.GetLoadingList(FAC.Trim())));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
        /// <summary>
        /// 获取装车主单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetByModel(HttpContext context)
        {
            try
            {
                string Warehousereceipt = context.Request["wh"].Trim();
                return JsonHelper<Messaging<LTA0001>>.EntityToJson(new Messaging<LTA0001>("0", xml.ReadLandXml("0"), dal.GetByModel(Warehousereceipt.Trim())));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
    }
}

