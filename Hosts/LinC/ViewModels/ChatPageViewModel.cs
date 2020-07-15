using IBM.Watson.Assistant.v1.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Cognizant.Hackathon.Shared.Mobile.Models.Models;
using IBM.Watson.Assistant.v1;
using IBM.Cloud.SDK.Core.Authentication.Iam;

namespace LinC.ViewModels
{
    public class ChatPageViewModel : ViewModelBase
    {
        private AssistantService _conversation;
        private string _outGoingText;
        public ObservableCollection<ChatMessage> Messages { get; }
        private dynamic _context;
        bool _isInitial = true;

        AssistantService _service;
        string apikey = "21llXUihNU03NecpwwGBlqRdq4rI7Whn0JSMzokFFCZL";// "SQeNwCcd7t2FKkaN6vNudfTTHq4EMh_n5L_UFN6Adbxf";
        string serviceUrl = "https://api.eu-gb.assistant.watson.cloud.ibm.com";
        string versionDate = "2020-06-17";
        string workspaceId = "0eba8ac1-0429-4b9d-af21-25b5312925c8"; //"63b22869-12d7-4496-8e96-878bea5899d3"; // skill id


        public ChatPageViewModel()
        {
            Messages = new ObservableCollection<ChatMessage>();
            OutGoingText = string.Empty;
            ConnectToWatson();
        }

        protected override async Task OnShellNavigatingIn(string sender, ShellNavigatingEventArgs args)
        {
            await base.OnShellNavigatingIn(sender, args);
            //ConnectToWatson();
        }

        private void ConnectToWatson()
        {
            IamAuthenticator authenticator = new IamAuthenticator(
                apikey: $"{apikey}");

            _service = new AssistantService($"{versionDate}", authenticator);
            _service.SetServiceUrl($"{serviceUrl}");            
        }

        public string OutGoingText
        {
            get
            {
                return _outGoingText;
            }
            set
            {
                _outGoingText = value;
                RaisePropertyChanged("OutGoingText");
            }
        }


        public ICommand SendCommand => new Command(SendMessage);


        private async void SendMessage()
        {
            try
            {
                if (!string.IsNullOrEmpty(OutGoingText))
                {
                    Messages.Add(new ChatMessage { Text = OutGoingText, IsIncoming = false, MessageDateTime = DateTime.Now });
                    string temp = OutGoingText;
                    OutGoingText = string.Empty;
                    
                    await Task.Run(() =>
                    {
                        var result = _service.Message(
                             workspaceId: $"{workspaceId}",
                             input: new MessageInput()
                             {
                                 Text = temp
                             }
                         );

                        OnWatsonMessagerecieved(result.Response);
                    });
                }
            }
            catch (Exception)
            {

            }
            
        }

        private void OnWatsonMessagerecieved(string data)
        {

            Device.BeginInvokeOnMainThread(() =>
            {
                WatsonMessage message = JsonConvert.DeserializeObject<WatsonMessage>(data);

                var listItem = new ChatMessage
                {

                    IsIncoming = true,
                    MessageDateTime = DateTime.Now

                };


                if (message.Output.Generic != null)
                {
                    foreach (var item in message.Output.Generic)
                    {
                        if (item.ResponseType.Equals("image"))
                        {
                            listItem.Image = item.Source.ToString();
                        }
                        if (item.ResponseType.Equals("text"))
                        {
                            listItem.Text = item.Text;
                        }
                    }

                }
                Console.WriteLine(data);
                Messages.Add(listItem);
            });
        }

    }
}
