
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
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid ClaimId { get; set; }

        public string State { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public DateTime DateCreated { get; set; }

        public string BordCardUrl { get; set; }
        public string BookConfirmationUrl { get; set; }
        public string AttorneyUrl { get; set; }

        public string Type { get; set; }
        public string ConnectionAriports { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Adress { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string FlightNumber { get; set; }
        public string Date { get; set; }
        public string DepartureAirport { get; set; }
        public string DestinationAirports { get; set; }
        public string HasConnection { get; set; }
        public string ConnectionAirports { get; set; }
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


    }

}