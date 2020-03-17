using Scraper.Data.Layer;
using ScraperModels;
using System;
using System.Collections.Generic;

namespace HtmlParser
{
    class Program
    {
        static void Main(string[] args)
        {
            ScraperDAL scraperDal = new ScraperDAL();

            ListingParser parser = new ListingParser();

            try
            {
                var listingBlobs = scraperDal.GetUnprocessedListingBlobs();


                List<Listing> parsedListings = parser.ParseListings(listingBlobs);


                scraperDal.SaveListings(parsedListings);

                //mark as processed

                foreach (var listingBlob in listingBlobs)
                {
                    scraperDal.MarkProcessed(listingBlob.ListingBlobId);
                }



                //after the information is parsed 
                // mark processed as "1" and then move on 
                //update the method 





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
    }
}
