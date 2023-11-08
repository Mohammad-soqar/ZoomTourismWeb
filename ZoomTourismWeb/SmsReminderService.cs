using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Hangfire;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;

namespace ZoomTourismWeb
{
    public class SmsReminderService
    {
        private readonly IConfiguration _configuration;

        public SmsReminderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendReminderToDriver(string driverPhoneNumber, DateTime sendTime)
        {
            var twilioConfig = _configuration.GetSection("Twilio").Get<TwilioSettings>();

            // Calculate the reminder time (e.g., 12 hours before the trip)
            var reminderTime = sendTime.AddMinutes(+4);

            if (reminderTime > DateTime.Now)
            {
                // Compose the message
                string messageBody = "Reminder: Your trip is starting in 12 hours. Please be prepared.";

                try
                {
                    // Initialize Twilio client
                    TwilioClient.Init(twilioConfig.AccountSid, twilioConfig.AuthToken);

                    // Attempt to send the SMS using Twilio
                    var message = MessageResource.Create(
                        body: messageBody,
                        from: new PhoneNumber("+447380279299"),  // Replace with your Twilio sender phone number
                        to: new PhoneNumber(driverPhoneNumber)
                    );

                    if (message.ErrorCode == null)
                    {
                        // SMS sent successfully
                    }
                    else
                    {
                        // Handle SMS sending failure (log or retry)
                    }
                }
                catch (Exception ex)
                {
                    // Handle other exceptions, e.g., network issues
                }
            }
        }
    }
}
