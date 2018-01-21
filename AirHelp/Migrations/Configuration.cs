using AirHelp.DAL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace AirHelp.DAL.Migration
{

    internal sealed class Configuration : DbMigrationsConfiguration<AirHelpDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AirHelpDBContext context)
        {
            base.Seed(context);

        }
    }
}
