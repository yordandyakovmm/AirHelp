using AirHelp.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHelp.Models
{
    public enum ProblemType {
        Pending = 0,
        Delay = 1,
        Cancel = 2,
        Overbooking = 3
    }

    public enum ClaimStatus
    {
        WaitForDocument = 1,
        Accepted = 2,
        InProgress = 3,
        Compleeted = 4, 
        Rejected = 5
    }

    public enum Reason
    {
        empty = -1,
        TechnicalIssue = 0,
        InfuenceOtherFlight = 1,
        Strike = 2,
        BadWeather = 3,
        AirportFault = 4,
        WithoutReason = 5

    }

    public enum DelayDelay
    {
        empty = -1,
        Beetwen0_2 = 0,
        Beetwen2_3 = 1,
        Beetwen3_4 = 2,
        MoreThan4 = 3,
    }

    public enum CancelAnnonsment
    {
        empty = -1,
        MoreThan14 = 0,
        Beetwen7_14 = 1,
        LessThat7 = 2
    }

    public enum CancelOverbokingDelay
    {
        empty = -1,
        Beetwen0_2 = 0,
        Beetwen2_3 = 1,
        Beetwen3_4 = 2,
        MoreThan4 = 3,
        NotArrive = 4

    }

    public enum DenayArival
    {
        empty = -1,
        Before30 = 0,
        After30 = 1
    }

    public enum DocumentSecurity
    {
        empty = -1,
        MyFault = 0,
        NotMyFault = 1
    }

    public enum Willness
    {
        empty = -1,
        Voluntary = 0,
        NotVoluntary = 1
    }

    public enum IsEUFlight
    {
        EU = 0,
        NonEU = 1,
        EUMixed =2
    }

    public enum FlightType
    {
        NotSupported = -1,
        F1500 = 0,
        FTo3500 = 1,
        FmoreThen3500 = 2
        
    }

}