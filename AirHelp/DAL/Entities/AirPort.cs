
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirHelp.DAL
{
	public class AirPort : EntityBase
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        public Guid ClaimId { get; set; }
        [ForeignKey("ClaimId")]
        public virtual Claim Claim { get; set; }
        
        public int number { get; set; }

        public string iata { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string icao { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public double elevation { get; set; }
        public double timezone { get; set; }
		public string type { get; set; }
		public string ap_name { get; set; }

    }
}
