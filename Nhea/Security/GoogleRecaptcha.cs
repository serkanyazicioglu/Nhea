using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nhea.Helper;
using System.Web;
using System.Web.Script.Serialization;
using Nhea.Logging;

namespace Nhea.Security
{
    public static class GoogleRecaptcha
    {
        private const string RecaptchaUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}&remoteip={2}";

        public static bool Verify()
        {
            if (HttpContext.Current.Request.IsLocal)
            {
                return true;
            }

            if (String.IsNullOrEmpty(Nhea.Configuration.Settings.Web.GoogleRecaptchaSecretKey) || String.IsNullOrEmpty(Nhea.Configuration.Settings.Web.GoogleRecaptchaSiteKey))
            {
                return true;
            }

            var captchaFormData = HttpContext.Current.Request.Form["g-recaptcha-response"];

            if (captchaFormData != null && !String.IsNullOrEmpty(captchaFormData.ToString()))
            {
                return VerifyCore(captchaFormData.ToString());
            }

            return false;
        }

        public static bool Verify(string captchaResponse)
        {
            if (HttpContext.Current.Request.IsLocal)
            {
                return true;
            }

            if (String.IsNullOrEmpty(Nhea.Configuration.Settings.Web.GoogleRecaptchaSecretKey) || String.IsNullOrEmpty(Nhea.Configuration.Settings.Web.GoogleRecaptchaSiteKey))
            {
                return true;
            }

            return VerifyCore(captchaResponse);
        }

        private static bool VerifyCore(string captchaResponse)
        {
            if (!String.IsNullOrEmpty(captchaResponse))
            {
                string postUrl = String.Format(RecaptchaUrl, Nhea.Configuration.Settings.Web.GoogleRecaptchaSecretKey, captchaResponse, Nhea.Web.Helper.RequestHelper.GetCurrentIp());

                string result = Nhea.Web.Helper.RequestHelper.PostHttpRequest(postUrl, Nhea.Web.Helper.HttpRequestMethod.POST);

                if (!String.IsNullOrEmpty(result))
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    GoogleRecaptchaResponse googleRecaptchaResponse = serializer.Deserialize<GoogleRecaptchaResponse>(result);

                    if (googleRecaptchaResponse.success.ToLower() == "true")
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
