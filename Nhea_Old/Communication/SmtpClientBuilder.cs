using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using Nhea.Configuration.GenericConfigSection.Communication;

namespace Nhea.Communication
{
    public static class SmtpClientBuilder
    {
        public static SmtpClient Build(SmtpElement smtpElement)
        {
            SmtpClient smtpClient = new SmtpClient(smtpElement.Host);
            smtpClient.Credentials = new System.Net.NetworkCredential(smtpElement.UserName, smtpElement.Password);
            smtpClient.Port = smtpElement.Port;
            smtpClient.EnableSsl = smtpElement.EnableSsl;

            return smtpClient;
        }
    }
}
