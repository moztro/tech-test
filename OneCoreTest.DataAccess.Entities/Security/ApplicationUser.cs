using OneCoreTest.DataAccess.Entities.Enums;
using OneCoreTest.Services.Infrastructure.Auditory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCoreTest.DataAccess.Entities.Security
{
    [Table("ApplicationUsers")]
    public class ApplicationUser: ICreatable, IUpdatable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        [Required]
        public string Email { get; set; }
        
        [MinLength(7)]
        [Required]
        public string Username { get; set; }

        public string Password { get; set; }

        public bool Status { get; set; }

        public Genre Genre { get; set; }

        #region ICreatable implementation
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        #endregion ICreatable implementation

        #region IUpdatable implementation
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        #endregion IUpdatable implementation
    }
}
