using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.DAL
{
	public class Measure:EntityBase
	{
		public string MeasureId { get; set; }
		public string MeasureName { get; set; }
		public string MeasureType { get; set; }
		public virtual ICollection<Form> Forms { get; set; }
	}
}
