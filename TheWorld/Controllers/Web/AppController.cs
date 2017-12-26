using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private ILogger<AppController> _logger;

        public AppController(IMailService mailService, IConfigurationRoot config, IWorldRepository repository, ILogger<AppController> logger)
        {
            MailService = mailService;
            _config = config;
            _repository = repository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get trips in the Index page : {e.Message}");
                return Redirect("/error");
            }
        }

        [Authorize]
        public IActionResult Trips()
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get trips in the Trips page : {e.Message}");
                return Redirect("/error");
            }
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
