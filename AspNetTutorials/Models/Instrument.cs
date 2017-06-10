using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetTutorials.Models
{
    public enum InstrumentType
    {
        Guitar,
        Drums,
        Piano
    }

    public class Instrument
    {
        [Key]
        public int Id { get; set; }

        public InstrumentType Type { get; set; }

        public string Manufacturer { get; set; }

        public string Model { get; set; }

        public double Price { get; set; }

    }

    public class MusicShopDbContext : DbContext
    {
        public DbSet<Instrument> Instruments { get; set; }

        public DbSet<IpRecord> IpRecords { get; set; }
    }
}
