using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace VKMess
{
    /// <summary>
    /// Логика взаимодействия для Messages.xaml
    /// </summary>
    public partial class Messages : Page
    {
        public MessagesClass mc = new MessagesClass();
        public ResponseUsers users = new ResponseUsers();
        public Messages()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            WebRequest webRequest = WebRequest.Create("https://api.vk.com/method/messages.getConversations?access_token=" + Properties.Settings.Default.token+ "&v=5.80&count=15");
            WebResponse webResponse = webRequest.GetResponse();
            Stream dataStream = webResponse.GetResponseStream();
            StreamReader streamReader = new StreamReader(dataStream);
            string response = streamReader.ReadToEnd().Replace("{\"response\":", "");
            streamReader.Close();
            webResponse.Close();
            response = response.Remove(response.Length-1);
            response = WebUtility.HtmlDecode(response);

            mc = JsonConvert.DeserializeObject<MessagesClass>(response);

            string usersIDs = null;

            for (int i = 0; i < mc.items.Length; i++)
            {
                usersIDs += mc.items[i].last_message.peer_id;
            }

            WebRequest webRequestUser = WebRequest.Create("https://api.vk.com/method/users.get?user_ids=" + usersIDs);
            WebResponse webResponseUser = webRequestUser.GetResponse();
            Stream dataStreamUser = webResponseUser.GetResponseStream();
            StreamReader streamReaderUser = new StreamReader(dataStreamUser);
            string responseUser = streamReaderUser.ReadToEnd();
            streamReaderUser.Close();
            webResponseUser.Close();
            responseUser = WebUtility.HtmlDecode(responseUser);

            users = JsonConvert.DeserializeObject<ResponseUsers>(response);
            MessageBox.Show(users.response[1].photo_50);
            for (int i = 0; i < mc.items.Length; i++)
            {
                Grid panel = new Grid();

                panel.Width = lb1.Width;
                panel.Height = 60;

                BitmapImage bmpImg = new BitmapImage();

                bmpImg.BeginInit();
                bmpImg.UriSource = new Uri(users.response[i].photo_50, UriKind.Absolute);
                bmpImg.EndInit();
                Image image = new Image();
                image.Source = bmpImg;
                image.Width = 50;
                image.Height = 50;

                Label id = new Label();
                id.Content = mc.items[i].last_message.peer_id;
                id.Height = 30;
                id.VerticalAlignment = VerticalAlignment.Top;
                id.Margin = new Thickness(60, 0, 0, 0);

                Label text = new Label();
                text.Content = mc.items[i].last_message.text;
                text.Height = 30;
                text.Margin = new Thickness(60, 30, 0, 0);
                text.VerticalAlignment = VerticalAlignment.Top;

                panel.Children.Add(image);
                panel.Children.Add(id);
                panel.Children.Add(text);

                lb1.Items.Add(panel);
            }
        }

        public class ResponseUsers
        {
            public Users[] response { get; set; }
        }

        public class Users
        {
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string photo_50 { get; set; }
        }

        public class MessagesClass
        {
            public string count { get; set; }
            public Item[] items { get; set; }
        }

        public class Item
        {
            public Conversation conversation { get; set; }
            public LastMessages last_message { get; set; }
        }

        public class Conversation
        {
            public Peer peer { get; set; }
            public string in_read { get; set; }
            public string out_read { get; set; }
            public string last_message_id { get; set; }
            public can_write can_write { get; set; }
        }

        public class Peer
        {
            public string id { get; set; }
            public string type { get; set; }
            public string local_id { get; set; }
        }

        public class can_write
        {
            public string allowed { get; set; }
        }

        public class LastMessages
        {
            public string date { get; set; }
            public string from_id { get; set; }
            public string id { get; set; }
            public string outt { get; set; }
            public string peer_id { get; set; }
            public string text { get; set; }
            public string conversation_message_id { get; set; }
          //  public fwd_messages fwd_messages { get; set; }
            public string important { get; set; }
            public string random_id { get; set; }
          //  public attachments attachments { get; set; }
            public string is_hidden { get; set; }
        }

        public class fwd_messages
        {
            public string date { get; set; }
            public string from_id { get; set; }
            public string text { get; set; }
            public attachments attachments { get; set; }
            public string update_time { get; set; }
        }

        public class attachments
        {
            public string type { get; set; }
            public audio audio { get; set; }
        }

        public class audio
        {
            public string artist { get; set; }
            public string title { get; set; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
