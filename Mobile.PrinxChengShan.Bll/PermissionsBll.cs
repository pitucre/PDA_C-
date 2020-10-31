using DataOperate.Net;
using Mobile.PrinxChengShan.Dal;
using Mobile.PrinxChengShan.Model;
using System;
using System.Web;
using Mobile.PrinxChengShan.Util;

namespace Mobile.PrinxChengShan.Bll
{
    /// <summary>
    /// 权限表
    /// </summary>
    public class PermissionsBll
    {
        private XmlHelper xml = null;
        private PermissionsDal dal = null;
        public PermissionsBll()
        {
            dal = new PermissionsDal();
            xml = new XmlHelper();

        }
        /// <summary>
        /// 请求入口
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string ProcessRequest(HttpContext context)
        {
            //try
            //{
            //    if (!context.Session["UserToken"].Equals(context.Request["Token"]))
            //    {
            //        return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("600", xml.ReadLandXml("600")));
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message));
            //}
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
                case "menu":
                    returnDate = GetPermissions(context);
                    break;
                case "second":
                    returnDate = GetPermissionsSecondLevel(context);
                    break;
                default:
                    returnDate = JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("404", xml.ReadLandXml("404")));
                    break;
            }
            return returnDate;

        }
        /// <summary>
        /// 获取菜单权限
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetPermissions(HttpContext context)
        {
            try
            {
                string loginName = context.Request["ln"] as string;
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", "", dal.GetPermissions(loginName)));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
        /// <summary>
        /// 二级菜单
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetPermissionsSecondLevel(HttpContext context)
        {
            try
            {
                string loginName = context.Request["ln"] as string;
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", "", dal.GetPermissionsSecondLevel(loginName)));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
    }
}