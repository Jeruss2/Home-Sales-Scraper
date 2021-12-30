using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using ScraperModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ListingsFrontEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportController : ControllerBase
    {

        [HttpGet]
        public IActionResult Export()
        {
            try
            {
                SqlConnection connection =
                    new SqlConnection(@"Data Source=.\SQLEXPRESS01;Initial Catalog=Scraper;Integrated Security=SSPI;");
                connection.Open();

                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = "SELECT Neighborhood, Address, Price, NumBeds, NumBath FROM Listing";

                cmd.Connection = connection;

                SqlDataReader reader = cmd.ExecuteReader();

                List<Listing> listings = new List<Listing>();

                while (reader.Read())
                {
                    Listing listing = new Listing();
                    listing.Neighborhood = reader.GetString(0);
                    listing.Address = reader.GetString(1);
                    listing.Price = reader.GetString(2);
                    listing.NumBeds = int.Parse(reader.GetString(3));
                    listing.NumBath = reader.GetString(4);

                    listings.Add(listing);
                }



                string listingJSON = JsonConvert.SerializeObject(listings);

                Console.WriteLine(listingJSON);

                System.IO.File.WriteAllText($"export_data_{DateTime.Now.Month}-{DateTime.Now.Day}-{DateTime.Now.Year}.txt", listingJSON);

                var data = Encoding.ASCII.GetBytes(listingJSON);

                return new FileContentResult(data, new
                    MediaTypeHeaderValue("application/octet"))
                {
                    FileDownloadName = $"export_data_{DateTime.Now.Month}-{DateTime.Now.Day}-{DateTime.Now.Year}.txt"
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }




        }
    }
}



