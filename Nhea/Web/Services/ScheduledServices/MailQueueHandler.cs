namespace Nhea.Web.Services.ScheduledServices
{
    //public class MailQueueHandler : IHttpHandler
    //{
    //    public bool IsReusable
    //    {
    //        get { return true; }
    //    }

    //    public void ProcessRequest(HttpContext context)
    //    {
    //        try
    //        {
    //            if(context.Request.QueryString["i"] != null)
    //            {
    //                if (Nhea.Security.QueryStringSecurity.CheckHashedQueryString(context.Request.RawUrl))
    //                {
    //                    Guid mailId = context.Request.QueryString.Get("i").Convert<Guid>(Guid.Empty);

    //                    if (mailId != Guid.Empty)
    //                    {
    //                        MailQueueHistory.SetMailStatus(mailId, MailStatus.Opened);
    //                    }
    //                }
    //            }
    //        }
    //        catch
    //        { 
    //        }
    //    }
    //}
}
