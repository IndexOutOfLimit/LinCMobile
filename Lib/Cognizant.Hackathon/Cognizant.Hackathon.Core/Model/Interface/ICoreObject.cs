using System;

namespace Cognizant.Hackathon.Core.Model.Interface
{
    /// <summary>
    ///     The CoreObject interface.
    /// </summary>
    public interface ICoreObject
    {
        /// <summary>
        ///     Gets or sets object id.
        /// </summary>
        /// <value>
        ///     From object  id.
        /// </value>
        Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the memberId that created this object
        /// </summary>
        Guid? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the member username that created this object
        /// </summary>
        string CreatedByUsername { get; set; }

        /// <summary>
        /// Gets or sets the updated.
        /// </summary>
        DateTime? Updated { get; set; }
    }
}
