using AirHelp.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirHelp.Models
{
    public enum ProblemType {
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
}