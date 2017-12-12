using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.DAL
{
	public class Study:EntityBase
	{
		public string StudyId { get; set; }
		public string StudyName { get; set; }
		public virtual ICollection<Form> Forms { get; set; }
	}
}
