
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirHelp.DAL
{
	public class AdditionalUser : EntityBase
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set;}

        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public Guid ClaimId { get; set; }
        [ForeignKey("ClaimId")]
        public virtual Claim Claim { get; set; }

       

    }
}
