using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace AspNetTutorials.Models
{
    public class IpRecord
    {
        [Key]
        public int Id { get; set; }

        public string IpAddress { get; set; }

        public DateTime TimeOfRecord { get; set; }

        public DateTime LastTimeOfIssue { get; set; }
     }
}