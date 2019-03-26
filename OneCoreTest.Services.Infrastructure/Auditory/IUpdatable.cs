using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCoreTest.Services.Infrastructure.Auditory
{
    /// <summary>
    /// Defines an entity that have an update record.
    /// </summary>
    public interface IUpdatable
    {
        /// <summary>
        /// Gets or sets the user who updated the record.
        /// </summary>
        string UpdatedBy { get; set; }
        /// <summary>
        /// Gets or sets the updated date. 
        /// </summary>
        System.DateTime? UpdatedDate { get; set; }
    }
}
