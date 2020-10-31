using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Mobile.PrinxChengShan.Dal;
using DataOperate.Net;
using Mobile.PrinxChengShan.Model;
using System.Security.Cryptography;
using Mobile.PrinxChengShan.Util;

namespace Mobile.PrinxChengShan.Bll
{
    /// <summary>
    /// 用户登录
    /// </summary>
    public class LoginBll
    {
        private XmlHelper xml = null;
        private LoginDal dal = null;
        string _lang = "CHN";
        public LoginBll()
        {
            dal = new LoginDal();
            xml = new XmlHelper();
        }
        /// <summary>
        /// 请求入口
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string ProcessRequest(HttpContext context)
        {           
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
                case "login":
                    returnDate = GetUserLogin(context);
                    break;
                case "update":
                    returnDate = AutoUpdate(context);
                    break;
                case "out":
                    returnDate = SignOut(context);
                    break;
                case "lang":
                    returnDate = GetLanguage();
                    break;
                case "fac":
                    returnDate = GetFactory();
                    break;
                case "upPas":
                    returnDate = UpdatePassword(context);
                    break;
                case "chk":
                    returnDate = CheckPassword(context);
                    break;
                default:
                    returnDate = JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("404", xml.ReadLandXml("404")));
                    break;
            }
            return returnDate;
        }
        private string CheckPassword(HttpContext context)
        {
            try
            {
                string loginName = context.Request["LOGINNAM"] as string;
                string password = context.Request["Password"] as string;
                MD5 md = new MD5CryptoServiceProvider();
                byte[] bytes = Encoding.Unicode.GetBytes(password);
                string md5Pass = BitConverter.ToString(md.ComputeHash(bytes)).Replace("-", "");
                LSFW_EMPLOYEE model = dal.GetUserLogin(loginName, md5Pass);
                if (!string.IsNullOrEmpty(model.LOGINNAME))
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
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message));
            }
        }
        private string UpdatePassword(HttpContext context)
        {
            try
            {
                string Id = context.Request["ID"] as string;
                string loginName = context.Request["LOGINNAM"] as string;
                string password = context.Request["Password"] as string;

                MD5 md = new MD5CryptoServiceProvider();
                byte[] bytes = Encoding.Unicode.GetBytes(password.Trim());
                string md5Pass = BitConverter.ToString(md.ComputeHash(bytes)).Replace("-", "");
                //
                if (dal.UpdatePassword(Id, loginName.Trim(), md5Pass))
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
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message));
            }
        }
        /// <summary>
        /// 获取工厂
        /// </summary>
        /// <returns></returns>
        private string GetFactory()
        {
            try
            {
                return JsonHelper<Messaging<TranslationConfig>>.EntityToJson(new Messaging<TranslationConfig>("0", "", new ComUtilDal().GetFactory(_lang)));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetLanguage()
        {
            try
            {
                return JsonHelper<Messaging<TranslationConfig>>.EntityToJson(new Messaging<TranslationConfig>("0", "", new LanguageDal().GetLanguage()));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message));
            }
        }
        /// <summary>
        /// 用户退出系统
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SignOut(HttpContext context)
        {
            try
            {
                context.Session.Remove(context.Request["Token"]);
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", xml.ReadLandXml("0")));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message));
            }
        }
        /// <summary>
        /// 获取自动更新数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string AutoUpdate(HttpContext context)
        {
            try
            {
                string configFile = HttpContext.Current.Server.MapPath("~/Version/VersionConfig.ini");
                ConfigIni conIni = new ConfigIni(configFile);
                return "{\"appid\":\"" + conIni.GetStringValue("appid") + "\",\"title\":\"" + conIni.GetStringValue("title") + "\",\"note\":\"" + conIni.GetStringValue("note") + "\",\"version\":\"" + conIni.GetStringValue("version") + "\",\"url\":\"" + conIni.GetStringValue("url") + "\"}";
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message));
            }
        }
        /// <summary>
        /// 用户登录验证
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetUserLogin(HttpContext context)
        {
            string wrong = string.Empty;
            string code = "0";
            try
            {
                string loginName = context.Request["ln"] as string;
                string password = context.Request["ps"] as string;
                string _Ver = string.Empty;
                try
                {
                    _Ver = context.Request["Ver"] as string;
                }
                catch { }
                string _Server = string.Empty;
                try
                {
                    _Server = context.Request["Server"] as string;
                }
                catch { }
                string _EquipmentNO = string.Empty;
                try
                {
                    _EquipmentNO = context.Request["EqNo"] as string;
                }
                catch { }
                string _DeviceName = string.Empty;
                try
                {
                    _DeviceName = context.Request["EqNm"] as string;
                }
                catch { }
                MD5 md = new MD5CryptoServiceProvider();
                byte[] bytes = Encoding.Unicode.GetBytes(password);
                string md5Pass = BitConverter.ToString(md.ComputeHash(bytes)).Replace("-", "");
                LSFW_EMPLOYEE model = dal.GetUserLogin(loginName, md5Pass);
                if (string.IsNullOrEmpty(model.LOGINNAME))
                {
                    wrong = "用户登录失败！";
                    code = "1";
                }
                else
                {
                    bytes = Encoding.Unicode.GetBytes(loginName);
                    string _token = new ComUtilDal().GenerateToKen();
                    model.Token = _token;
                    context.Session["UserToken"] = _token;
                    new T_MOBILE_LOGONLOGDal().Insert(model.LOGINNAME, model.NAME, model.DEPNAM, _Ver, _Server, _EquipmentNO, _DeviceName.ToUpper());
                }
                return JsonHelper<Messaging<LSFW_EMPLOYEE>>.EntityToJson(new Messaging<LSFW_EMPLOYEE>(code, wrong, model));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                wrong = ex.Message.ToString().Trim().Replace("\r\n", "");
                code = "500";
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>(code, wrong));
            }
        }
    }
}

