using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Net.Http;
using System.Xml;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Digital
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {


        public MainPage()
        {


            InitializeComponent();

            Google_trend();

            Sec_fade();

            Date_translate();

            Time();

            Weather();

            News();

           
        }


        private async void Time()
        {
            while (true)
            {

                ampm.Text = DateTime.Now.ToString("tt");
                h.Text = DateTime.Now.ToString("hh");
                m.Text = DateTime.Now.ToString("mm");
                s.Text = DateTime.Now.ToString("ss");
                
                
                date.Text = DateTime.Now.ToString("dddd MMMM dd, yyyy");

                if (s.Text == "20")
                {
                    await backgroundimage.FadeTo(0, 500);
                    backgroundimage.Source = "background2.jpg";
                    await backgroundimage.FadeTo(1, 500);
                }

                else if (DateTime.Now.ToString("ss") == "40")
                {
                    await backgroundimage.FadeTo(0, 500);
                    backgroundimage.Source = "background3.jpg";
                    await backgroundimage.FadeTo(1, 500);
                }

                else if (DateTime.Now.ToString("ss") == "59")
                {
                    await backgroundimage.FadeTo(0, 500);
                    backgroundimage.Source = "background1.jpg";
                    await backgroundimage.FadeTo(1, 500);
                }
                

                var cur = DateTime.Now;
                Analog(cur);

                await Task.Delay(1000);

            }

        }

        private void Analog(DateTime t)
        {

            boxS.RotateTo((t.Second -30 ) * 6, 1);
            boxM.RotateTo((t.Minute -30) * 6 + t.Second * 0.1, 1);
            boxH.RotateTo(((t.Hour -30) % 12) * 30 + t.Minute * 0.25 + t.Second * 0.25 / 60, 1);
           

        }
        private async void Sec_fade()
        {
            while (true)
            {
                s.TranslationX = -200;
                s.Opacity = 0;
                await s.FadeTo(1, 500);
                await Task.Delay(500);

            }
        }


        private async void Date_translate()

        {
            await Task.Delay(500);
            double translateValue = date.X + date.Width;

            while (true)
            {
                await date.TranslateTo(-translateValue, 0, 3000, Easing.Linear);
                date.TranslationX = translateValue;
                await date.TranslateTo(0, 0, 3000, Easing.Linear);

            }

        }

        private async void Google_trend()
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync("https://trends.google.co.kr/trends/trendingsearches/daily/rss?geo=KR");

            if (!response.IsSuccessStatusCode)
            {
                return;
            }

            string content = await response.Content.ReadAsStringAsync();
            XmlDocument document = new XmlDocument();

            document.LoadXml(content);

            XmlNodeList nodes = document.DocumentElement.SelectNodes("descendant::item");

            while (true)
            {
                foreach (XmlNode node in nodes)
                {
                    var title = node.SelectSingleNode("title");

                    Google.Text = title.InnerText;

                    await Google.FadeTo(1, 500);
                    await Task.Delay(3000);
                    await Google.FadeTo(0, 500);
                }
            }
        }




        private async void Weather()
        {


            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync("https://www.kma.go.kr/wid/queryDFSRSS.jsp?zone=2818582000");

            if (!response.IsSuccessStatusCode)
            {
                return;
            }

            string content = await response.Content.ReadAsStringAsync(); 

            XmlDocument document = new XmlDocument();

            document.LoadXml(content);

            XmlNode node = document.DocumentElement.SelectSingleNode("descendant::data"); 

            var wf = node.SelectSingleNode("wfKor"); 
            var temp = node.SelectSingleNode("temp"); 

            w.Text = wf.InnerText;
            t.Text = temp.InnerText;
        }


        private async void News()
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync("https://news.google.com/rss/search?hl=ko&gl=KR&ie=UTF-8&q=news&ceid=KR:ko");

            if (!response.IsSuccessStatusCode) 
            {
                return;
            }

            string content = await response.Content.ReadAsStringAsync(); 
            XmlDocument document = new XmlDocument();

            document.LoadXml(content);

            XmlNodeList nodes = document.DocumentElement.SelectNodes("descendant::item");

            while (true)
            {
                foreach (XmlNode node in nodes)
                {
                    var title = node.SelectSingleNode("title");

                    NewsTitle.Text = title.InnerText;
                    await NewsTitle.FadeTo(1, 500);
                    await Task.Delay(3000);
                    await NewsTitle.FadeTo(0, 500);
                }
            }
        }





    }




}