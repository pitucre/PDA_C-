using DataOperate.Net;
using Mobile.PrinxChengShan.Dal;
using Mobile.PrinxChengShan.Model;
using Mobile.PrinxChengShan.Util;
using System;
using System.Threading;
using System.Web;

namespace Mobile.PrinxChengShan.Bll
{
    public class UpperShelfBll
    {
        private const string _level = "1";
        private XmlHelper xml = null;
        private UpperShelfDal dal = null;
        public UpperShelfBll()
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
                string _LNO = context.Request["LNO"].Trim();
                string _Storehouse = context.Request["Storehouse"].Trim();
                string _LOGINNAM = context.Request["LOGINNAM"].Trim();
                string _EqID = context.Request["EqID"].Trim();
                //XXXXXXXXXXXXXXX 软控货笼验证 XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                /*string mess = new MESNACMESDal().CheckShelfState(_LNO);
                 if (!string.IsNullOrWhiteSpace(mess))
                 {
                     return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", mess));
                 }*/
                //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
                if (dal.MaximumQuantity(_level, _EqID))
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "空货笼已经达到最大数量3个！"));
                }
                if (dal.CheckUpCage17(_LNO) > 0)
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "[" + _LNO + "]货笼已经使用！"));
                }
                STC0009 stc9 = new STC0009
                {
                    LNO = _LNO,
                    LOGINNAM = _LOGINNAM,
                    Storehouse = _Storehouse,
                    LEVEL = _level,
                    EQUIPID = _EqID
                };
                if (dal.Exists(_LNO, _level))
                {
                    if (dal.Create(stc9))
                    {
                        return JsonHelper<Messaging<STC0009>>.EntityToJson(new Messaging<STC0009>("0", xml.ReadLandXml("0"), dal.GetDataList(_LNO, _EqID)));
                    }
                    else
                    {
                        return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("1", xml.ReadLandXml("1")));
                    }
                }
                else
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("1", "[" + _LNO + "]货笼已经使用中！"));
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
                string _LNO = context.Request["LNO"].Trim();
                string _NAME = context.Request["ENAM"].Trim();
                string _fac = context.Request["FAC"].Trim();
                string _storehouse = context.Request["Storehouse"].Trim();
                string _EqID = context.Request["EqID"].Trim();               
                if (dal.Insert(_LNO, _NAME, _fac, _storehouse, _EqID))
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", dal.GetFirstCargo(_EqID)));
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
        /// 撤销
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetShelvesUndo(HttpContext context)
        {
            try
            {
                string _LNO = context.Request["LNO"].Trim();
                string _EqID = context.Request["EqID"].Trim();
                if (dal.GetShelvesUndo(_LNO, _level, _EqID))
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
                string _BARCODE = context.Request["BARCODE"].Trim();
                string _EqID = context.Request["EqID"].Trim();
                //限制只能扫描镂空条码
                if (_BARCODE.Length != 11)
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("444", "请扫描镂空硫化条码！"));
                }
                STC0009 stc09 = dal.GetBySTC0009(_BARCODE, _EqID, _level);
                if (string.IsNullOrWhiteSpace(stc09.BARCODE))
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "该条码的轮胎不在货架上！"));
                }
                if (dal.DelBarCode(stc09.BARCODE, _EqID, _level))
                {
                    return JsonHelper<Messaging<STC0009>>.EntityToJson(new Messaging<STC0009>("0", xml.ReadLandXml("0"), dal.GetDataList(stc09.LNO, _EqID, _level)));
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
                string _LNO = context.Request["LNO"].Trim();
                string _EqID = context.Request["EqID"].Trim();
                return JsonHelper<Messaging<STC0009>>.EntityToJson(new Messaging<STC0009>("0", xml.ReadLandXml("0"), dal.GetDataList(_LNO, _EqID, _level)));
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
                string _EqID = context.Request["EqID"].Trim();
                return JsonHelper<Messaging<STC0009>>.EntityToJson(new Messaging<STC0009>("0", xml.ReadLandXml("0"), dal.GetDataList(_EqID, _level)));
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
                string _Storehouse = context.Request["Storehouse"].Trim();
                string _LOGINNAM = context.Request["LOGINNAM"].Trim();
                string _FAC = context.Request["FAC"].Trim();
                string _EqID = context.Request["EqID"].Trim();
                string _seq = dal.GetGeneratingVirtualNumbers(_FAC.Trim());
                if (dal.MaximumQuantity(_level, _EqID))
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "虚拟空货笼已经达到最大数量3个！"));
                }
                STC0009 stc9 = new STC0009
                {
                    LNO = _seq,
                    LOGINNAM = _LOGINNAM,
                    Storehouse = _Storehouse,
                    LEVEL = _level,
                    EQUIPID = _EqID
                };
                if (dal.Create(stc9))
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", _seq));
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
                string _EqID = context.Request["EqID"].Trim();
                string _BARCODE = context.Request["BARCODE"].Trim();
                string _LNO = context.Request["LNO"].Trim();
                string _Storehouse = context.Request["Storehouse"].Trim();
                string _LOGINNAM = context.Request["LOGINNAM"].Trim();
                string _ENAM = context.Request["ENAM"].Trim();
                string _FAC = context.Request["FAC"].Trim();
                //限制只能扫描镂空条码
                if (_BARCODE.Length != 11)
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("444", "请扫描镂空硫化条码！"));
                }
                //软控货架验证
                /*string mess = new MESNACMESDal().GetShelfInfo(_BARCODE);
                 if (!string.IsNullOrWhiteSpace(mess))
                 {
                     return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("444", mess));
                 }*/
                //验证软控报警胎
                string _alarm = new LogisticsLaunchVerificationDal().ALARM(_BARCODE);
                if (!string.IsNullOrWhiteSpace(_alarm))
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("444", _alarm));
                }
                //MES综合验证
                string strMes = dal.GetScanShelfChecking(_LNO, _BARCODE, _LOGINNAM, _FAC, _ENAM, _Storehouse);
                if (!string.IsNullOrEmpty(strMes))
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("444", strMes));
                }
                //如果多个货架为空优先使用最新的
                string newLNO = dal.GetNewestLnoNum(_EqID.Trim());
                if (!string.IsNullOrWhiteSpace(newLNO))
                {
                    _LNO = newLNO;
                }
                QMB0101 qmb0101 = new QMB0101Dal().GetByModel(_BARCODE);
                STC0009 stc0009 = dal.GetBySTC0009Model(qmb0101.ITNBR, _EqID, _level);
                //同一个规格默认装载一个货笼
                if (!string.IsNullOrWhiteSpace(stc0009.LNO))
                {
                    _LNO = stc0009.LNO;
                }
                else
                {
                    string derLno = dal.GetFirstCargoASC(_EqID);
                    if (!string.IsNullOrEmpty(derLno))
                    {
                        _LNO = derLno;
                    }
                }
                STC0009 stc9 = new STC0009
                {
                    BARCODE = qmb0101.BARCODE,
                    DBRES = qmb0101.DBRES,
                    Grade = qmb0101.QCSTATE,
                    ITDSC = qmb0101.ITDSC,
                    ITNBR = qmb0101.ITNBR,
                    LEVEL = _level,
                    LNO = _LNO,
                    LOGINNAM = _LOGINNAM,
                    Storehouse = _Storehouse,
                    TyreNo = qmb0101.TYRENO,
                    EQUIPID = _EqID
                };
                if (dal.Create(stc9))
                {
                    dal.DeleteCage(stc9.LNO, stc9.EQUIPID, _level);
                    return JsonHelper<Messaging<STC0009>>.EntityToJson(new Messaging<STC0009>("0", xml.ReadLandXml("0"), dal.GetDataList(stc9.LNO, stc9.EQUIPID, _level)));
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

