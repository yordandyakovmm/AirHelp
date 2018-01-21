using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AirHelp.DAL;
using AirHelp.Models;

namespace AirHelp.Hellpers
{
    public class UserHeppler
    {
        public static VMUser GetUserById(string userID)
        {
            VMUser user = null;
            using (AirHelpDBContext context = new AirHelpDBContext())
            {
                var userDB = context.Users.Where(u => u.UserId == userID).SingleOrDefault();
                if (userDB != null)
                {
                    user = new VMUser()
                    {
                        UserId = userDB.UserId,
                        FirstName = userDB.FirstName,
                        LastName = userDB.LastName,
                        Email = userDB.Email,
                        PictureUrl = userDB.PictureUrl,
                        Role = userDB.Role
                    };
                }
            }
            return user;
        }

        public static VMUser SyncUserToDatabase(VMUser user)
        {
            user.PictureUrl = user.PictureUrl ?? "";
            user.FirstName = user.FirstName ?? "";
            user.LastName = user.LastName ?? "";
            user.Email = user.Email ?? "";
            user.Role = user.Role ?? "";

            using (AirHelpDBContext context = new AirHelpDBContext())
            {
                var userDB = context.Users.Where(u => u.UserId == user.UserId).SingleOrDefault();
                if (userDB != null)
                {
                    user.Role = userDB.Role;
                    return user;
                }
                user.Role = "";
                var newUserBD = new User()
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PictureUrl = user.PictureUrl,
                    CreateDate = DateTime.Now,
                    Role = user.Role
                };

                context.Users.Add(newUserBD);
                context.SaveChanges();
                return user;
            }
        }
        
    }
}