using System;
using System.ComponentModel.DataAnnotations;
using Cognizant.Hackathon.Core.Common.Enum;
using Cognizant.Hackathon.Core.Model.Attributes;
using Cognizant.Hackathon.Core.Model.Interface;

namespace Cognizant.Hackathon.Core.Model.Internal
{
    /// <summary>
    ///     Base Object
    /// </summary>
    public abstract class CoreObject : object, ICoreObject
    {
        /// <summary>
        ///     Gets or sets object id.
        /// </summary>
        /// <value>
        ///     From object  id.
        /// </value>
        [Key]
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the memberId that created this object
        /// </summary>
        [JsonSerialise(UserRole.Realogy)]
        public Guid? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the member username that created this object
        /// </summary>
        [JsonSerialise(UserRole.Realogy)]
        public string CreatedByUsername { get; set; }

        /// <summary>
        /// Gets or sets the updated.
        /// </summary>
        [JsonSerialise(UserRole.Realogy)]
        public DateTime? Updated { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreObject"/> class.
        /// </summary>
        public CoreObject()
        {
            Id = Guid.NewGuid();
            Created = DateTime.UtcNow;
        }
    }
}