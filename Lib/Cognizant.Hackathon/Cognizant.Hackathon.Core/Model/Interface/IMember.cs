using System.Collections.Generic;
using Cognizant.Hackathon.Core.Common.Enum;

namespace Cognizant.Hackathon.Core.Model.Interface
{
    /// <summary>
    ///     The i member.
    /// </summary>
    public interface IMember : ICoreObject
    {
        #region Public Properties
        
        /// <summary>
        ///     Gets or sets NewPassword.
        /// </summary>
        //string NewPassword { get; set; }

        /// <summary>
        ///     Gets or sets Password.
        /// </summary>
        //string Password { get; set; }

        /// <summary>
        ///     Gets or sets Text.
        /// </summary>
        string UserName { get; set; }
       
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        string FirstName { get; set; }

        /// <summary>
        ///     Gets or sets FirstName.
        /// </summary>
        string LastName { get; set; }
        
        #endregion
    }
}