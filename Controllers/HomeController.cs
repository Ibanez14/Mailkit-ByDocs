using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MailKitMVC.Models;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using NETCore.MailKit.Core;

namespace MailKitMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        /// <summary>
        /// This is Default usage of Mailkit assembly
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("shamilms.edu.az@yandex.ru"));
            message.To.Add(new MailboxAddress("shamilms@code.edu.az"));
            message.Subject = "Testing Email";
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = "<h1> Testing Net Core</h1>"
            };

            // Doesn't work with smtp.google.com

            // Яндекс не принимает обычный пароль от почты для сторонних приложений.
            // Необходимо создать отдельный пароль для приложения.
            // Зайти в web - интерфейс.Настройки->Безопасность->Включите и создайте пароли приложений. 

            using (var smtp = new SmtpClient())
            {
                // this configurations are taken from Yandex site
                smtp.Connect("smtp.yandex.com", port: 465, useSsl: true); 
                // this password is created in Yandex Settings
                smtp.Authenticate("shamilms.edu.az@yandex.ru", "mdpmuwomjpeumzbs");
                smtp.Send(message);
                smtp.Disconnect(true);
            }

            return View();
        }



        /// <summary>
        /// This doesnt work
        /// </summary>
        /// <param name="emailService"></param>
        /// <returns></returns>
        public async Task<IActionResult> SendViaOtherAssembly([FromServices] IEmailService emailService)
        {
            await
            emailService.SendAsync(mailTo: "zoom7oom@gmail.com",
                                  subject: "Creedence",
                                  message: "<h3> Left a good job in the city </h3>",
                                  isHtml: true);

            return Ok();
        }

            
    }
}
