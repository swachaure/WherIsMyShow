using Microsoft.AspNetCore.Hosting;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WhereIsMyShow.Models;

namespace WhereIsMyShow.Utils
{
    public class EmailSender
    {
        
        private readonly SendGridClient _client;
        //private readonly string apiKey = "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";
        private static readonly string MessageId = "X-Message-Id";

        public EmailSender()
        {
            _client = new SendGridClient(apiKey);
            
        }

        public EmailResponse Send(SendEmailViewModel contract)
        {

            var emailMessage = new SendGridMessage()
            {
                From = new EmailAddress("neilchaure2@gmail.com", "swapnil"),
                //From = new EmailAddress("", ""),
                Subject = contract.Subject,
                HtmlContent = contract.Contents,
            };
          


            emailMessage.AddTo(new EmailAddress(contract.To));


            return ProcessResponse(_client.SendEmailAsync(emailMessage).Result);
        }


        public EmailResponse SendEmail(SendEmailViewModel contract)
        {

            var emailMessage = new SendGridMessage()
            {
                From = new EmailAddress("neilchaure2@gmail.com", "swapnil"),
                //From = new EmailAddress("", ""),
                Subject = contract.Subject,
                HtmlContent = contract.Contents,
            };
            
            byte[] byteData = Encoding.ASCII.GetBytes(File.ReadAllText(@"D:\uni\FIT5032-S2-2018-master\project\WhereIsMyShow\WhereIsMyShow\booking.txt"));
            emailMessage.Attachments = new List<Attachment>
        {
            new Attachment
            {
                Content = Convert.ToBase64String(byteData),
                Filename = "booking.txt",
                Type = "txt/plain",
                Disposition = "attachment"
            }
        };


            emailMessage.AddTo(new EmailAddress(contract.To));


            return ProcessResponse(_client.SendEmailAsync(emailMessage).Result);
        }
        private static EmailResponse ProcessResponse(Response response)
        {
            if (response.StatusCode.Equals(System.Net.HttpStatusCode.Accepted)
                || response.StatusCode.Equals(System.Net.HttpStatusCode.OK))
            {
                return ToMailResponse(response);
            }

            //TODO check for null
            var errorResponse = response.Body.ReadAsStringAsync().Result;

            throw new EmailServiceException(response.StatusCode.ToString(), errorResponse);
        }

        private static EmailResponse ToMailResponse(Response response)
        {
            if (response == null)
                return null;

            var headers = (HttpHeaders)response.Headers;
            var messageId = headers.GetValues(MessageId).FirstOrDefault();
            return new EmailResponse()
            {
                UniqueMessageId = messageId,
                DateSent = DateTime.UtcNow,
            };
        }
    }
}
