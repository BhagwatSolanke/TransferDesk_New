using System;
using System.Linq;
using System.Net.Mail;

/// <summary>
/// Summary description for Email
/// </summary>
    public class Email
    {
        Boolean flag = false;
        String SMTPServer = "smtp.springer-sbm.com";//ConfigurationManager . AppSettings [ "SMTPServer" ] . ToString ( );

        public Boolean SendEmail(String emailTo, String emailFrom, String emailCC, String emailBCC, String subject, String body)
        {
            try
            {
                MailMessage Msg = new MailMessage();
                Msg.Subject = subject;
                Msg.Body = body;
                Msg.From = new MailAddress(emailFrom);

                try
                {
                    if (emailTo.Trim() != "")
                    {
                        String[] mailTo = emailTo.Split(';');
                        foreach (String to in mailTo)
                        {
                            if (Msg.To.Contains<Object>(to) == false)
                            {
                                Msg.To.Add(to);
                            }
                        }
                    }
                }
                catch { }

                try
                {
                    if (emailCC.Trim() != "")
                    {
                        String[] mailCC = emailCC.Split(';');
                        foreach (String cc in mailCC)
                        {
                            if (Msg.CC.Contains<Object>(cc) == false)
                            {
                                Msg.CC.Add(cc);
                            }
                        }
                    }
                }
                catch { }

                try
                {
                    if (emailBCC.Trim() != "")
                    {
                        String[] mailBCC = emailBCC.Split(';');
                        foreach (String bcc in mailBCC)
                        {
                            Msg.Bcc.Add(bcc);
                        }
                    }
                }
                catch { }

                Msg.Priority = MailPriority.Normal;
                Msg.IsBodyHtml = true;
                SmtpClient client = new SmtpClient(SMTPServer, 25);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(Msg);
                flag = true;
            }
            catch (SmtpException smtpEx)
            {
                flag = false;
            }
            finally { }

            return flag;
        }
    }