using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Database;
using Android.Provider;
using Android.Runtime;
using Android.Support.V4.App;
using Plugin.CurrentActivity;
using Cognizant.Hackathon.Shared.Mobile.Models.Models;
using LinC.Droid.Helper;
using LinC.Droid.Platforms;
using LinC.Platforms;

[assembly: Xamarin.Forms.Dependency(typeof(ContactServiceDroid))]
namespace LinC.Droid.Platforms
{
    public class ContactServiceDroid : IContactsService
    {
        static TaskCompletionSource<bool> contactPermissionTcs;
        public const int RequestContacts = 1239;
        static string[] PermissionsContact = {
            Manifest.Permission.ReadContacts
        };
        private static string[] PROJECTION = new String[] {
    ContactsContract.CommonDataKinds.Email.InterfaceConsts.ContactId,
    ContactsContract.Contacts.InterfaceConsts.DisplayName,
    ContactsContract.CommonDataKinds.Email.InterfaceConsts.Data
    };


        async void RequestContactsPermissions()
        { 
             ActivityCompat.RequestPermissions(CrossCurrentActivity.Current.Activity, PermissionsContact, RequestContacts);
          
        }
        public static void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            if (requestCode == ContactServiceDroid.RequestContacts)
            {
                if (PermissionUtil.VerifyPermissions(grantResults))
                {
                    contactPermissionTcs.TrySetResult(true);
                }
                else
                {
                    contactPermissionTcs.TrySetResult(false);
                }

            }
        }

        public async Task<bool> RequestPermissionAsync()
        {
            contactPermissionTcs = new TaskCompletionSource<bool>();
            if (Android.Support.V4.Content.ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.Activity, Manifest.Permission.ReadContacts) != (int)Permission.Granted
                || Android.Support.V4.Content.ContextCompat.CheckSelfPermission(CrossCurrentActivity.Current.Activity, Manifest.Permission.WriteContacts) != (int)Permission.Granted)
            {
                RequestContactsPermissions();
            }
            else
            {
                contactPermissionTcs.TrySetResult(true);
            }

            return await contactPermissionTcs.Task;
        }

        public async Task<IList<Contact>> RetrieveContactsAsync(CancellationToken? cancelToken = null)
        {
            try
            {
                if (!cancelToken.HasValue)
                    cancelToken = CancellationToken.None;
                var taskCompletionSource = new TaskCompletionSource<IList<Contact>>();
                cancelToken.Value.Register(() =>
                {
                    taskCompletionSource.TrySetCanceled();
                });

                var task = LoadContactsAsync();
                var completedTask = await Task.WhenAny(task, taskCompletionSource.Task);
                return await completedTask;
            }

            catch(Exception)
            {
                return null;
            }
        }
       

        async Task<IList<Contact>> LoadContactsAsync()
        {
            try
            {
                IList<Contact> contacts = new List<Contact>();
                var hasPermission = await RequestPermissionAsync();
                if (hasPermission)
                {
                    var ctx = Application.Context;
                    await Task.Run(() =>
                    {
                        var cursor = ctx.ApplicationContext.ContentResolver.Query(ContactsContract.CommonDataKinds.Email.ContentUri, PROJECTION, null, null, $"{ContactsContract.Contacts.InterfaceConsts.DisplayName} ASC");
                       
                        if (cursor.Count > 0)
                        {
                                 contacts = CreateContact(cursor, ctx);
                        }
                    });

                }
                return contacts;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }


        List<Contact> CreateContact(ICursor cursor, Android.Content.Context ctx)
        {
            List<Contact> contacts = new List<Contact>();
            if (cursor != null)
            {
                try
                {
                     int contactIdIndex = cursor.GetColumnIndex(ContactsContract.CommonDataKinds.Email.InterfaceConsts.ContactId);
                     int displayNameIndex = cursor.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.DisplayName);
                    int emailIndex = cursor.GetColumnIndex(ContactsContract.CommonDataKinds.Email.InterfaceConsts.Data);
                    long contactId;
                    string displayName;
                    string[] address;
                    while (cursor.MoveToNext())
                    {
                        contactId = cursor.GetLong(contactIdIndex);
                        displayName = cursor.GetString(displayNameIndex);
                        address =  new[] { cursor.GetString(emailIndex) };

                        var contact = new Contact
                        {
                            Name = displayName,
                            Emails = address,
                        };
                        if (!string.IsNullOrWhiteSpace(contact.Name) && (contact.Emails.Count() > 0))
                        {
                            contacts.Add(contact);
                        }

                    }
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                  
                finally
                {
                    cursor.Close();
                   
                }
            }
            return contacts;
        }

        public async Task<bool> CheckContactPermissionStatus()
        {
            var hasPermission = await RequestPermissionAsync();
            return hasPermission;
        }

    }
}

