using DataOperate.Net;
using Mobile.PrinxChengShan.Dal;
using Mobile.PrinxChengShan.Model;
using Mobile.PrinxChengShan.Util;
using System;
using System.Data;
using System.Web;

namespace Mobile.PrinxChengShan.Bll
{
    public class EPScanBll
    {
        private XmlHelper xml = null;
        private EPScanDal dal = null;
        public EPScanBll()
        {
            dal = new EPScanDal();
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
                case "chkBar":
                    returnDate = Get_Scan_Barcode_State(context);
                    break;
                case "chkwait":
                    returnDate = Is_Production_info_Waiting(context);
                    break;
                case "add":
                    returnDate = Insert(context);
                    break;
                case "sel":
                    returnDate = GetByModel(context);
                    break;
                case "d":
                    returnDate = Del(context);
                    break;
                default:
                    returnDate = JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("404", xml.ReadLandXml("404")));
                    break;
            }
            return returnDate;
        }
        private string GetByModel(HttpContext context)
        {
            try
            {
                string BARCODE = context.Request["BARCODE"];
                return JsonHelper<Messaging<LTA1002>>.EntityToJson(new Messaging<LTA1002>("0", xml.ReadLandXml("0"), dal.GetByModel(BARCODE.Trim())));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
        private string Del(HttpContext context)
        {
            try
            {
                string ID = context.Request["ID"].Trim();
                if (dal.Delete(ID))
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
        private string Insert(HttpContext context)
        {
            try
            {
                LTA1002 model = new LTA1002();              
                model.MCHID = context.Request["MCHID"].Trim();
                model.LR = context.Request["LR"].Trim();
                model.BARCODE = context.Request["BARCODE"].Trim();
                model.TYRENO = context.Request["TYRENO"].Trim();
                model.UPNAME = context.Request["LOGINNAME"].Trim();
                string username = context.Request["ENAM"].Trim();
                string FAC = context.Request["FAC"].Trim();
                Shift shift = new Shift();
                DataTable dtComUtil = new ComUtilDal().GetShift();
                shift.WDATE = dtComUtil.Rows[0]["WDATE"].ToString();
                shift.SHT = dtComUtil.Rows[0]["SHT"].ToString();
                shift.NSHT = dtComUtil.Rows[0]["NSHT"].ToString();
                shift.BAN = dtComUtil.Rows[0]["BAN"].ToString();
                shift.STIME = dtComUtil.Rows[0]["STIME"].ToString();
                shift.SHTNAM = dtComUtil.Rows[0]["SHTNAM"].ToString();
                shift.NSHTNAM = dtComUtil.Rows[0]["NSHTNAM"].ToString();
                dal.Update_Scan_Barcode_Cure_PDA(new string[] { model.MCHID, model.LR, model.BARCODE, model.UPNAME, username, model.TYRENO }, shift, FAC);
                if (dal.InsertOrUpdate(model))
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
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
        private string Get_Scan_Barcode_State(HttpContext context)
        {
            try
            {
                string barcode = context.Request["BARCODE"].Trim();
                string mchid = context.Request["MCHID"].Trim();
                string lr = context.Request["LR"].Trim();
                string str = dal.Get_Scan_Barcode_State(barcode, mchid, lr);
                if (string.IsNullOrEmpty(str))
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", xml.ReadLandXml("0")));
                }
                else
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("1", str));
                }
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
        private string Is_Production_info_Waiting(HttpContext context)
        {
            try
            {
                string MCHID = context.Request["MCHID"].Trim();
                string MCHNAME = string.Empty;
                string LR = context.Request["LR"].Trim();
                string TYRENO = context.Request["TYRENO"].Trim();
                string FAC = context.Request["FAC"].Trim();
                string str = dal.Is_Production_info_Waiting(new string[] { MCHID, MCHNAME, LR, TYRENO });
                if (string.IsNullOrEmpty(str))
                {
                    string checkMess = dal.GetCheck_MESNACMES_Tyre(FAC, MCHID, TYRENO, LR);
                    if (checkMess == "0")
                    {
                    int mes = dal.Is_Production_info_Existed(new string[] { MCHID, MCHNAME, LR, TYRENO });
                        if (mes == 0)
                        {
                            return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", xml.ReadLandXml("0")));
                        }
                        else
                        {
                            return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("1", "镂空条码已经硫化。"));
                        }
                    }
                    else
                    {
                        return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("1", checkMess));
                    }
                }
                else
                {
                    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("1", str));
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
