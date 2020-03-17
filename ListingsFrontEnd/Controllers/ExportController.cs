using Microsoft.AspNetCore.Mvc;
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
            SqlConnection connection =
                new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=Josh;Integrated Security=SSPI;");
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

            //System.IO.File.WriteAllText(, listingJSON);

            var net = new System.Net.WebClient();
            var data = Encoding.ASCII.GetBytes(listingJSON);
            var content = new System.IO.MemoryStream(data);
            var contentType = "APPLICATION/octet-stream";
            var fileName = "something.bin";


            //System.IO.File.WriteAllBytes(@"D:\OneDrive\Desktop", data);


            return File(content, contentType, fileName);


        }
    }
}



//@"D:\OneDrive\Desktop", listingJSON