using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Scraper.Data.Layer;
using ScraperModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Scraper
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string baseUrl = "https://www.joehaydenrealtor.com/louisville-homes/?pg=";

            ScraperDAL scraperDal = new ScraperDAL();

            try
            {
                Random random = new Random();

                for (int i = 2; i < 10; i++)
                {
                    string url = baseUrl + i;

                    Task<IHtmlDocument> documentTask = GetHtmlDocument(url);

                    documentTask.Wait();

                    var document = documentTask.Result;

                    List<string> listings = GetListingsFromHtmlDoc(document);

                    foreach (var listing in listings)
                    {
                        scraperDal.SaveDataListingBlob(listing);
                    }


                    Thread.Sleep(random.Next(1000, 3000));
                }


            }
            catch (Exception e)
            {
                var error = new ErrorLog();

                error.ApplicationName = "Scraper";
                error.ExceptionMessage = e.Message;
                error.StackTrace = e.StackTrace;





                scraperDal.SaveErrorLog(error);
            }



        }

        private static async Task<IHtmlDocument> GetHtmlDocument(string url)
        {
            CancellationTokenSource cancellationToken = new CancellationTokenSource();
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage request = await httpClient.GetAsync(url);
            cancellationToken.Token.ThrowIfCancellationRequested();

            Stream response = await request.Content.ReadAsStreamAsync();
            cancellationToken.Token.ThrowIfCancellationRequested();

            HtmlParser parser = new HtmlParser();
            IHtmlDocument document = parser.ParseDocument(response);

            return document;
        }

        private static List<string> GetListingsFromHtmlDoc(IHtmlDocument document)
        {

            var listingElements = document.GetElementsByClassName("si-listings-column").Select(x => x.InnerHtml).ToList();

            return listingElements;
        }

        private static string ParseListingContent(string textContent)
        {
            List<string> data = textContent.Split(' ').Select(t => t.Trim()).Where(x => !string.IsNullOrEmpty(x) && x != "" && x != "\\n").ToList();

            string blob = string.Join(' ', data);

            blob = blob.Replace("Open 2/23 ", "");

            blob = blob.Remove(blob.LastIndexOf("Ft.") + 3);
            
            return blob;
        }

        private static void SaveToDatabase(List<string> listings)
        {
            System.IO.File.WriteAllLines(@"D:\OneDrive\Desktop\TestFolder\SavedLists.txt", listings);







        }
    }
}