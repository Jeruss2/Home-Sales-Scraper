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





                //scraperDal.SaveErrorLog(error);
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

            //join the blob with a space


            //if (blob.Contains('{'))
            //    blob = blob.Substring(0, blob.LastIndexOf('{'));





            blob = blob.Replace("Open 2/23 ", "");

            //    .Replace("Re/Max Properties East", "")
            //    .Replace("Re/Max Associates Of Louisville", "")
            //    .Replace("Wakefield Reutlinger Realtors", "")
            //    .Replace("Exp Realty Llc", "")
            //    .Replace("Champion Properties", "")
            //    .Replace("Red Edge Realty", "")
            //    .Replace("Semonin Realtor", "")
            //    .Replace("Mayer Realtors", "")
            //    .Replace("Mary Jane Halbleib Realtors", "");
            //.Replace("", "")
            //.Replace("{"@context": "https://schema.org/","@type": "Event","name": "Open House - 2:00 PM - 4:00 PM","description": "Open House","url": "https://www.joehaydenrealtor.com/homes/435-whiteheath-ln-louisville-ky-40243/11502886_spid/","startDate": "2020-02-23T14:00:00-5:00","endDate": "2020-02-23T16:00:00-5:00","location": {"@type": "Place","name": "435 Whiteheath Ln","address": {"@type": "PostalAddress","streetAddress": "435 Whiteheath Ln","postalCode": "40243","addressLocality": "Louisville","addressRegion": "KY"},"geo": {"@type": "GeoCoordinates","latitude": 38.234731,"longitude": -85.558343}}}", "");

            blob = blob.Remove(blob.LastIndexOf("Ft.") + 3);
            //blob = blob.Remove(blob.Substring(blob.IndexOf("$")));


            return blob;
        }

        private static void SaveToDatabase(List<string> listings)
        {
            System.IO.File.WriteAllLines(@"D:\OneDrive\Desktop\TestFolder\SavedLists.txt", listings);







        }
    }
}