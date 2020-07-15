using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cognizant.Hackathon.Shared.Mobile.Models.Models;
namespace LinC.Platforms
{
    public interface IContactsService
    {
        Task<IList<Contact>> RetrieveContactsAsync(CancellationToken? token = null);
        Task<bool> CheckContactPermissionStatus();
    }
}
