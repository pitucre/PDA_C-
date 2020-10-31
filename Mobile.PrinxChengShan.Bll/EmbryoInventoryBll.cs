﻿using DataOperate.Net;
using Mobile.PrinxChengShan.Dal;
using Mobile.PrinxChengShan.Model;
using Mobile.PrinxChengShan.Util;
using System;
using System.Web;

namespace Mobile.PrinxChengShan.Bll
{
    /// <summary>
    /// 胎胚盘点
    /// </summary>
    public class EmbryoInventoryBll
    {
        private XmlHelper xml = null;
        private STA0005Dal dal = null;
        public EmbryoInventoryBll()
        {
            dal = new STA0005Dal();
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
                case "sel":
                    returnDate = GetInventoryData(context);
                    break;              
                default:
                    returnDate = JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("404", xml.ReadLandXml("404")));
                    break;
            }
            return returnDate;
        }       
        /// <summary>
        /// 获取胎胚盘库单据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string GetInventoryData(HttpContext context)
        {
            try
            {
                XmlHelper basic = new XmlHelper();
                basic.FilePath = context.Server.MapPath("~/BasicInfo/BasicInfomation.xml");
              
                //胎胚盘点代码
                string DIV = basic.ReadBasicXml("12");
                //开盘状态代码
                string STA = basic.ReadBasicXml("6");
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("0", "", dal.GetInventoryData(DIV, STA)));
            }
            catch (Exception ex)
            {
                SystemErrorPlug.ErrorRecord(ex.ToString());
                return JsonHelper<Messaging<string>>.EntityToJson(new Messaging<string>("500", ex.Message.ToString().Trim().Replace("\r\n", "")));
            }
        }
    }
}

