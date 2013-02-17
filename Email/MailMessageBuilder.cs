#region Licence
/*
 * The MIT License
 *
 * Copyright (c) 2008-2012, Andrew Gray
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
#endregion

using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using euthenias_delight;

namespace com.phoenixconsulting.common.mail {
    public class MailMessageBuilder {
        //
        // Email to Client
        //
        public static void SendEmailToClient(string toEmail, string body, MemoryStream stream) {
            MailSender.SendMail(CreateMailMessageObject(false,
                                                        "andrew.paul.gray@gmail.com",
                                                        toEmail,
                                                        "Phoenix Consulting Invoice",
                                                        body,
                                                        stream));
        }

        //
        // Construct MailMessage object
        //
        private static MailMessage CreateMailMessageObject(bool isHtml, string from, string to, string subject, string body, MemoryStream stream) {
            MailMessage oMsg = new MailMessage();

            ContentType contentType = new ContentType { 
                MediaType = MediaTypeNames.Application.Octet, 
                Name = Constants.INVOICE_PDF_FILENAME
            };

            oMsg.IsBodyHtml = isHtml;
            oMsg.From = new MailAddress(from);
            oMsg.To.Add(to);
            oMsg.Bcc.Add(from);
            oMsg.Subject = subject;
            oMsg.Body = body;
            oMsg.Attachments.Add(new Attachment(stream, contentType));

            return oMsg;
        }
    }
}