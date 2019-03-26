using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCoreTest.Services.Infrastructure.Auditory
{
    /// <summary>
    /// Defines an entity that have a creation record.
    /// </summary>
    public interface ICreatable
    {
        /// <summary>
        /// Gets or sets the user who creates the record.
        /// </summary>
        string CreatedBy { get; set; }
        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        System.DateTime? CreatedDate { get; set; }
    }
}
