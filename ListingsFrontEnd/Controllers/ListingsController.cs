using Microsoft.AspNetCore.Mvc;
using ScraperModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ListingsFrontEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingsController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Listing> Get()
        {
            SqlConnection connection =
                new SqlConnection(@"Data Source=.\SQLEXPRESS01;Initial Catalog=Scraper;Integrated Security=SSPI;");
            connection.Open();


            //Connect();

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "SELECT DISTINCT Address, Neighborhood, Price, NumBeds, NumBath FROM Listing";

            cmd.Connection = connection;

            SqlDataReader reader = cmd.ExecuteReader();

            List<Listing> listings = new List<Listing>();

            while (reader.Read())
            {
                Listing listing = new Listing();
                listing.Neighborhood = reader.GetString(1);
                listing.Address = reader.GetString(0);
                listing.Price = reader.GetString(2);
                listing.NumBeds = int.Parse(reader.GetString(3));
                listing.NumBath = reader.GetString(4);

                listings.Add(listing);
            }

            return listings;
        }

        // GET: api/Listings/5
        [HttpGet("{neighborhood}", Name = "Get")]
        public List<Listing> Get(string neighborhood)
        {
            try
            {
                List<Listing> listings = new List<Listing>();

                using (SqlConnection connection =
                    new SqlConnection(@"Data Source=.\SQLEXPRESS01;Initial Catalog=Scraper;Integrated Security=SSPI;"))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "SELECT DISTINCT Address, Neighborhood, Price, NumBeds, NumBath FROM Listing WHERE Neighborhood = @neighborhood";
                        cmd.Parameters.AddWithValue("@neighborhood", neighborhood);
                        cmd.Connection = connection;
                        connection.Open();

                        SqlDataReader reader = cmd.ExecuteReader();



                        while (reader.Read())
                        {
                            Listing listing = new Listing();
                            listing.Neighborhood = reader.GetString(1);
                            listing.Address = reader.GetString(0);
                            listing.Price = reader.GetString(2);
                            listing.NumBeds = int.Parse(reader.GetString(3));
                            listing.NumBath = reader.GetString(4);

                            listings.Add(listing);
                        }

                        return listings;
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }



        }


        // POST: api/Listings
        [HttpPost]
        public void AddListing([FromBody] Listing listing)
        {
            SqlConnection connection = new SqlConnection(@"Data Source=.\SQLEXPRESS01;Initial Catalog=Scraper;Integrated Security=SSPI;");
            connection.Open();

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "insert into Listing\r\nvalues (@Neighborhood, @Address, @Price, @NumBeds, @NumBath)";

            SqlParameter param = new SqlParameter();
            param.ParameterName = "@Neighborhood";
            param.Value = listing.Neighborhood;

            SqlParameter param1 = new SqlParameter();
            param1.ParameterName = "@Address";
            param1.Value = listing.Address;

            SqlParameter param2 = new SqlParameter();
            param2.ParameterName = "@Price";
            param2.Value = listing.Price;

            SqlParameter param3 = new SqlParameter();
            param3.ParameterName = "@NumBeds";
            param3.Value = listing.NumBeds;

            SqlParameter param4 = new SqlParameter();
            param4.ParameterName = "@NumBath";
            param4.Value = listing.NumBath;

            cmd.Parameters.Add(param);
            cmd.Parameters.Add(param1);
            cmd.Parameters.Add(param2);
            cmd.Parameters.Add(param3);
            cmd.Parameters.Add(param4);

            cmd.Connection = connection;

            cmd.ExecuteNonQuery();
        }

        // PUT: api/Listings/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
