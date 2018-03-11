using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHelp.Models
{

    public class FlightStatus
    {
        public Request request { get; set; }
        public Flightstatus[] flightStatuses { get; set; }
        public Appendix appendix { get; set; }
    }

    public class Request
    {
        public string endpoint { get; set; }
        public Carrier carrier { get; set; }
        public Year year { get; set; }
        public Month month { get; set; }
        public Day day { get; set; }
        public Flightnumber flightNumber { get; set; }
        public Isutc isUtc { get; set; }
        public string url { get; set; }
    }

    public class Carrier
    {
        public string requested { get; set; }
        public string interpreted { get; set; }
    }

    public class Year
    {
        public string requested { get; set; }
        public int interpreted { get; set; }
    }

    public class Month
    {
        public string requested { get; set; }
        public int interpreted { get; set; }
    }

    public class Day
    {
        public string requested { get; set; }
        public int interpreted { get; set; }
    }

    public class Flightnumber
    {
        public string requested { get; set; }
        public int interpreted { get; set; }
    }

    public class Isutc
    {
        public string requested { get; set; }
        public bool interpreted { get; set; }
    }

    public class Appendix
    {
        public Airlines[] airlines { get; set; }
        public Airports[] airports { get; set; }
        public Equipment[] equipments { get; set; }
    }

    public class Airlines
    {
        public string fs { get; set; }
        public string iata { get; set; }
        public string icao { get; set; }
        public string name { get; set; }
        public bool active { get; set; }
        public string category { get; set; }
        public string phoneNumber { get; set; }
    }

    public class Airports
    {
        public string fs { get; set; }
        public string iata { get; set; }
        public string icao { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string cityCode { get; set; }
        public string countryCode { get; set; }
        public string countryName { get; set; }
        public string regionName { get; set; }
        public string timeZoneRegionName { get; set; }
        public DateTime localTime { get; set; }
        public float utcOffsetHours { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public int elevationFeet { get; set; }
        public int classification { get; set; }
        public bool active { get; set; }
    }

    public class Equipment
    {
        public string iata { get; set; }
        public string name { get; set; }
        public bool turboProp { get; set; }
        public bool jet { get; set; }
        public bool widebody { get; set; }
        public bool regional { get; set; }
    }

    public class Flightstatus
    {
        public int flightId { get; set; }
        public string carrierFsCode { get; set; }
        public string operatingCarrierFsCode { get; set; }
        public string primaryCarrierFsCode { get; set; }
        public string flightNumber { get; set; }
        public string departureAirportFsCode { get; set; }
        public string arrivalAirportFsCode { get; set; }
        public Departuredate departureDate { get; set; }
        public Arrivaldate arrivalDate { get; set; }
        public string status { get; set; }
        public Schedule schedule { get; set; }
        public Operationaltimes operationalTimes { get; set; }
        public Codeshare[] codeshares { get; set; }
        public Delays delays { get; set; }
        public Flightdurations flightDurations { get; set; }
        public Airportresources airportResources { get; set; }
        public Flightequipment flightEquipment { get; set; }
        public object[] irregularOperations { get; set; }
    }

    public class Departuredate
    {
        public DateTime dateUtc { get; set; }
        public DateTime dateLocal { get; set; }
    }

    public class Arrivaldate
    {
        public DateTime dateUtc { get; set; }
        public DateTime dateLocal { get; set; }
    }

    public class Schedule
    {
        public string flightType { get; set; }
        public string serviceClasses { get; set; }
        public string restrictions { get; set; }
        public object[] uplines { get; set; }
        public object[] downlines { get; set; }
    }

    public class Operationaltimes
    {
        public Publisheddeparture publishedDeparture { get; set; }
        public Scheduledgatedeparture scheduledGateDeparture { get; set; }
        public Estimatedgatedeparture estimatedGateDeparture { get; set; }
        public Actualgatedeparture actualGateDeparture { get; set; }
        public Estimatedrunwaydeparture estimatedRunwayDeparture { get; set; }
        public Actualrunwaydeparture actualRunwayDeparture { get; set; }
        public Publishedarrival publishedArrival { get; set; }
        public Scheduledgatearrival scheduledGateArrival { get; set; }
        public Estimatedgatearrival estimatedGateArrival { get; set; }
        public Actualgatearrival actualGateArrival { get; set; }
    }

    public class Publisheddeparture
    {
        public DateTime dateUtc { get; set; }
        public DateTime dateLocal { get; set; }
    }

    public class Scheduledgatedeparture
    {
        public DateTime dateUtc { get; set; }
        public DateTime dateLocal { get; set; }
    }

    public class Estimatedgatedeparture
    {
        public DateTime dateUtc { get; set; }
        public DateTime dateLocal { get; set; }
    }

    public class Actualgatedeparture
    {
        public DateTime dateUtc { get; set; }
        public DateTime dateLocal { get; set; }
    }

    public class Estimatedrunwaydeparture
    {
        public DateTime dateUtc { get; set; }
        public DateTime dateLocal { get; set; }
    }

    public class Actualrunwaydeparture
    {
        public DateTime dateUtc { get; set; }
        public DateTime dateLocal { get; set; }
    }

    public class Publishedarrival
    {
        public DateTime dateUtc { get; set; }
        public DateTime dateLocal { get; set; }
    }

    public class Scheduledgatearrival
    {
        public DateTime dateUtc { get; set; }
        public DateTime dateLocal { get; set; }
    }

    public class Estimatedgatearrival
    {
        public DateTime dateUtc { get; set; }
        public DateTime dateLocal { get; set; }
    }

    public class Actualgatearrival
    {
        public DateTime dateUtc { get; set; }
        public DateTime dateLocal { get; set; }
    }

    public class Delays
    {
        public int departureGateDelayMinutes { get; set; }
    }

    public class Flightdurations
    {
        public int scheduledBlockMinutes { get; set; }
        public int blockMinutes { get; set; }
        public int taxiOutMinutes { get; set; }
    }

    public class Airportresources
    {
        public string departureTerminal { get; set; }
        public string departureGate { get; set; }
        public string arrivalTerminal { get; set; }
    }

    public class Flightequipment
    {
        public string scheduledEquipmentIataCode { get; set; }
        public string actualEquipmentIataCode { get; set; }
        public string tailNumber { get; set; }
    }

    public class Codeshare
    {
        public string fsCode { get; set; }
        public string flightNumber { get; set; }
        public string relationship { get; set; }
    }

}