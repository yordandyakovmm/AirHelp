
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.DAL
{
	public class Form:EntityBase
	{
		public Guid StudyId { get; set;}

		[ForeignKey("StudyId")]
		public virtual Study Study { get; set; }

		public Guid MeasureId { get; set;}

		[ForeignKey("MeasureId")]
		public virtual Measure Measure { get; set; }
        public int? TimeToggle { get; set; }
        public string FormId { get; set; }
        public string  p { get; set; }
		
	}
}
