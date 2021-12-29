using HtmlAgilityPack;
using ScraperModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HtmlParser
{
    public class ListingParser
    {
        public List<Listing> ParseListings(List<ListingBlobs> blobs)
        {
            List<Listing> parsedListings = new List<Listing>();

            foreach (var blob in blobs)
            {
                var htmlDoc = new HtmlDocument();

                try
                {
                    htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(blob.ListingBlob);

                    var nodes = htmlDoc.DocumentNode.AncestorsAndSelf().ToList();

                    string address = nodes[0].SelectNodes("//*[contains(@class, 'si-listing-photo')]").ToList()[0].Attributes["alt"].Value;

                    string price = nodes[0].SelectNodes("//*[contains(@class, 'si-listing__photo-price')]").ToList()[0]
                        .InnerText.Replace("\n", "").Split(' ').Where(x => !string.IsNullOrEmpty(x)).FirstOrDefault();
                    //price = price.Remove(price.Length - 1);

                    string numBeds = nodes[0].SelectNodes("//*[contains(@class, 'si-listing__info-value')]").ToList()[0].InnerText.Replace("\n", "").Trim();


                    string numBath = nodes[0].SelectNodes("//*[contains(@class, 'si-listing__info-value')]").ToList()[1].InnerText.Replace("\n", "").Trim();


                    string neighborhood = nodes[0].SelectNodes("//*[contains(@class, 'si-listing__neighborhood-place')]").ToList()[0].InnerText;

                    if (neighborhood == "&nbsp;")
                    {
                        neighborhood = "Not Available";
                    }

                    Listing l = new Listing()
                    {
                        Address = address,
                        Neighborhood = neighborhood,
                        Price = price,
                        NumBath = numBath,
                        NumBeds = int.Parse(numBeds)
                    };

                    parsedListings.Add(l);
                }
                catch (Exception e)
                {
                    File.WriteAllText("parseException_" + DateTime.UtcNow.ToFileTimeUtc(),
                        e.Message + "\r\n\r\n" + htmlDoc.DocumentNode.InnerHtml);
                }


            }

            return parsedListings;
        }
    }
}



