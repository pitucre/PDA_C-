using DataOperate.Net;
using Mobile.PrinxChengShan.Dal;
using Mobile.PrinxChengShan.Model;
using Mobile.PrinxChengShan.Util;
using System;
using System.Data;
using System.Web;

namespace Mobile.PrinxChengShan.Bll
{
    public class InFacStorageBll
    {
        private XmlHelper xml = null;
        private InFacStorageDal dal = null;
        public InFacStorageBll()
        {
            dal = new InFacStorageDal();
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
                    returnDate = GetWarehouseReceiptList(context);
                    break;
                case "by":
                    returnDate = GetByModel(context);
                    break;
                case "subls":
                    returnDate = GetSubList(context);
                    break;
                case "nb":
                    returnDate = NewInsertBill(context);
                    break;
                case "del":
                    returnDate = DelOddNumbers(context);
                    break;
                case "de":
                    returnDate = DelEmpty(context);
                    break;
                case "sel":
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
        private string Create(HttpContext context)
        {
            try
            {
                string _putstorNUM = context.Request["PN"].Trim();
                string _LOGINNAM = context.Request["LOGINNAM"].Trim();                            
                if (dal.Create(_putstorNUM, _LOGINNAM))
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
                string _putstornum = context.Request["PN"] as string;
                string _barcode = context.Request["BARCODE"] as string;
                string _reservoirarea = context.Request["Reservoirarea"] as string;
                string _warehouselocation = context.Request["Warehouselocation"] as string;
                string _enam = context.Request["ENAM"] as string;
                string _loginnam = context.Request["LOGINNAM"] as string;
                //-------------------------------------------------------------------------------------
                string _putstorNUM = _putstornum.Trim();
                string _BARCODE = _barcode.Trim();
                string _Reservoirarea = _reservoirarea.Trim();
                string _Warehouselocation = _warehouselocation.Trim();
                string _ENAM = _enam.Trim();
                string _LOGINNAM = _loginnam.Trim();
                WIP0010 model = new WIP0010Dal().GetEntityById(_BARCODE);
                if (model.LOCKYN == "Y")
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "该条码信息的轮胎在库存中已经锁定！"));
                }
                if (model.STA != "1")
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "该条码信息的轮胎在库存中处于不良、保留或者报废状态！"));
                }
                if (string.IsNullOrEmpty(model.BARCODE))
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "该条码信息的轮胎不在车间库存中！"));
                }
                string _itnbr = dal.VerifySpecificationUniqueness(_putstorNUM);
                if (!string.IsNullOrEmpty(_itnbr))
                {
                    if (_itnbr != model.ITNBR)
                    {
                        return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "同一个入库单不能装两种规格的轮胎！"));
                    }
                }
                if (!dal.ExistCheck(_BARCODE, _putstorNUM))
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "该条码信息的轮胎已经扫描过！"));
                }
                if (!dal.QueryCheck(_BARCODE))
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "该条码信息的轮胎已经被别人扫描过！"));
                }
                STC0022 stc0022 = dal.GetByModel(_putstorNUM);
                QMB0101 qmb0101 = new QMB0101Dal().GetByModel(_BARCODE);
                if ("1" == stc0022.TYPE)
                {
                    if ("1" != qmb0101.QCSTATE)
                    {
                        return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "合格品入库单不能扫描入库废次品轮胎！"));
                    }
                }
                else
                {
                    if ("1" == qmb0101.QCSTATE)
                    {
                        return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("2", "废次品入库单不能扫描入库合格品轮胎！"));
                    }
                }
                STC0023 stc0023 = new STC0023
                {
                    BARCODE = _BARCODE.Trim(),
                    putstorNUM = _putstorNUM.Trim(),
                    Grade = qmb0101.QCSTATE,
                    Reservoirarea = _Reservoirarea.Trim(),
                    Warehouselocation = _Warehouselocation.Trim(),
                    ENAM = _ENAM.Trim(),
                    LOGINNAM = _LOGINNAM.Trim(),
                    DBRES = qmb0101.DBRES
                };
                if (dal.ScanningInsert(stc0023))
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

        private string DelEmpty(HttpContext context)
        {
            try
            {
                string putstorNUM = context.Request["PN"].Trim();
                if (dal.DelEmpty(putstorNUM))
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
                string putstorNUM = context.Request["PN"].Trim();
                if (dal.DelOddNumbers(putstorNUM))
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
        private string NewInsertBill(HttpContext context)
        {
            try
            {
                string _DIV = context.Request["DIV"].Trim();
                string _fac = context.Request["FAC"].Trim();
                string _ENAM = context.Request["ENAM"].Trim();
                string _LOGINNAM = context.Request["LOGINNAM"].Trim();
                DataTable dtComUtil = new ComUtilDal().GetShift();
                STC0022 model = new STC0022
                {
                    FAC = _fac,
                    putstorNUM = dal.NewCreateBill(_fac),
                    DIV = _DIV,
                    TYPE = "1",
                    notetaker = string.Empty,
                    WSHT = dtComUtil.Rows[0]["SHT"].ToString(),
                    WBAN = dtComUtil.Rows[0]["BAN"].ToString(),
                    ENAM = _ENAM,
                    LOGINNAM = _LOGINNAM
                };
                if (dal.NewInsertBill(model))
                {
                    return JsonHelper<Messaging<STC0022>>.EntityToJson(new Messaging<STC0022>("0", xml.ReadLandXml("0"), dal.GetWarehouseReceiptList(model.TYPE, model.DIV)));
                }
                else
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("1", xml.ReadLandXml("0")));
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
        private string GetSubList(HttpContext context)
        {
            try
            {
                string putstorNUM = context.Request["PN"].Trim();
                return JsonHelper<Messaging<LTA0001>>.EntityToJson(new Messaging<LTA0001>("0", xml.ReadLandXml("0"), dal.GetSubList(putstorNUM)));
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
        private string GetWarehouseReceiptList(HttpContext context)
        {
            try
            {
                string TYPE = "1";
                string DIV = context.Request["DIV"].Trim();
                return JsonHelper<Messaging<LTA0001>>.EntityToJson(new Messaging<LTA0001>("0", xml.ReadLandXml("0"), dal.GetWarehouseReceiptList(TYPE, DIV)));
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
                string putstorNUM = context.Request["PN"].Trim();
                return JsonHelper<Messaging<STC0022>>.EntityToJson(new Messaging<STC0022>("0", xml.ReadLandXml("0"), dal.GetByModel(putstorNUM)));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
    }
}
