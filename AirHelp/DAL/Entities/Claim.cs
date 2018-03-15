
using AirHelp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirHelp.DAL
{
    public class Claim : EntityBase
    {
        public Claim()
        {
            this.AirPorts = new List<AirPort>();
            this.AdditionalUsers = new List<AdditionalUser>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ClaimId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int referalNumber { get; set; }
        
        public ClaimStatus State { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public DateTime DateCreated { get; set; }
        
        public string BordCardUrl { get; set; }
        public string BookConfirmationUrl { get; set; }
        public string AttorneyUrl { get; set; }

        public ProblemType Type { get; set; }

        public string FlightNumber { get; set; }

        public DateTime Date { get; set; }

        public string Reason { get; set; }

        public string HowMuch { get; set; }

        public string Annonsment { get; set; }

        public string BookCode { get; set; }

        public string AirCompany { get; set; }

        public string AirCompanyCountry { get; set; }

        public string AdditionalInfo { get; set; }

        public string Confirm { get; set; }

        public string Arival { get; set; }

        public string DocumentSecurity { get; set; }

        public string Willness { get; set; }

        public string Delay { get; set; }

        public string SignitureImage { get; set; }

        public virtual ICollection<AirPort> AirPorts { get; set; }

        public virtual ICollection<AdditionalUser> AdditionalUsers { get; set; }

        public double distance { get; set; }


    }

}