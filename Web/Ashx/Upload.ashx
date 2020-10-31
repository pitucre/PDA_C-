<%@ WebHandler Language="C#" Class="Upload" %>

using System.Web;
using System.Web.SessionState;
using Mobile.PrinxChengShan.Bll;

public class Upload : IHttpHandler, IReadOnlySessionState
{
    public Upload()
    {

    }

    public void ProcessRequest(HttpContext context)
    {
        context.Response.Write(new UpLoadFileBll().ProcessRequest(context));
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}