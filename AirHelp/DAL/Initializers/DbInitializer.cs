using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirHelp.DAL.Initializers
{
	internal class DbInitializer: MigrateDatabaseToLatestVersion<AirHelpDBContext, AirHelp.DAL.Migration.Configuration>
	{
	}
}
