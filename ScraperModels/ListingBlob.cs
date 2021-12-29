using System;

namespace ScraperModels
{
    public class ListingBlobs
    {

        public int ListingBlobId { get; set; } 
        public string ListingBlob { get; set; }

        public DateTime? DateAdded { get; set; }

        public bool? Processed { get; set; }

    }
}
