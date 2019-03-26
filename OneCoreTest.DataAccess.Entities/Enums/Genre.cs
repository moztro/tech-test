using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCoreTest.DataAccess.Entities.Enums
{
    public enum Genre
    {
        [Display(Name = "Masculino")]
        Male,
        [Display(Name = "Femenino")]
        Female
    }
}
