using System;
using System.Activities;
using System.Net;


namespace OpenRPA.Custom.Activities
{
    using System.Net.Mail;
    public class SendMailSMTP : CodeActivity
    {
        [RequiredArgument]
        public InArgument<string> SMTPServer { get; set; }
        [RequiredArgument]
        public InArgument<string> PortSMTP { get; set; }
        [RequiredArgument]
        public InArgument<string> ToBox { get; set; }
        [RequiredArgument]
        public InArgument<string> FromBox { get; set; }
        [RequiredArgument]
        public InArgument<string> Subject { get; set; }
        [RequiredArgument]
        public InArgument<string> Body { get; set; }
        [RequiredArgument]
        public InArgument<string> FromName { get; set; }
        [RequiredArgument]
        public InArgument<string> MailPassword { get; set; }      

        public InArgument<string[]> Attachments { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            try
            {
                string[] Addresses = ToBox.Get(context).Split(';');
                MailAddress From = new MailAddress(FromBox.Get(context), FromName.Get(context));
                MailMessage Message = new MailMessage();

                foreach (string addr in Addresses)
                {
                    Message.To.Add(addr);
                }

                Message.Subject = Subject.Get(context);
                Message.Body = Body.Get(context);
                Message.IsBodyHtml = true;
                Message.From = From;

                int SMTP = Convert.ToInt32(SMTPServer.Get(context));
                SmtpClient Client = new SmtpClient(SMTPServer.Get(context), SMTP);
                Client.Credentials = new NetworkCredential(FromBox.Get(context), MailPassword.Get(context));

                string[] AttachedFiles = Attachments.Get(context);
                if (AttachedFiles.Length > 0)
                {
                    foreach (string file in AttachedFiles)
                    {
                        if (!string.IsNullOrEmpty(file))
                        {
                            Message.Attachments.Add(new Attachment(file));
                        }
                    }
                }

                Client.Send(Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
