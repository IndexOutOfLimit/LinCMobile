using System;

namespace Cognizant.Hackathon.RestClient.Helpers
{
    public static class ExceptionHelpers
    {
        /// <summary>
        /// The get innermost exception message.
        /// </summary>
        /// <param name="ex">
        /// The ex.
        /// </param>
        /// <returns>
        /// The get innermost exception message.
        /// </returns>
        public static Exception GetInnermostException(this Exception ex)
        {
            while (true)
            {
                if (ex == null) return null;

                if (ex.InnerException == null) return ex;

                ex = ex.InnerException;
            }
        }

        /// <summary>
        /// The get innermost exception message.
        /// </summary>
        /// <param name="ex">
        /// The ex.
        /// </param>
        /// <returns>
        /// The get innermost exception message.
        /// </returns>
        public static string GetInnermostExceptionMessage(this Exception ex)
        {
            while (true)
            {
                if (ex == null) return string.Empty;

                if (ex.InnerException == null) return ex.Message;
                ex = ex.InnerException;
            }
        }

        public static bool HasInnerExceptions(this Exception ex)
        {
            return ex.InnerException != null;
        }
    }
}
