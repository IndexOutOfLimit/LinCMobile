namespace Cognizant.Hackathon.Shared.Mobile.Core.Helpers
{
    /// <summary>
    /// Abstracting this layer separately for Android since the
    /// CrossCurrentActivity plugin doesn't support dot net standard projects
    /// </summary>
    public static class CrossCurrentActivity
    {
        public static object Current { get; set; }
    }
}
