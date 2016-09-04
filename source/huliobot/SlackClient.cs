using System.Threading.Tasks;
using NLog;

namespace huliobot
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Specialized;
    using System.Net;
    using System.Text;

    public class MyLogger
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Debug(string message)
        {
            Logger.Debug(message);
            Slacker.Send(message).Wait();
        }


        public static void Error(Exception ex, string message)
        {
            Logger.Error(ex, message);
            Slacker.Send(message + ":" + ex.Message);
        }
    }


    //Sends messages to slack channel
    public class Slacker
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static Task Send(string message)
        {
            return Task.Run(() =>
            {
                try
                {
                    string urlWithAccessToken = SettingsStore.Settings["slackHulioUrl"];

                    SlackClient client = new SlackClient(urlWithAccessToken);

                    client.PostMessage(username: "Hulio",
                        text: message,
                        channel: "#hulio");
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Slacker is broken");
                }
            });


        }
    }


    //A simple C# class to post messages to a Slack channel
    //Note: This class uses the Newtonsoft Json.NET serializer available via NuGet
    public class SlackClient
    {
        private readonly Uri _uri;
        private readonly Encoding _encoding = new UTF8Encoding();

        public SlackClient(string urlWithAccessToken)
        {
            _uri = new Uri(urlWithAccessToken);
        }

        //Post a message using simple strings
        public void PostMessage(string text, string username = null, string channel = null)
        {
            Payload payload = new Payload()
            {
                Channel = channel,
                Username = username,
                Text = text
            };

            PostMessage(payload);
        }

        //Post a message using a Payload object
        public void PostMessage(Payload payload)
        {
            string payloadJson = JsonConvert.SerializeObject(payload);

            using (WebClient client = new WebClient())
            {
                NameValueCollection data = new NameValueCollection();
                data["payload"] = payloadJson;

                var response = client.UploadValues(_uri, "POST", data);

                //The response text is usually "ok"
                string responseText = _encoding.GetString(response);
            }
        }
    }

    //This class serializes into the Json payload required by Slack Incoming WebHooks
    public class Payload
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}