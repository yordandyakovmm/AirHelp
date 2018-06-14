
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirHelp.DAL
{
	public class AirPortData : EntityBase
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Index]
        [Column(TypeName = "VARCHAR")]
        [StringLength(4)]
        public string iata { get; set; }

        [Index]
        [Column(TypeName = "VARCHAR")]
        [StringLength(100)]
        public string name { get; set; }

        [Index]
        [Column(TypeName = "VARCHAR")]
        [StringLength(40)]
        public string city { get; set; }

        [Index]
        [Column(TypeName = "VARCHAR")]
        [StringLength(40)]
        public string country { get; set; }


        public string countryCode { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public double elevation { get; set; }
        public double timezone { get; set; }
		public string url { get; set; }



    }
}
