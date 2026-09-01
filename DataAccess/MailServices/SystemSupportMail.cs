using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace DataAccess.MailServices
{
    class SystemSupportMail:MasterMailServer
    {
        public SystemSupportMail()
        {
            senderMail = "jeurbina1210@gmail.com";
            password="ju12108859";
            host = "smt.gmail.com";
            port = 587;
            ssl = true;
            initializeSmtpClient();

        }
    }
}
