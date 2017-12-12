using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.DAL.Initializers
{
	internal class DbInitializer: MigrateDatabaseToLatestVersion<AirHelpDBContext, Recipes.DAL.Migration.Configuration>
	{
	}
}
