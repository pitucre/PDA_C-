<%@ WebHandler Language="C#" Class="FixedAssetInfo" %>

using System;
using System.Web;
using Mobile.PrinxChengShan.Bll;
public class FixedAssetInfo : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        //context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");
        context.Response.Write(new FixedAssetInfoBll().ProcessRequest(context));
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}