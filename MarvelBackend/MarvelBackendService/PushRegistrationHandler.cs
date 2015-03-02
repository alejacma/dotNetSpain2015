using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Notifications;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace MarvelBackendService
{
    public class PushRegistrationHandler : INotificationHandler
    {
        public Task Register(ApiServices services, HttpRequestContext context,
            NotificationRegistration registration)
        {
            try
            {
                // Perform a check here for user ID tags, which are not allowed.
                if (!ValidateTags(registration))
                {
                    throw new InvalidOperationException(
                        "You cannot supply a tag that is a user ID.");
                }

                // Get the logged-in user.
                var currentUser = context.Principal as ServiceUser;

                // Add a new tag that is the user ID.
                registration.Tags.Add(currentUser.Id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return Task.FromResult(true);
        }

        private bool ValidateTags(NotificationRegistration registration)
        {
            // Create a regex to search for disallowed tags.
            System.Text.RegularExpressions.Regex searchTerm =
            new System.Text.RegularExpressions.Regex(@"facebook:|google:|twitter:|microsoftaccount:",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            foreach (string tag in registration.Tags)
            {
                if (searchTerm.IsMatch(tag))
                {
                    return false;
                }
            }
            return true;
        }

        public Task Unregister(ApiServices services, HttpRequestContext context,
            string deviceId)
        {
            // This is where you can hook into registration deletion.
            return Task.FromResult(true);
        }
    }
}