using Facebook;
using Newtonsoft.Json.Linq;
using AirHelp.DAL;
using AirHelp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Threading;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Script.Serialization;
using System.Device.Location;
using AirHelp.Hellpers;

namespace AirHelp.Controllers
{


    public class ClaimController : BaseController
    {

        [HttpGet]
        [Route("калкулиране-на-обезщетение")]
        public ActionResult ColectFlightDataFlight()
        {
            var model = new VMDirectFlight();
            return View("ColectFlightDataFlight", model);
        }

        // ----- 1 ---------------
        [HttpPost]
        [Route("калкулиране-на-обезщетение")]
        public ActionResult ColectFlightDataFlightPost()
        {

            Guid newGuid = Guid.NewGuid();

            var jsonString = Request.Form["jsonAirport"];
            var issueDepartureCode = Request.Form["Flight"];
            var nubmer = Request.Form["FlightNumber"];
            var nubmers = Request.Form["FlightNumbers"];
            var date = Request.Form["Date"];
            var dates = Request.Form["Dates"];

            string[] flightNumbers = nubmers != null ? nubmers.Split(',') : new string[] { nubmer, "" };
            string[] flightDates = dates != null ? dates.Split(',') : new string[] { date, "" };

            var json = new JavaScriptSerializer();
            Airport[] airports = json.Deserialize<Airport[]>(jsonString);

            Claim claim = new Claim
            {
                ClaimId = Guid.NewGuid(),
                State = ClaimStatus.WaitForDocument,
                UserId = null,
                DateCreated = DateTime.Now,
                Type = ProblemType.Pending
            };

            if (issueDepartureCode == null)
            {
                issueDepartureCode = airports[0].iata;
            }

            int number = 1;
            airports.ToList().ForEach(a =>
            {
                AirPort airport = new AirPort
                {
                    Id = Guid.NewGuid(),
                    ap_name = a.ap_name,
                    city = a.city,
                    country = a.country,
                    elevation = int.Parse(a.elevation),
                    iata = a.iata,
                    number = number,
                    name = a.name,
                    timezone = double.Parse(a.timezone),
                    icao = a.icao,
                    type = a.type,
                    x = double.Parse(a.x),
                    y = double.Parse(a.y),
                    FlightNumber = airports.Length == number ? (nubmer != null ? nubmer : "") : flightNumbers[number - 1],
                    FlightDate = airports.Length == number ? (date != null ? date : "") : flightDates[number - 1],
                    startIssue = (number == 1 && airports.Length == 2) || (a.iata == issueDepartureCode)
                };
                number++;
                claim.AirPorts.Add(airport);
            });

            double allDistance = 0;
            double issuDistance = 0;
            bool flag = false;
            var claimAirPorts = claim.AirPorts.ToArray();
            for (int i = 0; i < claimAirPorts.Length - 1; i++)
            {
                var sCoord = new GeoCoordinate(claimAirPorts[i].y, claimAirPorts[i].x);
                var eCoord = new GeoCoordinate(claimAirPorts[i + 1].y, claimAirPorts[i + 1].x);

                var d = sCoord.GetDistanceTo(eCoord);
                claimAirPorts[i].distanceToNext = d;
                allDistance += d;
                flag = flag || (claimAirPorts[i].iata == issueDepartureCode);
                if (flag)
                {
                    issuDistance += d;
                }

            }

            claim.issueDistance = issuDistance;
            claim.allDistance = allDistance;

            // Check EU flagth
            IsEUFlight isEUFlight = IsEUFlight.NonEU;
            if (claim.AirPorts.All(a => CommonHeppler.IsEuCountryByName(a.country)))
            {
                isEUFlight = IsEUFlight.EU;
            }
            else if (claim.AirPorts.Any(a => CommonHeppler.IsEuCountryByName(a.country)))
            {
                isEUFlight = IsEUFlight.EUMixed;
            }

            double distance = (claim.Type == ProblemType.Delay) ? claim.allDistance : claim.issueDistance;

            FlightType flightType = FlightType.NotSupported;

            if (isEUFlight == IsEUFlight.NonEU)
            {
                flightType = FlightType.NotSupported;
            }
            else if (distance <= 1500000)
            {
                flightType = FlightType.F1500;
            }
            else if (distance <= 3500000 || isEUFlight == IsEUFlight.EU)
            {
                flightType = FlightType.FTo3500;
            }
            else
            {
                flightType = FlightType.FmoreThen3500;
            }

            claim.IsEUFlight = isEUFlight;
            claim.FlightType = flightType;

            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                dc.Claims.Add(claim);
                dc.SaveChanges();
            }

            return View("ColectData", claim);

        }


        // ----- 1 ---------------
        [HttpGet]
        [Route("проверка-полет")]
        public ActionResult DirctFlight()
        {
            var model = new VMDirectFlight();
            return View("DirectFlight", model);
        }


        // ----- 2 ---------------
        [HttpPost]
        [Route("описание-на-проблема")]
        public ActionResult IssueData()
        {

            Guid ClaimId = Guid.Parse(Request.Form["ClaimID"]);
            ProblemType Type = (ProblemType)(int.Parse(Request.Form["Type"] ?? "-1"));
            Reason Reason = (Reason)(int.Parse(Request.Form["Reason"] ?? "-1"));
            DelayDelay DelayDelay = (DelayDelay)(int.Parse(Request.Form["DelayDelay"] ?? "-1"));
            CancelAnnonsment CancelAnnonsment = (CancelAnnonsment)(int.Parse(Request.Form["CancelAnnonsment"] ?? "-1"));
            CancelOverbokingDelay CancelOverbokingDelay = (CancelOverbokingDelay)(int.Parse(Request.Form["CancelOverbokingDelay"] ?? "-1"));
            DenayArival DenayArival = (DenayArival)(int.Parse(Request.Form["DenayArival"] ?? "-1"));
            DocumentSecurity DocumentSecurity = (DocumentSecurity)(int.Parse(Request.Form["DocumentSecurity"] ?? "-1"));
            Willness Willness = (Willness)(int.Parse(Request.Form["Willness"] ?? "-1"));

            Claim claim = null;

            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                claim = dc.Claims.Include("AirPorts").Where(c => c.ClaimId == ClaimId).SingleOrDefault();

                var isEUFlight = claim.IsEUFlight;
                var flightType = claim.FlightType;

                claim.Type = Type;
                claim.Reason = Reason;
                claim.DelayDelay = DelayDelay;
                claim.CancelAnnonsment = CancelAnnonsment;
                claim.CancelOverbokingDelay = CancelOverbokingDelay;
                claim.DenayArival = DenayArival;
                claim.DocumentSecurity = DocumentSecurity;
                claim.Willness = Willness;

                // reject by not in EU
                if (flightType == FlightType.NotSupported)
                {
                    RelectClaim model = new RelectClaim()
                    {
                        Reason = "Полета е изцяло извън рамките на Европейският съюз. Регламент 261/2004 не покрива полети извън EU"
                    };
                    return View("RejectClaim", model);
                }

                // Reject by reason 
                if (Reason == Reason.Strike || Reason == Reason.BadWeather)
                {
                    RelectClaim model = new RelectClaim()
                    {
                        Reason = "Причини за проблем с полета \"Стачка\" или \"Лошо време\" се водят форсмажорни обстоятрлства. " +
                        "Авиокомпанията не дължи обезщетение по регламент 261/2004"
                    };
                    return View("RejectClaim", model);
                }

                if (Type == ProblemType.Delay)
                {
                    // Reject less than 2 hours
                    if (flightType == FlightType.F1500 && DelayDelay < DelayDelay.Beetwen2_3)
                    {
                            RelectClaim model = new RelectClaim()
                            {
                                Reason = "Закъснението на полети до 1500 км трябва да е повече от 2 часа."
                            };
                            return View("RejectClaim", model);
                    }
                    // Reject less than 3 hours
                    if (flightType > FlightType.F1500 && DelayDelay < DelayDelay.Beetwen3_4)
                    {
                        RelectClaim model = new RelectClaim()
                        {
                            Reason = "Закъснението на полети над 1500 км трябва да е повече от 3 часа."
                        };
                        return View("RejectClaim", model);

                    }
                    if (flightType == FlightType.F1500 && DelayDelay > DelayDelay.Beetwen0_2)
                    {
                        claim.CompensationAmount = 250;
                        claim.CompensationReason = "Закъснение с повече от 2 часа за полет с дистанция до 1500 км.";
                    }
                    else if (flightType == FlightType.FTo3500)
                    {
                        claim.CompensationAmount = 400;
                        claim.CompensationReason = "Закъснял полет с дистанция до 3500 км или в ранмите на EU";
                    }
                    else if (flightType == FlightType.FmoreThen3500 && DelayDelay == DelayDelay.Beetwen3_4)
                    {
                        claim.CompensationAmount = 300;
                        claim.CompensationReason = "Закъснял полет с дистанция над 3500 км. Ако полета заъснее с повече от 3 и по-малко от 4 часа, обезщетението се намалява с 50 %";
                    }
                    else
                    {
                        claim.CompensationAmount = 600;
                        claim.CompensationReason = "Закъснял полет с дистанция над 3500 км със закъснение над 4 часа.";
                    }

                }

                if (Type == ProblemType.Cancel)
                {
                    if (CancelAnnonsment == CancelAnnonsment.MoreThan14)
                    {
                        RelectClaim model = new RelectClaim()
                        {
                            Reason = "Авиокомпанията не дължи обезщетение при опоестяване 2 седмици преди отмяната на полета."
                        };
                        return View("RejectClaim", model);
                    }

                    if (CancelAnnonsment == CancelAnnonsment.Beetwen7_14 && CancelOverbokingDelay < CancelOverbokingDelay.MoreThan4)
                    {
                        RelectClaim model = new RelectClaim()
                        {
                            Reason = "При отмяна на полета с оповестяване на отмяната повече от 7 дни преди полета е нужно закъснение от поне 4 часа."
                        };
                        return View("RejectClaim", model);
                    }

                    if (CancelAnnonsment == CancelAnnonsment.LessThat7 && CancelOverbokingDelay < CancelOverbokingDelay.Beetwen2_3)
                    {
                        RelectClaim model = new RelectClaim()
                        {
                            Reason = "При отмяна на полета с оповестяване на отмяната по-малко от 7 дни преди полета е нужно закъснение от поне 2 часа."
                        };
                        return View("RejectClaim", model);
                    }

                    // cut off compensation 
                    if (flightType == FlightType.F1500 && CancelOverbokingDelay == CancelOverbokingDelay.Beetwen0_2)
                    {
                        claim.CompensationAmount = 125;
                        claim.CompensationReason = "Отмяна на полета или откзан достъп до борда за полет " +
                            "до 1500 км и премаршрутизиране със закъснение до 2 ч. имат 50 % намеление на обезщетението.";

                    }

                    else if (flightType == FlightType.FTo3500 && CancelOverbokingDelay <= CancelOverbokingDelay.Beetwen2_3)
                    {
                        claim.CompensationAmount = 200;
                        claim.CompensationReason = "Отмяна на полета или откзан достъп до борда за полет " +
                            "до 3500 км и премаршрутизиране със закъснение до 3 ч. имат 50 % намеление на обезщетението.";

                    }
                    else if (flightType == FlightType.FmoreThen3500 && CancelOverbokingDelay <= CancelOverbokingDelay.Beetwen3_4)
                    {
                        claim.CompensationAmount = 300;
                        claim.CompensationReason = "Отмяна на полета или откзан достъп до борда за полет " +
                            "над 3500 км и премаршрутизиране със закъснение до 4 ч. имат 50 % намеление на обезщетението.";

                    }
                    else if (flightType == FlightType.F1500)
                    {
                        claim.CompensationAmount = 250;
                        claim.CompensationReason = "Отменен полет до 1500 км";

                    }
                    else if (flightType == FlightType.FTo3500)
                    {
                        claim.CompensationAmount = 400;
                        claim.CompensationReason = "Отменен полет до 3500 км";

                    }
                    else if (flightType == FlightType.FmoreThen3500)
                    {
                        claim.CompensationAmount = 600;
                        claim.CompensationReason = "Отменен полет над 3500 км";

                    }

                }
                if (Type == ProblemType.Overbooking)
                {
                    if (DenayArival == DenayArival.After30)
                    {
                        RelectClaim model = new RelectClaim()
                        {
                            Reason = "Причина за отказа е неявяване на чек-ина 30 мин преди полета."
                        };
                        return View("RejectClaim", model);
                    }

                    if (DocumentSecurity == DocumentSecurity.MyFault)
                    {
                        RelectClaim model = new RelectClaim()
                        {
                            Reason = "При проблем със документите нямате право на обезщетение."
                        };
                        return View("RejectClaim", model);
                    }
                    if (Willness == Willness.Voluntary)
                    {
                        RelectClaim model = new RelectClaim()
                        {
                            Reason = "При доброволно педосъпване място си, нямате право на обезщетение по член 7 от регламент 261/2004."
                        };
                        return View("RejectClaim", model);
                    }
                    // cut off compensation 
                    if (flightType == FlightType.F1500 && CancelOverbokingDelay == CancelOverbokingDelay.Beetwen0_2)
                    {
                        claim.CompensationAmount = 125;
                        claim.CompensationReason = "Отмяна на полета или откзан достъп до борда за полет " +
                            "до 1500 км и премаршрутизиране със закъснение до 2 ч. имат 50 % намеление на обезщетението.";

                    }

                    else if (flightType == FlightType.FTo3500 && CancelOverbokingDelay <= CancelOverbokingDelay.Beetwen2_3)
                    {
                        claim.CompensationAmount = 200;
                        claim.CompensationReason = "Отмяна на полета или откзан достъп до борда за полет " +
                            "до 3500 км и премаршрутизиране със закъснение до 3 ч. имат 50 % намеление на обезщетението.";

                    }
                    else if (flightType == FlightType.FmoreThen3500 && CancelOverbokingDelay <= CancelOverbokingDelay.Beetwen3_4)
                    {
                        claim.CompensationAmount = 300;
                        claim.CompensationReason = "Отмяна на полета или откзан достъп до борда за полет " +
                            "над 3500 км и премаршрутизиране със закъснение до 4 ч. имат 50 % намеление на обезщетението.";

                    }
                    else if (flightType == FlightType.F1500)
                    {
                        claim.CompensationAmount = 250;
                        claim.CompensationReason = "Отменен полет до 1500 км";

                    }
                    else if (flightType == FlightType.FTo3500)
                    {
                        claim.CompensationAmount = 400;
                        claim.CompensationReason = "Отменен полет до 3500 км";

                    }
                    else if (flightType == FlightType.FmoreThen3500)
                    {
                        claim.CompensationAmount = 600;
                        claim.CompensationReason = "Отменен полет над 3500 км";

                    }

                }

                dc.SaveChanges();

            }

            return View("CalculatorResult", claim);

        }

        // ----- 3 ---------------
        [HttpPost]
        [Route("регистриране-на-потребител")]
        public ActionResult RegisterUserToClaim()
        {
            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                var claimId = Guid.Parse(Request.Form["claimId"]);
                var FirstName = Request.Form["FirstName"];
                var LastName = Request.Form["LastName"];
                var City = Request.Form["City"];
                var Country = Request.Form["Country"];
                var Email = Request.Form["Email"];
                var Adress = Request.Form["Adress"];
                var Tel = Request.Form["Tel"];
                var Password = Request.Form["Password"];


                var claim = dc.Claims.Where(c => c.ClaimId == claimId).SingleOrDefault();

                var newUserBD = new User()
                {
                    UserId = Email,
                    FirstName = FirstName,
                    LastName = LastName,
                    City = City, 
                    Adress = Adress, 
                    Country = Country,
                    Tel = Tel,
                    Email = Email,
                    password = GetHash(Password),
                    PictureUrl = "",
                    CreateDate = DateTime.Now,
                    Role = "user"
                };

                dc.Users.Add(newUserBD);
                claim.UserId = newUserBD.UserId;
                dc.SaveChanges();

                return View("ConfirmClaim", claim);

            }

        }

        // ----- 4 ---------------
        [HttpGet]
        [Route("потвърждение-на-иск/{claimId}")]
        public ActionResult ConfirmClaim(string claimId)
        {
            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                var ClaimId = Guid.Parse(claimId);
                var claim = dc.Claims.Include("User").Where(c => c.ClaimId == ClaimId).SingleOrDefault();
                return View("ConfirmClaim", claim);
            }
        }


        // ----- 4 ---------------
        [HttpPost]
        [Route("потвърждение-на-иск")]
        public ActionResult ConfirmClaimPost(IEnumerable<HttpPostedFileBase> UserFiles)
        {
            using (AirHelpDBContext dc = new AirHelpDBContext())
            {

                var BookCode = Request.Form["BookCode"];
                var TikedNumber = Request.Form["TikedNumber"];
                var AdditionalInfo = Request.Form["AdditionalInfo"];
                var claimId = Request.Form["claimId"];
                var SignitureImage = Request.Form["SignitureImage"];
                
                var ClaimId = Guid.Parse(claimId);
                var claim = dc.Claims
                    .Include("User")
                    .Include("Documents")
                    .Include("AdditionalUsers")
                    .Include("AirPorts")
                    .Where(c => c.ClaimId == ClaimId).SingleOrDefault();

                claim.SignitureImage = SignitureImage;
                claim.AdditionalInfo = AdditionalInfo;
                claim.TikedNumber = TikedNumber;
                claim.BookingCode = BookCode;

                if (UserFiles != null)
                {
                    foreach (var file in UserFiles)
                    {

                        if (file != null && file.ContentLength > 0)
                        {
                            var name = Guid.NewGuid() + "." + file.FileName.Split('.')[1];
                            file.SaveAs(Server.MapPath("~/UserDocuments/" + name));
                            claim.Documents.Add(new Document
                            {
                                Id = Guid.NewGuid(),
                                DocumentName = file.FileName,
                                Url = "/UserDocuments/" + name
                            });
                        }
                    }
                }

                if (claim.Documents.Count > 0)
                {
                    claim.State = ClaimStatus.InProgress;
                }

                Guid newGuid = Guid.NewGuid();
                string AttorneyUrl = $"/UserDocuments/{newGuid}.pdf";



                string port = Request.Url.Port == 80 ? string.Empty : $":{Request.Url.Port.ToString()}";

                String url = $"{Request.Url.Scheme}://{Request.Url.Host}{port}/attorneyPdf/{claim.ClaimId}";

                SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();
                SelectPdf.PdfDocument doc = converter.ConvertUrl(url);
                doc.Save(Server.MapPath($"~/UserDocuments/{newGuid}.pdf"));
                doc.Close();
                claim.AttorneyUrl = $"/UserDocuments/{newGuid}.pdf";
                dc.SaveChanges();

                Session["claim"] = claim;

                return View("ViewClaim", claim);
            }
        }


        [Route("ъпдейт-на-иск")]
        [Route("ъпдейт-на-иск/{id}")]
        public ActionResult UpdateClaimPost(IEnumerable<HttpPostedFileBase> UserFiles,string id)
        {
            using (AirHelpDBContext dc = new AirHelpDBContext())
            {

                var claimId = Request.Form["claimId"] ?? id;

                var ClaimId = Guid.Parse(claimId);
                var claim = dc.Claims
                    .Include("User")
                    .Include("Documents")
                    .Include("AdditionalUsers")
                    .Include("AirPorts")
                    .Where(c => c.ClaimId == ClaimId).SingleOrDefault();

                if (UserFiles != null)
                {
                    foreach (var file in UserFiles)
                    {

                        if (file != null && file.ContentLength > 0)
                        {
                            var name = Guid.NewGuid() + "." + file.FileName.Split('.')[1];
                            file.SaveAs(Server.MapPath("~/UserDocuments/" + name));
                            claim.Documents.Add(new Document
                            {
                                Id = Guid.NewGuid(),
                                DocumentName = file.FileName,
                                Url = "/UserDocuments/" + name
                            });
                        }
                    }
                }

                if (claim.Documents.Count > 0)
                {
                    claim.State = ClaimStatus.InProgress;
                }

                dc.SaveChanges();
                return View("ViewClaim", claim);
            }

        }


        [Authorize(Roles = "admin,user")]
        [Route("обезщетение-списък")]
        public ActionResult ClaimList(string category)
        {
            var isAdmin = User.IsInRole("admin");
            List<Claim> list = null;
            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                list = dc.Claims
                    .Include("AirPorts")
                    .Where(c => c.Type != ProblemType.Pending && (c.UserId == User.Identity.Name || isAdmin) ).Select(c => c)
                    .ToList();
            }
            return View("ClaimList", list);
        }


















        [HttpPost]
        [Route("проверка-полет")]
        public ActionResult CheckDirctFlightPost(string FlightNumber, string Date)
        {
            // testing
            Claim c = null;
            Guid g = new Guid("ecbc3e69-71c0-4fab-971d-3acc3bd98480");
            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                c = dc.Claims.Where(cl => cl.ClaimId == g).SingleOrDefault();


                ViewBag.IsEu = c.AirPorts.Any(a => CommonHeppler.IsEuCountry(a.countryCode));
                ViewBag.delayMessage = (c.issueDistance < 1500000 ? "Полета ви закъсня ли с повече от 2 часа?" :
                    (c.issueDistance < 3500000 || ViewBag.IsEu ? "Полета ви закъсня ли с повече от 3 часа?" : "Полета ви закъсня ли с повече от 3 часа ?"));
            }
            return View("ColectData", c);

            var model = new VMDirectFlight()
            {
                date = Date,
                number = FlightNumber,
                numberError = ""
            };
            var arr = Date.Split('.');
            var day = int.Parse(arr[0]);
            var mont = int.Parse(arr[1]);
            var year = int.Parse(arr[2]);

            FlightStatus fligth = CommonHeppler.GetFlight(FlightNumber, Date);

            if (fligth.flightStatuses.Length == 0)
            {
                model.commonError = "Невалидна комбинция от номер и дата на полета";
                return View("DirectFlight", model);
            }

            var AirLine = fligth.appendix.airlines.ToList().Find(a => a.iata == fligth.flightStatuses[0].primaryCarrierFsCode);

            Claim claim = new Claim
            {
                ClaimId = Guid.NewGuid(),
                FlightDate = Date,
                State = ClaimStatus.Accepted,
                UserId = User.Identity.IsAuthenticated ? User.Identity.Name : null,
                DateCreated = DateTime.Now,
                Type = ProblemType.Pending,
                FlightNumber = FlightNumber,
                AirCompany = AirLine.name,
                AirCompanyCountry = ""
            };

            int number = 0;
            fligth.appendix.airports.ToList().ForEach(a =>
            {
                double distance = 0;
                if (number > 0)
                {
                    var sCoord = new GeoCoordinate(fligth.appendix.airports[number - 1].longitude, fligth.appendix.airports[number - 1].latitude);
                    var eCoord = new GeoCoordinate(fligth.appendix.airports[number].longitude, fligth.appendix.airports[number].latitude);

                    distance = sCoord.GetDistanceTo(eCoord);
                }
                AirPort airport = new AirPort
                {
                    Id = Guid.NewGuid(),
                    ap_name = a.name,
                    city = a.city,
                    cityCode = a.cityCode,
                    country = a.countryName,
                    countryCode = a.countryCode,
                    elevation = a.elevationFeet,
                    iata = a.iata,
                    number = number,
                    name = a.name,
                    timezone = 0,
                    icao = a.icao,
                    type = "",
                    x = a.latitude,
                    y = a.longitude,
                    distanceToNext = distance
                };
                number++;
                claim.AirPorts.Add(airport);
            });

            claim.allDistance = claim.AirPorts.Sum(a => a.distanceToNext);

            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                dc.Claims.Add(claim);
                dc.SaveChanges();
            }

            return View("ColectData", claim);
        }

        [HttpGet]
        [Route("обезщетение-при-полет/{category}")]
        public ActionResult RegisterClaim(string category)
        {
            ViewBag.category = category;
            return View("RegisterClaim");
        }

        [HttpPost]
        [Route("обезщетение-при-полет/{category}")]
        public ActionResult RegisterClaimSave(string category)
        {

            ViewBag.category = category;

            string BordCardUrl = "";
            string BookConfirmationUrl = "";

            if (Request.Files["BordCard"].ContentLength > 0)
            {
                var file = Request.Files["BordCard"];
                var name = Guid.NewGuid() + "." + file.FileName.Split('.')[1];
                BordCardUrl = $"/UserDocuments/{name}";
                file.SaveAs(Server.MapPath("~/UserDocuments/" + name));
            }
            if (Request.Files["BookConfirmation"].ContentLength > 0)
            {
                var file = Request.Files["BookConfirmation"];
                var name = Guid.NewGuid() + "." + file.FileName.Split('.')[1];
                BookConfirmationUrl = $"/UserDocuments/{name}";
                file.SaveAs(Server.MapPath("~/UserDocuments/" + name));
            }

            Guid newGuid = Guid.NewGuid();
            string AttorneyUrl = $"/UserDocuments/{newGuid}.pdf";

            var jsonString = Request.Form["json"];

            var json = new JavaScriptSerializer();
            Rootobject data = json.Deserialize<Rootobject>(jsonString);


            Claim claim = new Claim
            {
                ClaimId = Guid.NewGuid(),
                State = ClaimStatus.Accepted,

                UserId = null,
                DateCreated = DateTime.Now,


                Type = ProblemType.Cancel,
                FlightNumber = Request.Form["FlightNumber"],


                AirCompany = data.airline.al_name,
                AirCompanyCountry = data.airline.country,
                AdditionalInfo = Request.Form["AdditionalInfo"],
                Confirm = Request.Form["Confirm"],

                SignitureImage = Request.Form["SignitureImage"],
                AttorneyUrl = AttorneyUrl
            };

            int number = 1;
            data.airports.ToList().ForEach(a =>
            {
                AirPort airport = new AirPort
                {
                    Id = Guid.NewGuid(),
                    ap_name = a.ap_name,
                    city = a.city,
                    country = a.country,
                    elevation = int.Parse(a.elevation),
                    iata = a.iata,
                    number = number,
                    name = a.name,
                    timezone = double.Parse(a.timezone),
                    icao = a.icao,
                    type = a.type,
                    x = double.Parse(a.x),
                    y = double.Parse(a.y)
                };
                number++;

                claim.AirPorts.Add(airport);
            });

            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                dc.Claims.Add(claim);
                dc.SaveChanges();
            }

            string port = Request.Url.Port == 80 ? string.Empty : $":{Request.Url.Port.ToString()}";

            String url = $"{Request.Url.Scheme}://{Request.Url.Host}{port}/attorneyPdf/{claim.ClaimId}";

            SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();
            SelectPdf.PdfDocument doc = converter.ConvertUrl(url);
            doc.Save(Server.MapPath($"~/UserDocuments/{newGuid}.pdf"));
            doc.Close();

            LogWithUser("temp", "temp");

            return Redirect($"/обезщетение-списък/{claim.ClaimId}");

        }

        [Route("обезщетение-при-полет")]
        public ActionResult ClaimSpliter(string category)
        {
            return View("ClaimSpliter");
        }

        [HttpGet]
        [Route("обезщетение-списък/{id}")]
        [Authorize]
        public ActionResult ClaimDetail(Guid id)
        {

            Claim claim = null;
            List<AirPort> airPorts = null;
            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                claim = dc.Claims.Where(c => c.ClaimId == id).SingleOrDefault();

                airPorts = claim.AirPorts.OrderBy(a => a.number).ToList();

            }
            var model = new VMClaim(claim);


            model.totalDistance = 0;

            for (int i = 0; i < airPorts.Count - 1; i++)
            {
                var sCoord = new GeoCoordinate(airPorts[i].y, airPorts[i].x);
                var eCoord = new GeoCoordinate(airPorts[i + 1].y, airPorts[i + 1].x);

                var distance = sCoord.GetDistanceTo(eCoord);

                var AirportDistance = new AirportDistance
                {
                    From = $"{airPorts[i].name} ({airPorts[i].iata})",
                    To = $"{airPorts[i + 1].name} ({airPorts[i + 1].iata})",
                    distance = distance / 1000
                };
                model.totalDistance = model.totalDistance + distance / 1000;
                model.AirporstDistance.Add(AirportDistance);
            }

            model.rightOfCompensation = true;
            model.CompensationAmount = 250;
            if (model.totalDistance >= 3500)
            {
                model.CompensationAmount = 600;
            }
            else if (model.totalDistance >= 1500)
            {
                model.CompensationAmount = 450;
            }
            return View("ViewClaim", model);
        }

        [HttpPost]
        [Authorize(Roles = "admin,temp")]
        [Route("обезщетение-списък/{id}")]
        public ActionResult ClaimUpdate(Guid id)
        {
            Claim claim = null;
            using (AirHelpDBContext dc = new AirHelpDBContext())
            {
                claim = dc.Claims.Where(c => c.ClaimId == id).SingleOrDefault();

                var password = Request.Form["password"];
                var newUserBD = new User()
                {
                    UserId = claim.User.Email,
                    FirstName = claim.User.FirstName,
                    LastName = claim.User.LastName,
                    Email = claim.User.Email,
                    password = GetHash(password),
                    PictureUrl = "",
                    CreateDate = DateTime.Now,
                    Role = "user"
                };

                dc.Users.Add(newUserBD);
                claim.UserId = newUserBD.UserId;
                dc.SaveChanges();

            }

            return Redirect($"/обезщетение-списък/{claim.ClaimId}");

        }

        



    }
}