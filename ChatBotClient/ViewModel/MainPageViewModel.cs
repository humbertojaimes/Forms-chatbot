using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ChatBotClient.Model.HttpClasses;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace ChatBotClient.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {

        #region Class properties
        static HttpClient chatClient;
        static HttpClient startConversationClient;
        static PostResult postResult;
        static ActivityToPost activityToPost;
        static GetResult getResult;
        static string botUriStartConversation;
        static string botUriChat;
        static string botSecret;
        static string activity;
        static bool firstMessage;
        static MessageRequest messageRequest;
        static StringContent content;
        #endregion
        #region Properties


        private string outgoingText;
        public string OutGoingText
        {
            get { return outgoingText; }
            set
            {
                outgoingText = value;
                activityToPost = new ActivityToPost
                {
                    type = "message",
                    from = new User { id = "user1" },
                    text = outgoingText,
                    locale = "es-MX",
                };
                activity = JsonConvert.SerializeObject(activityToPost);
                content = new StringContent(activity, Encoding.UTF8, "application/json");
                RaisePropertyChanged();
            }
        }
        #endregion


        #region Commands
        public ICommand SendMessageCommand { get; set; }
        public async void SendMessage()
        {
            Messages.Add(new MessageViewModel { Text = outgoingText, IsIncoming = false, MessagDateTime = DateTime.Now.AddMinutes(-25) });
            postResult = JsonConvert.DeserializeObject<PostResult>(await PostAsync(botUriChat, content));
            if (postResult != null)
            {
                if (firstMessage)
                {
                    getResult = JsonConvert.DeserializeObject<GetResult>(await chatClient.GetStringAsync(botUriChat));
                    firstMessage = false;
                }

                else
                {
                    string jsonResultFromBot = await chatClient.GetStringAsync(botUriChat + "?watermark=" + getResult.watermark);
                    getResult = JsonConvert.DeserializeObject<GetResult>(jsonResultFromBot);
                }

                for (int i = 1; i < getResult.activities.Count; i++)
                {
                    if (getResult.activities[i].attachments.Count > 0)
                    {
                        foreach (var content in getResult.activities[i].attachments)
                        {
                            StringBuilder message = new StringBuilder();

                            message.AppendLine(content.content.text);

                            foreach (var button in content.content.buttons)
                            {
                                message.AppendLine(button.title);
                            }
                            Messages.Add(new MessageViewModel { Text = "Bot: " + message.ToString(), IsIncoming = true, MessagDateTime = DateTime.Now.AddMinutes(-25) });

                        }
                    }
                    else
                    {
                        Messages.Add(new MessageViewModel { Text = "Bot: " + getResult.activities[i].text, IsIncoming = true, MessagDateTime = DateTime.Now.AddMinutes(-25) });

                    }
                }
            }

            OutGoingText = string.Empty;

        }
        #endregion
        #region Methods
        public async void StartConversation()
        {
            StringContent content = new StringContent("", Encoding.UTF8, "application/json");
            try
            {
                string result = await PostAsync(botUriStartConversation, content);
                messageRequest = JsonConvert.DeserializeObject<MessageRequest>(result);
                botUriChat = String.Format(botUriChat, messageRequest.conversationId);
                Messages.Add(new MessageViewModel { Text = "Bot: Listo para comenzar", IsIncoming = true, MessagDateTime = DateTime.Now.AddMinutes(-25) });

            }
            catch (Exception ex)
            {
                botSecret = ex.ToString();
            }
        }

        private static async Task<string> PostAsync(string uri, HttpContent content)
        {
            try
            {
                HttpResponseMessage response = await startConversationClient.PostAsync(uri, content);
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        #endregion



        private ObservableCollection<MessageViewModel> messagesList;

        public ObservableCollection<MessageViewModel> Messages
        {
            get { return messagesList; }
            set { messagesList = value; RaisePropertyChanged(); }
        }



        public ICommand SendCommand { get; set; }


        public MainPageViewModel()
        {
            SendMessageCommand = new Command(SendMessage);
            getResult = new GetResult();
            firstMessage = true;

            botUriStartConversation = "https://directline.botframework.com/v3/directline/conversations/";
            botUriChat = "https://directline.botframework.com/v3/directline/conversations/{0}/activities";
            botSecret = "_mVKnEKCmM8.cwA.5XA.gSXRFS_SgSX3kWWJfHwnwXHodV5KwubNLqB4BXA5mN0";
            //"UhG0pRRMQFk.cwA._KU.6J6uMpDCs2jnMcoGbGniSYLPTaIwzYKj2va11PA3O4U";
            chatClient = new HttpClient();
            startConversationClient = new HttpClient();
            chatClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + botSecret);
            startConversationClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + botSecret);

            StartConversation();


            // Initialize with default values
            Messages = new ObservableCollection<MessageViewModel>();

            OutGoingText = null;
            SendCommand = new Command(() =>
            {
                SendMessage();
            });
        }


    }
}
