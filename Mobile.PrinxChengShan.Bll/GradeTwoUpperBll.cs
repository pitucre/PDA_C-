using DataOperate.Net;
using Mobile.PrinxChengShan.Dal;
using Mobile.PrinxChengShan.Model;
using Mobile.PrinxChengShan.Util;
using System;
using System.Web;

namespace Mobile.PrinxChengShan.Bll
{
    //作废
    public class GradeTwoUpperBll
    {
        private const string _level = "2";
        private XmlHelper xml = null;
        private UpperShelfDal dal = null;
        public GradeTwoUpperBll()
        {
            dal = new UpperShelfDal();
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
                case "add":
                    returnDate = Create(context);
                    break;
                case "seq":
                    returnDate = GetGeneratingVirtualNumbers(context);
                    break;
                case "ccl":
                    returnDate = GetCargoCageList(context);
                    break;
                case "lst":
                    returnDate = GetDataList(context);
                    break;
                case "wh":
                    returnDate = GetWarehouse(context);
                    break;
                case "loc":
                    returnDate = GetLocation(context);
                    break;
                case "down":
                    returnDate = GetSoldOut(context);
                    break;
                case "undo":
                    returnDate = GetShelvesUndo(context);
                    break;
                case "ins":
                    returnDate = Insert(context);
                    break;
                case "addCage":
                    returnDate = AddCage(context);
                    break;
                default:
                    returnDate = JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("404", xml.ReadLandXml("404")));
                    break;
            }
            return returnDate;
        }
        private string AddCage(HttpContext context)
        {
            try
            {
                string LNO = context.Request["LNO"].Trim();
                string Storehouse = context.Request["Storehouse"].Trim();
                string LOGINNAM = context.Request["LOGINNAM"].Trim();
                if (dal.MaximumQuantity(_level, LOGINNAM))
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "空货笼已经达到最大数量10！"));
                }
                if (dal.CheckUpCage17(LNO) > 0)
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "货笼已经使用！"));
                }
                STC0009 stc9 = new STC0009
                {
                    LNO = LNO,
                    LOGINNAM = LOGINNAM,
                    Storehouse = Storehouse,
                    LEVEL = _level
                };
                if (dal.Exists(LNO, _level))
                {
                    if (dal.Create(stc9))
                    {
                        return JsonHelper<Messaging<STC0009>>.EntityToJson(new Messaging<STC0009>("0", xml.ReadLandXml("0"), dal.GetDataList(LNO, LOGINNAM)));
                    }
                    else
                    {
                        return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("1", xml.ReadLandXml("1")));
                    }
                }
                else
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("1", "货笼已经使用中！"));
                }
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Insert(HttpContext context)
        {
            try
            {
                string LNO = context.Request["LNO"].Trim();
                string LOGINNAM = context.Request["LOGINNAM"].Trim();
                string NAME = context.Request["ENAM"].Trim();
                string fac = context.Request["FAC"].Trim();
                string _storehouse = context.Request["Storehouse"].Trim();
                // if (dal.Insert(LNO, LOGINNAM, NAME, fac, _storehouse))
                // if (true)
               // {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", xml.ReadLandXml("0")));
               // }
               // else
               // {
                 //   return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("1", xml.ReadLandXml("1")));
               // }
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
        /// <summary>
        /// 撤销
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetShelvesUndo(HttpContext context)
        {
            try
            {
                string LNO = context.Request["LNO"].Trim();
                string LOGINNAM = context.Request["LOGINNAM"].Trim();
                string NAME = context.Request["NAME"].Trim();
                string fac = context.Request["FAC"].Trim();
                if (dal.GetShelvesUndo(LNO, LOGINNAM, _level))
                {
                    return JsonHelper<Messaging<STC0009>>.EntityToJson(new Messaging<STC0009>("0", xml.ReadLandXml("0")));
                }
                else
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "轮胎撤销货笼失败！"));
                }
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
        /// <summary>
        /// 下架
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetSoldOut(HttpContext context)
        {
            try
            {
                string BARCODE = context.Request["BARCODE"].Trim();
                string LOGINNAM = context.Request["LOGINNAM"].Trim();
                STC0009 stc09 = dal.GetBySTC0009(BARCODE, LOGINNAM, _level);
                if (string.IsNullOrEmpty(stc09.BARCODE))
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "该条码的轮胎不在货架上！"));
                }
                if (dal.DelBarCode(stc09.BARCODE, stc09.LOGINNAM, _level))
                {
                    return JsonHelper<Messaging<STC0009>>.EntityToJson(new Messaging<STC0009>("0", xml.ReadLandXml("0"), dal.GetDataList(stc09.LNO, stc09.LOGINNAM, _level)));
                }
                else
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "轮胎下架货笼失败！"));
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
        private string GetLocation(HttpContext context)
        {
            try
            {
                return JsonHelper<Messaging<STC0009>>.EntityToJson(new Messaging<STC0009>("0", xml.ReadLandXml("0"), dal.GetSTC0016()));
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
        private string GetWarehouse(HttpContext context)
        {
            try
            {
                return JsonHelper<Messaging<STC0009>>.EntityToJson(new Messaging<STC0009>("0", xml.ReadLandXml("0"), dal.GetSTC0015()));
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
        private string GetDataList(HttpContext context)
        {
            try
            {
                string LNO = context.Request["LNO"].Trim();
                string LOGINNAM = context.Request["LOGINNAM"].Trim();
                return JsonHelper<Messaging<STC0009>>.EntityToJson(new Messaging<STC0009>("0", xml.ReadLandXml("0"), dal.GetDataList(LNO, LOGINNAM, _level)));
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
        private string GetCargoCageList(HttpContext context)
        {
            try
            {
                string LOGINNAM = context.Request["LOGINNAM"].Trim();
                return JsonHelper<Messaging<STC0009>>.EntityToJson(new Messaging<STC0009>("0", xml.ReadLandXml("0"), dal.GetDataList(LOGINNAM, _level)));
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
        private string GetGeneratingVirtualNumbers(HttpContext context)
        {
            try
            {
                string Storehouse = context.Request["Storehouse"].Trim();
                string LOGINNAM = context.Request["LOGINNAM"].Trim();
                string FAC = context.Request["FAC"].Trim();
                string seq = dal.GetGeneratingVirtualNumbers(FAC.Trim());
                if (dal.MaximumQuantity(_level, LOGINNAM))
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "虚拟空货笼已经达到最大数量10！"));
                }
                STC0009 stc9 = new STC0009
                {
                    LNO = seq,
                    LOGINNAM = LOGINNAM,
                    Storehouse = Storehouse,
                    LEVEL = _level
                };
                if (dal.Create(stc9))
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", seq));
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
        /// 通过条码查询相对应的产品信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Create(HttpContext context)
        {
            try
            {
                string _barcode = context.Request["BARCODE"] as string;
                string _LNO = context.Request["LNO"] as string;
                string _Storehouse = context.Request["Storehouse"] as string;
                string _LOGINNAM = context.Request["LOGINNAM"] as string;
                string _ENAM = context.Request["ENAM"] as string;
                string _BARCODE = _barcode.Trim();
                string LNO = _LNO.Trim();
                string Storehouse = _Storehouse.Trim();
                string LOGINNAM = _LOGINNAM.Trim();
                string ENAM = _ENAM.Trim();
                WIP0010 model = new WIP0010Dal().GetEntityById(_BARCODE);
                if (string.IsNullOrEmpty(model.BARCODE))
                {
                    //扫描异常轮胎报警
                    //dal.CallPolice(_BARCODE, ENAM, LOGINNAM, Storehouse);
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "该条码信息的轮胎不在车间库存中！"));
                }
                if (dal.CheckSTC0017BarCode(model.BARCODE.Trim()) > 0)
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "该条码已经扫描上架！"));
                }

                if (dal.CheckUpCage17(LNO) > 0)
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "货笼已经使用！"));
                }
                if (model.LOCKYN == "Y")
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "该条码信息的轮胎在库存中已经锁定！"));
                }
                if (model.STA != "1")
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "该条码信息的轮胎在库存中处于、不良、保留或者报废状态！"));
                }
               
                QMB0101 qmb0101 = new QMB0101Dal().GetByModel(model.BARCODE);
                  if (qmb0101.QCSTATE.Trim() == "1")
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "该条码信息的轮胎是合格品，无法上架操作！"));
                }
                int barCodeCount = dal.GetBarCodeCount(model.BARCODE);
                if (barCodeCount > 0)
                {
                    STC0009 stc09 = dal.GetBySTC0009(model.BARCODE, LOGINNAM, _level);
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "该条码信息的轮胎已经在[" + stc09.LNO + "]货架上！"));
                }
                int count = dal.GetCount(LNO, model.ITNBR,"", _level);
                int num = dal.GetCargoCageQuantity(model.ITNBR);
                if (0 != num)
                {
                    if (count > num)
                    {
                        return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "该货笼已经达到最大装胎数量，无法在装笼操作！"));
                    }
                }
                else
                {
                    num = 30;
                    if ("03" == model.FAC || "01" == model.FAC)
                    {
                        num = 9;
                    }
                    if (count > num)
                    {
                        return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "该货笼已经达到最大装胎数量，无法在装笼操作！"));
                    }
                }

                STC0009 stc9 = new STC0009
                {
                    BARCODE = model.BARCODE,
                    DBRES = qmb0101.DBRES,
                    Grade = qmb0101.QCSTATE,
                    ITDSC = model.ITDSC,
                    ITNBR = model.ITNBR,
                    LEVEL = _level,
                    LNO = LNO,
                    LOGINNAM = LOGINNAM,
                    Storehouse = Storehouse,
                    TyreNo = qmb0101.TYRENO,
                    QRCode = "",
                    RfidCode = ""
                };
                if (dal.Create(stc9))
                {
                    dal.DeleteCage(stc9.LNO, stc9.LOGINNAM, _level);
                    return JsonHelper<Messaging<STC0009>>.EntityToJson(new Messaging<STC0009>("0", xml.ReadLandXml("0"), dal.GetDataList(LNO, LOGINNAM, _level)));
                }
                else
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "轮胎上架货笼失败！"));
                }
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
    }
}
