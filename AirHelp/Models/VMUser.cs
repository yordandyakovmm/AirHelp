using AirHelp.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHelp.Models
{
    public class VMUser
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PictureUrl { get; set; }
        public string Role { get; set; }
    }

    public class AirportDistance
    {
        public string From { get; set; }
        public string To { get; set; }

        public double distance { get; set; }
    }

    public class VMClaim
    {

        public VMClaim(Claim claim)
        {
            ClaimId = claim.ClaimId;
            State = claim.State;

            User = null;
            DateCreated = claim.DateCreated;

            BordCardUrl = claim.BordCardUrl;
            BookConfirmationUrl = claim.BookConfirmationUrl;
            Type = claim.Type;
            ConnectionAriports = claim.ConnectionAriports;
            FirstName = claim.FirstName;
            LastName = claim.LastName;
            City = claim.City;
            Country = claim.Country;
            Adress = claim.Adress;
            Email = claim.Email;
            Tel = claim.Tel;
            FlightNumber = claim.FlightNumber;
            Date = claim.Date;
            DepartureAirport = claim.DepartureAirport;
            DestinationAirports = claim.DestinationAirports;
            HasConnection = claim.HasConnection;
            ConnectionAirports = claim.ConnectionAirports;
            Reason = claim.Reason;
            HowMuch = claim.HowMuch;
            Annonsment = claim.Annonsment;
            BookCode = claim.BookCode;
            AirCompany = claim.AirCompany;
            AdditionalInfo = claim.AdditionalInfo;
            Confirm = claim.Confirm;
            Arival = claim.Arival;
            DocumentSecurity = claim.DocumentSecurity;
            Willness = claim.Willness;
            Delay = claim.Delay;
            SignitureImage = claim.SignitureImage;
            AttorneyUrl = claim.AttorneyUrl;
            AirporstDistance = new List<AirportDistance>();

        }

        public VMClaim FromClaim(Claim claim)
        {
            ClaimId = claim.ClaimId;
            State = claim.State;

            User = null;
            DateCreated = claim.DateCreated;

            BordCardUrl = claim.BordCardUrl;
            BookConfirmationUrl = claim.BookConfirmationUrl;
            Type = claim.Type;
            ConnectionAriports = claim.ConnectionAriports;
            FirstName = claim.FirstName;
            LastName = claim.LastName;
            City = claim.City;
            Country = claim.Country;
            Adress = claim.Adress;
            Email = claim.Email;
            Tel = claim.Tel;
            FlightNumber = claim.FlightNumber;
            Date = claim.Date;
            DepartureAirport = claim.DepartureAirport;
            DestinationAirports = claim.DestinationAirports;
            HasConnection = claim.HasConnection;
            ConnectionAirports = claim.ConnectionAirports;
            Reason = claim.Reason;
            HowMuch = claim.HowMuch;
            Annonsment = claim.Annonsment;
            BookCode = claim.BookCode;
            AirCompany = claim.AirCompany;
            AdditionalInfo = claim.AdditionalInfo;
            Confirm = claim.Confirm;
            Arival = claim.Arival;
            DocumentSecurity = claim.DocumentSecurity;
            Willness = claim.Willness;
            Delay = claim.Delay;
            SignitureImage = claim.SignitureImage;
            AttorneyUrl = claim.AttorneyUrl;
            return this;
        }
        public Guid ClaimId { get; set; }

        public string State { get; set; }
        public User User { get; set; }
        public DateTime DateCreated { get; set; }

        public string BordCardUrl { get; set; }
        public string BookConfirmationUrl { get; set; }

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
        public string AdditionalInfo { get; set; }
        public string Confirm { get; set; }
        public string Arival { get; set; }

        public string DocumentSecurity { get; set; }
        public string Willness { get; set; }
        public string Delay { get; set; }
        public string SignitureImage { get; set; }
        public string AttorneyUrl { get; set; }
        public int CompensationAmount { get; set; }
        public List<AirportDistance> AirporstDistance { get; set; }
        public double totalDistance { get; set; }
        public bool rightOfCompensation { get; set; }
        public string reasonOfRejection { get; set; }


    }

}