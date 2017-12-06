
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.DAL
{
	public class Users: EntityBase
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserId { get; set;}

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PictureUrl { get; set; }
        
        public string Role { get; set; }

        public DateTime CreateDate { get; set; }

	}
}
