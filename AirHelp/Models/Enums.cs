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

    public enum emType
    {
        Pending = 0,
        Delay = 1,
        Cancel = 2,
        Overbooking = 3
    }

    public enum Reason
    {
        TechnicalIssue = 0,
        InfuenceOtherFlight = 1,
        Strike = 2,
        BadWeather = 3,
        AirportFault = 4,
        WithoutReason = 5

    }

    public enum Delay
    {
        LessThat3 = 0,
        MoreThat3 = 1,
        MoreThan4 = 2
    }

    public enum CancelAnnonsment
    {
        MoreThan14 = 0,
        Beetwen7_14 = 1,
        LessThat7 = 2
    }

    public enum CancelDelay
    {
        Beetwen0_2 = 0,
        Beetwen2_3 = 1,
        Beetwen3_4 = 2,
        MoreThan4 = 3
    }

    public enum DenayArival
    {
        Before30 = 0,
        After30 = 1
    }

    public enum DocumentSecurity
    {
        MyFault = 0,
        NotMyFault = 1
    }

    public enum Willness
    {
        Voluntary = 0,
        NotVoluntary = 1
    }
}