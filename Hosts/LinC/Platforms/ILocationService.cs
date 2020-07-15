using System.Threading.Tasks;

namespace LinC.Platforms
{
    public interface ILocationService
    {
        /// <summary>
        /// Opens the settings.
        /// </summary>
        /// <returns>The settings.</returns>
        Task OpenSettings();

        /// <summary>
        /// Checks the permission.
        /// </summary>
        /// <returns><c>true</c>, if permission was checked, <c>false</c> otherwise.</returns>
        bool CheckPermission();

        /// <summary>
        /// Checks the version.
        /// </summary>
        /// <returns><c>true</c>, if version was checked, <c>false</c> otherwise.</returns>
        bool CheckVersion();
    }
}
