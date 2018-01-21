﻿using AirHelp.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHelp.Models
{
    public class VMUser
    {
        public string UserId {get; set;}
        public string Email { get; set; }
        public string Token { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PictureUrl { get; set; }
        public string Role { get; set; }
    }

    public class VMClaim
    {
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
    }

}