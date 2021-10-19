using System;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;
using HtmlAgilityPack;


namespace CsharpParser
{
    public partial class FormParser : Form
    {
        public FormParser()
        {
            InitializeComponent();
        }

        private void btnParsing_Click(object sender, EventArgs e)
        {
            GetHtmlAsync();
        }
       
        private async void GetHtmlAsync()
        {
            var url = "https://www.avito.ru/krasnodar/avtomobili/toyota/mark_ii-ASgBAgICAkTgtg20mSjitg2qqig?f=ASgBAgECA0TyCrCKAeC2DbSZKOK2DaqqKAFFxpoMFnsiZnJvbSI6MCwidG8iOjMwMDAwMH0&radius=200";

            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            var htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.LoadHtml(html);

            var productsHtml = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("items-items-kAJAg")).ToList();

            var productListItems = productsHtml[0].Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Contains("iva-item-root-Nj_hb")).ToList();

            foreach (var productListItem in productListItems)
            {
                // id предмета
                textBoxResult.Text += "Id предмета: " + productListItem.GetAttributeValue("data-item-id", "") + "\r\n";

                // заголовок.
                textBoxResult.Text += "Заголовок автомобиля: " + productListItem.Descendants("h3")
                    .Where(node => node.GetAttributeValue("class", "")
                    .Equals("title-root-j7cja iva-item-title-_qCwt title-listRedesign-XHq38 title-root_maxHeight-SXHes text-text-LurtD text-size-s-BxGpL text-bold-SinUO"))
                    .FirstOrDefault().InnerText + "\r\n";

                // стоимость
                textBoxResult.Text += "Стоимость автомобиля: " + productListItem.Descendants("span")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("price-text-E1Y7h text-text-LurtD text-size-s-BxGpL"))
                .FirstOrDefault().InnerText + "\r\n";

                // url
                textBoxResult.Text += "Url ссылка на автомобиль: https://www.avito.ru" + productListItem.Descendants("a").FirstOrDefault().GetAttributeValue("href", "") + "\r\n" + "\r\n";
            }
        }

        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
            textBoxResult.Text = "";
        }
    }
}