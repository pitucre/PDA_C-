using DataOperate.Net;
using Mobile.PrinxChengShan.Dal;
using Mobile.PrinxChengShan.Model;
using Mobile.PrinxChengShan.Util;
using System;
using System.Data;
using System.Web;

namespace Mobile.PrinxChengShan.Bll
{
    public class LocalWarehousingBll
    {
        private XmlHelper xml = null;
        private LocalWarehousingDal dal = null;
        public LocalWarehousingBll()
        {
            dal = new LocalWarehousingDal();
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
                if (!string.IsNullOrWhiteSpace(lang))
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
                case "loc":
                    returnDate = GetLocation(context);
                    break;
                case "list":
                    returnDate = GetLoadingList(context);
                    break;
                case "new":
                    returnDate = NewlyBuild(context);
                    break;
                case "del":
                    returnDate = DelOddNumbers(context);
                    break;
                case "by":
                    returnDate = GetByModel(context);
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
                case "clean":
                    returnDate = CleanOddNumbers(context);
                    break;
                case "rtsel":
                    returnDate = ReturnSelModel(context);
                    break;
                case "rtok":
                    returnDate = ReturnOK(context);
                    break;
                default:
                    returnDate = JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("404", xml.ReadLandXml("404")));
                    break;
            }
            return returnDate;
        }
        private string ReturnOK(HttpContext context)
        {

            try
            {
                string _tyreno = context.Request["TYRENO"].Trim();
                string _enam = context.Request["ENAM"].Trim();
                string _loginname = context.Request["LOGINNAME"].Trim();
                if (dal.ReturnOK(_tyreno, _enam, _loginname))
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
        private string ReturnSelModel(HttpContext context)
        {
            try
            {
                string _tyreno = context.Request["TYRENO"].Trim();
                DataTable dt = dal.GetModel(_tyreno);
                if (dt == null)
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("444", "未查询到成品胎入库的信息！"));
                }
                else
                {
                    WIP0010 wip0010 = new WIP0010Dal().GetEntityById(_tyreno);
                    if (!string.IsNullOrEmpty(wip0010.TYRENO))
                    {
                        return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("444", "成品胎已经退回本地库，请勿重复操作！"));
                    }
                    else
                    {
                        return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", xml.ReadLandXml("0"), dt));
                    }
                }
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }

        private string CleanOddNumbers(HttpContext context)
        {
            try
            {
                string _warehousereceipt = context.Request["wh"].Trim();
                int _num = dal.GetCount(_warehousereceipt);
                if (_num > 0)
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("444", string.Format("入库单号[{0}]下还有[{1}]条成品胎，请先清空再删除操作！", _warehousereceipt, _num)));
                }
                if (dal.Delete(_warehousereceipt))
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
        private string ConfirmCargoCage(HttpContext context)
        {
            try
            {
                string _warehousereceipt = context.Request["wh"] as string;                
                string _LOGINNAM = context.Request["LOGINNAM"] as string;
                string _ENAM = context.Request["ENAM"] as string;               
                int _num = dal.GetCount(_warehousereceipt);
                if (_num == 0)
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("444", string.Format("入库单号[{0}]下没有扫描成品胎信息，无法提交！", _warehousereceipt, _num)));
                }
                if (dal.ConfirmCargoCage(_warehousereceipt.Trim(), _LOGINNAM.Trim(), _ENAM.Trim()))
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
                string _warehousereceipt = context.Request["wh"].Trim();
                string _tyreno = context.Request["TYRENO"].Trim();
                if (dal.CheckSTC0045(_warehousereceipt, _tyreno))
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("44", string.Format(@"该轮胎[{0}]不在入库单上！", _tyreno)));
                }
                if (dal.UpDownCargoCage(_warehousereceipt, _tyreno))
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", xml.ReadLandXml("0"), dal.GetSubList(_warehousereceipt)));
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
                string _warehousereceipt = context.Request["wh"].Trim();
                string _TYRENO = context.Request["TYRENO"].Trim();
               
                string _Storehouse = context.Request["Storehouse"].Trim();
                string _LOGINNAM = context.Request["LOGINNAM"].Trim();
                string _ENAM = context.Request["ENAM"].Trim();
                string _FAC = context.Request["FAC"].Trim();
                if (string.IsNullOrEmpty(_Storehouse))
                {
                    _Storehouse = dal.CheckSTC0044_WareHouse(_warehousereceipt);
                }
                //验证软控报警胎
                string _alarm = new LogisticsLaunchVerificationDal().ALARM(_TYRENO);
                if (!string.IsNullOrWhiteSpace(_alarm))
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("444", _alarm));
                }
                //MES综合验证
                string strMes = dal.GetScanLocalWarehousChecking(_TYRENO, _LOGINNAM, _FAC, _ENAM, _Storehouse);
                if (!string.IsNullOrEmpty(strMes))
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("444", strMes));
                }
                STC0045 stc0045 = dal.GetSTC0045_ITNBR(_warehousereceipt);
                if (!string.IsNullOrEmpty(stc0045.ITNBR))
                {
                    WIP0010 wip0010 = new WIP0010Dal().GetEntityById(_TYRENO);
                    if (!stc0045.ITNBR.Equals(wip0010.ITNBR))
                    {
                        string mess = "此轮胎规格[{0}]与当前货架轮胎规格[{1}]不同，不允许入本地库！";
                        return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("444", string.Format(mess, wip0010.ITDSC, stc0045.ITDSC)));
                    }
                }
                if (dal.AddCargoCage(_warehousereceipt, _TYRENO, _Storehouse, _LOGINNAM, _ENAM))
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", "入库完成!", dal.GetSubList(_warehousereceipt)));
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
        private string GetSubList(HttpContext context)
        {
            try
            {
                string _warehousereceipt = context.Request["wh"].Trim();
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", "", dal.GetSubList(_warehousereceipt)));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
        private string GetLocation(HttpContext context)
        {
            try
            {
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", xml.ReadLandXml("0"), dal.GetSTC0041()));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
        private string GetLoadingList(HttpContext context)
        {
            try
            {
                string _FAC = context.Request["FAC"].Trim();
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", xml.ReadLandXml("0"), dal.GetLoadingList(_FAC.Trim())));
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
                    STATE = "1",
                    Disknumber = 0,
                    total = 0,
                    Consignor = context.Request["ENAM"].Trim(),
                    FAC = context.Request["FAC"].Trim(),
                    ENAM = context.Request["ENAM"].Trim(),
                    LOGINNAM = context.Request["LOGINNAM"].Trim(),
                    Typestrans = "入库"
                };
                string _warehouse = context.Request["Warehouse"].Trim();
                string _OddNumbers = dal.NewlyBuild(model, _warehouse);
                if (!string.IsNullOrEmpty(_OddNumbers))
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", _OddNumbers));
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
                string _warehousereceipt = context.Request["wh"].Trim();
                if (dal.DelOddNumbers(_warehousereceipt.Trim()))
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
        private string GetByModel(HttpContext context)
        {
            try
            {
                string _warehousereceipt = context.Request["wh"].Trim();
                return JsonHelper<Messaging<LTA0001>>.EntityToJson(new Messaging<LTA0001>("0", xml.ReadLandXml("0"), dal.GetByModel(_warehousereceipt.Trim())));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
    }
}

