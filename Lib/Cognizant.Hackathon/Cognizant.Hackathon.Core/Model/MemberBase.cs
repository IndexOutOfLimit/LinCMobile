using Cognizant.Hackathon.Core.Common.Enum;
using Cognizant.Hackathon.Core.Model.Attributes;
using Cognizant.Hackathon.Core.Model.Interface;
using Cognizant.Hackathon.Core.Model.Internal;
using Newtonsoft.Json;

namespace Cognizant.Hackathon.Core.Model
{
     /// <inheritdoc />
    /// <summary>
    ///     The member.
    /// </summary>
    public partial class MemberBase : CoreObject, IMember
    {
        #region Public Properties

        /// <inheritdoc />
        /// <summary>
        ///     Gets or sets FirstName.
        /// </summary>
        public virtual string FirstName { get; set; }

        /// <summary>
        ///     Gets the full name.
        /// </summary>
        [JsonIgnore]
        public virtual string FullName
        {
            get => FirstName + " " + LastName;
            set
            {
                // do nothing
                var dummy = value;
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Gets or sets LastName.
        /// </summary>
        public virtual string LastName { get; set; }


        // <summary>
        /// <summary>
        ///     Gets or sets the middle name.
        /// </summary>
        //public string MiddleName { get; set; }

        //[JsonSerialise(UserRole.None)]
        //public virtual string NewPassword { get; set; }

        /// <inheritdoc />
        /// <summary>
        ///     Gets or sets Password.
        ///     This is only used for fake repository use and repository population, not production!
        /// </summary>
        //[JsonSerialise(UserRole.None)]
        //public virtual string Password { get; set; }

        /// <summary>
        ///     Gets or sets Title.
        /// </summary>
        [JsonIgnore]
        public virtual string Title { get; set; }

        /// <inheritdoc />
        /// <summary>
        ///     Gets or sets Text.
        /// </summary>
        public virtual string UserName { get; set; }

        #endregion
    }
}