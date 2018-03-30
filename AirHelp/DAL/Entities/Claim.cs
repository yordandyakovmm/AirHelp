
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
            this.Documents = new LinkedList<Document>();
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
        
        public string AttorneyUrl { get; set; }

        public ProblemType Type { get; set; }

        public string FlightNumber { get; set; }

        public string FlightDate { get; set; }

        public double allDistance { get; set; }

        public double issueDistance { get; set; }
        
        public Reason Reason { get; set; }

        public DelayDelay DelayDelay { get; set; }

        public CancelOverbokingDelay CancelOverbokingDelay { get; set; }

        public CancelAnnonsment CancelAnnonsment { get; set; }

        public DenayArival DenayArival { get; set; }

        public DocumentSecurity DocumentSecurity { get; set; }

        public Willness Willness { get; set; }


        public string BookingCode { get; set; }

        public string TikedNumber { get; set; }


        public string AirCompany { get; set; }

        public string AirCompanyCountry { get; set; }

        public string AirCompanyCode { get; set; }


        public string AdditionalInfo { get; set; }

        public string Confirm { get; set; }

        public string SignitureImage { get; set; }


        public int CompensationAmount { get; set; }

        public string CompensationReason { get; set; }


        public IsEUFlight IsEUFlight { get; set; }

        public FlightType FlightType  { get; set; }


        public virtual ICollection<AirPort> AirPorts { get; set; }

        public virtual ICollection<AdditionalUser> AdditionalUsers { get; set; }

        public virtual ICollection<Document> Documents { get; set; }

        


    }

}