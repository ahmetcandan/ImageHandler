using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageHandler.Models
{
    public class UploadResponse
    {
        public Guid ImageId { get; set; }
        public string ImageName { get; set; }
        public string ImageType { get; set; }
        public DateTime CreateDate { get; set; }
    }
}