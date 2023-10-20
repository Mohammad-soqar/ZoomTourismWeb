using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Configuration;
using Twilio;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using ZoomTourism.DataAccess.Repository.IRepository;

namespace ZoomTourismWeb.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class SmsController : Controller
    {
        private readonly IOptions<TwilioSettings> _twilioConfig;
        private readonly TwilioRestClient _twilioClient;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _HostEnvironment;
        private readonly UserManager<IdentityUser> _userManager;

        public SmsController(TwilioRestClient twilioClient, IOptions<TwilioSettings> twilioConfig, IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment,
           UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _HostEnvironment = hostEnvironment;
            _userManager = userManager;
            _twilioClient = twilioClient;
            _twilioConfig = twilioConfig;
        }
        public IActionResult SendSms(string reviewLink, string phoneNumber)
        {


            try
            {
                var twilioConfig = _twilioConfig.Value;

                TwilioClient.Init(twilioConfig.AccountSid, twilioConfig.AuthToken);

                string messageBody = "Thank you for choosing our service! Please leave a review: " + reviewLink;

                // Attempt to send the SMS using Twilio
                var message = MessageResource.Create(
                    body: messageBody,
                    from: new Twilio.Types.PhoneNumber("+447380279299"),
                    to: new Twilio.Types.PhoneNumber(phoneNumber)
                );

                if (message.ErrorCode == null)
                {
                    return RedirectToAction("SuccessView");
                }
                else
                {
                    return RedirectToAction("ErrorView");
                }
            }
            catch (Exception ex)
            {
             
                return RedirectToAction("ErrorView");
            }
        }
    }
}
