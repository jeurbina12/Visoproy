using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;


namespace DataAccess.MailServices
{
    public abstract class MasterMailServer
    {
        //Atributos
        private SmtpClient smtpClient;
        protected string senderMail { get; set; }
        protected string password { get; set; }
        protected string host { get; set; }
        protected int port { get; set; }
        protected bool ssl { get; set; }

        //Inicializar propiedades del cliente SMTP
        protected void initializeSmtpClient()//Simple Mail Transfer Protocol
        {
            smtpClient = new SmtpClient();
            smtpClient.Credentials = new NetworkCredential(senderMail, password);
            smtpClient.Host = host;
            smtpClient.Port = port;
            smtpClient.EnableSsl = ssl;
        }

        public void sendMail(string subject, string body, List<string> recipientMail)
        {
            var mailMessage = new MailMessage();
            try
            {
                mailMessage.From = new MailAddress(senderMail);
                foreach (string mail in recipientMail)
                {
                    mailMessage.To.Add(mail);
                }
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.Priority = MailPriority.Normal;
                smtpClient.Send(mailMessage);//Enviar mensaje
            }
            catch { }
            finally
            {
                mailMessage.Dispose();
                //smtpClient.Dispose();
            }
        }

        //class SystemSupportMail : MasterMailServer
        //{
        //    public SystemSupportMail()
        //    {
        //        senderMail = "soporteSystemTiendita@gmail.com";
        //        password = "admin4321";
        //        host = "smtp.gmail.com";
        //        port = 587;
        //        ssl = true;
        //        initializeSmtpClient();
        //    }
        //}


    }
}
