using System;
using System.Net.Mail;
using System.Security.Authentication;
using MailKit.Security;
using Nhea.Configuration.GenericConfigSection.Communication;

namespace Nhea.CoreCommunicationService
{
    public static class SmtpClientBuilder
    {
        public static SmtpClient Build(SmtpElement smtpElement)
        {
            return new(smtpElement.Host)
            {
                Credentials = new System.Net.NetworkCredential(smtpElement.UserName, smtpElement.Password),
                Port = smtpElement.Port,
                EnableSsl = smtpElement.EnableSsl
            };
        }

        public static MailKit.Net.Smtp.SmtpClient BuildForMailKit(SmtpElement smtpElement)
        {
            var smtpClient = new MailKit.Net.Smtp.SmtpClient();

            if (smtpElement.EnableSsl)
            {
                smtpClient.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13;

                if (Environment.GetEnvironmentVariable("MAILQUEUE_IGNORE_SSL_VALIDATION") == "true")
                {
                    smtpClient.CheckCertificateRevocation = false;
                    smtpClient.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
                }
            }

            smtpClient.Connect(smtpElement.Host, smtpElement.Port, SecureSocketOptions.Auto);
            smtpClient.Authenticate(smtpElement.UserName, smtpElement.Password);

            return smtpClient;
        }
    }
}
