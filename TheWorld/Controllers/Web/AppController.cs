using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TheWorld.Models;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Web
{
    public class AppController : Controller
    {
        private IMailService MailService;
        private IConfigurationRoot _config;
        private IWorldRepository _repository;

        public AppController(IMailService mailService, IConfigurationRoot config, IWorldRepository repository)
        {
            MailService = mailService;
            _config = config;
            _repository = repository;

        }

        public IActionResult Index()
        {
            var data = _repository.GetAllTrips();
            return View(data);
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (model.Email.Contains("aol.com"))
            {
                ModelState.AddModelError("Email", "We don't support AOL addresses");
            }
            if (ModelState.IsValid)
            {
                MailService.SendMail(_config["MailSettings:ToAddress"], model.Email, "From TheWorld", model.Message);

                ModelState.Clear();
                ViewBag.UserMessage = "Message Sent!";
            }
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
