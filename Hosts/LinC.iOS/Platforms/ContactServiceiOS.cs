using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Contacts;
using Foundation;
using Cognizant.Hackathon.Shared.Mobile.Models.Models;
using LinC.iOS.Platforms;
using LinC.Platforms;

[assembly: Xamarin.Forms.Dependency(typeof(ContactServiceiOS))]
namespace LinC.iOS.Platforms
{
    public class ContactServiceiOS : NSObject, IContactsService
    {
        bool requestStop = false;
     
        public async Task<IList<Contact>> RetrieveContactsAsync(CancellationToken? cancelToken = null)
        {
            try
            {
                requestStop = false;

                if (!cancelToken.HasValue)
                    cancelToken = CancellationToken.None;


                var taskCompletionSource = new TaskCompletionSource<IList<Contact>>();

                // Registering a lambda into the cancellationToken
                cancelToken.Value.Register(() =>
                {
                    requestStop = true;
                    taskCompletionSource.TrySetCanceled();
                });


                var task = LoadContactsAsync();
                var completedTask = await Task.WhenAny(task, taskCompletionSource.Task);
                return await completedTask;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }
        public async Task<bool> RequestPermissionAsync()
        {
            var status = CNContactStore.GetAuthorizationStatus(CNEntityType.Contacts);

            Tuple<bool, NSError> authotization = new Tuple<bool, NSError>(status == CNAuthorizationStatus.Authorized, null);

            if (status == CNAuthorizationStatus.NotDetermined)
            {
                using (var store = new CNContactStore())
                {
                    authotization = await store.RequestAccessAsync(CNEntityType.Contacts);
                }
            }
            return authotization.Item1;

        }

        public async Task<IList<Contact>> LoadContactsAsync()
        {
            try
            {
                IList<Contact> contacts = new List<Contact>();
                var hasPermission = await RequestPermissionAsync();
                if (hasPermission)
                {

                    NSError error = null;
                    var keysToFetch = new[] { CNContactKey.GivenName, CNContactKey.FamilyName, CNContactKey.EmailAddresses };

                    var request = new CNContactFetchRequest(keysToFetch: keysToFetch);
                    request.SortOrder = CNContactSortOrder.GivenName;

                    using (var store = new CNContactStore())
                    {
                        var result = store.EnumerateContacts(request, out error, new CNContactStoreListContactsHandler((CNContact c, ref bool stop) =>
                        {
                            var contact = new Contact()
                            {
                                Name = string.IsNullOrEmpty(c.FamilyName) ? c.GivenName : $"{c.GivenName} {c.FamilyName}",
                                Emails = c.EmailAddresses?.Select(p => p?.Value?.ToString()).ToArray(),

                            };

                            if (!string.IsNullOrWhiteSpace(contact.Name) && (contact.Emails.Count() > 0))
                            {
                                contacts.Add(contact);
                            }
                            stop = requestStop;
                        }));
                    }
                }

                return contacts;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<bool> CheckContactPermissionStatus()
        {
            var hasPermission = await RequestPermissionAsync();
            return hasPermission;
        }
    }
}
