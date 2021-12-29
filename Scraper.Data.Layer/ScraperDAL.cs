using ScraperModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Scraper.Data.Layer
{
    public class ScraperDAL
    {
        public void SaveListings(IEnumerable<Listing> listings)
        {
            SqlConnection connection = new SqlConnection(@"Data Source=.\SQLEXPRESS01;Initial Catalog=Scraper;Integrated Security=SSPI;");
            connection.Open();

            foreach (var listing in listings)
            {
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = @"Insert into Listing Values (@Neighborhood, @Address, @Price, @NumBeds, @NumBath)";

                cmd.Parameters.AddWithValue("@Address", listing.Address);
                cmd.Parameters.AddWithValue("@Neighborhood", listing.Neighborhood);
                cmd.Parameters.AddWithValue("@Price", listing.Price);
                cmd.Parameters.AddWithValue("@NumBeds", listing.NumBeds);
                cmd.Parameters.AddWithValue("@NumBath", listing.NumBath);

                cmd.Connection = connection;

                cmd.ExecuteNonQuery();

            }

            connection.Close();
        }

        public void SaveDataListingBlob(string listing)
        {
            SqlConnection connection = new SqlConnection(@"Data Source=.\SQLEXPRESS01;Initial Catalog=Scraper;Integrated Security=SSPI;");
            connection.Open();

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = @"Insert into ListingBlobs Values (@ListingBlob, @DateAdded, @Processed)";

            cmd.Parameters.AddWithValue("@ListingBlob", listing);
            cmd.Parameters.AddWithValue("@DateAdded", DateTime.Now);
            cmd.Parameters.AddWithValue("@Processed", false);

            cmd.Connection = connection;

            cmd.ExecuteNonQuery();

            connection.Close();
        }

        public void SaveErrorLog(ErrorLog error)
        {
            SqlConnection connection = new SqlConnection(@"Data Source=.\SQLEXPRESS01;Initial Catalog=Scraper;Integrated Security=SSPI;");
            connection.Open();

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = @"Insert into Errorlog Values (@ExceptionMessage, @DateAdded, @StackTrace, @ApplicationName)";

            cmd.Parameters.AddWithValue("@ExceptionMessage", error.ExceptionMessage);
            cmd.Parameters.AddWithValue("@DateAdded", DateTime.Now);
            cmd.Parameters.AddWithValue("@StackTrace", error.StackTrace);
            cmd.Parameters.AddWithValue("@ApplicationName", error.ApplicationName);

            cmd.Connection = connection;

            cmd.ExecuteNonQuery();
        }

        public List<ListingBlobs> GetUnprocessedListingBlobs()
        {
            List<ListingBlobs> blobs = new List<ListingBlobs>();

            SqlConnection connection = new SqlConnection(@"Data Source=.\SQLEXPRESS01;Initial Catalog=Scraper;Integrated Security=SSPI;");
            connection.Open();

            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = @"SELECT [ListingBlobId]
                                      ,[ListingBlob]
                                      ,[DateAdded]
                                      ,[Processed]
                                  FROM [dbo].[ListingBlobs]
                                  WHERE Processed = 0";

            cmd.Connection = connection;

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ListingBlobs l = new ListingBlobs();

                l.ListingBlobId = reader.GetInt32(0);
                l.ListingBlob = reader.GetString(1);
                l.DateAdded = reader.GetDateTime(2);


                blobs.Add(l);
            }

            cmd.Connection = connection;

            return blobs;
        }

        public void MarkProcessed(int listingBlobId)
        {
            SqlConnection connection = new SqlConnection(@"Data Source=.\SQLEXPRESS01;Initial Catalog=Scraper;Integrated Security=SSPI;");
            connection.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@listingBlobId", listingBlobId);

            cmd.CommandText = @"Update ListingBlobs

                                Set Processed = 1

                                Where listingBlobId = @listingBlobId";

            cmd.Connection = connection;
            cmd.ExecuteNonQuery();
        }


        public List<Listing> GetListingsInDB()
        {
            using SqlConnection connection =
                new SqlConnection(@"Data Source=.\SQLEXPRESS01;Initial Catalog=Scraper;Integrated Security=SSPI;");

            string sqlQuery = "Select Neighborhood, Address, Price, NumBeds, NumBath from Listings";

            SqlCommand command = new SqlCommand(sqlQuery, connection);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            var listInDb = new List<Listing>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Listing listingInDb = new Listing();

                    listingInDb.Neighborhood = reader.GetString(0);
                    listingInDb.Address = reader.GetString(1);
                    listingInDb.Price = reader.GetString(2);
                    listingInDb.NumBeds = int.Parse(reader.GetString(3));
                    listingInDb.NumBath = reader.GetString(4);

                    listInDb.Add(listingInDb);
                }
            }

            return listInDb;
        }
    }
}