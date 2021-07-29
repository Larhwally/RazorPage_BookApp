using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBookApp.Models
{
    public class book
    {
        public int id { get; set; }
        public string bookTitle { get; set; }
        public string authorName { get; set; }
        public string ISBN { get; set; }
        public DateTime publishYear { get; set; }
        public DateTime createDate { get; set; }
        public string createdBy { get; set; }
        public string status { get; set; }
    }
}
