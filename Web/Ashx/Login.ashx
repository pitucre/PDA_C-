<%@ WebHandler Language="C#" Class="Login" %>

using System.Text;
using System.Web;
using System.Web.SessionState;
using Mobile.PrinxChengShan.Bll;

public class Login : IHttpHandler, IReadOnlySessionState
{
    /// <summary>
    /// web请求入口
    /// </summary>
    /// <param name="context"></param>
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentEncoding = Encoding.UTF8;
        context.Response.Write(new LoginBll().ProcessRequest(context));
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}