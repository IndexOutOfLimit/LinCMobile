using System;
using System.Collections.Generic;
using System.Text;

namespace Cognizant.Hackathon.RestClient.Infrastructure
{
    /// <summary>
    ///     The verb
    /// </summary>
    public enum HttpVerb
    {
        /// <summary>
        ///     The GET.
        /// </summary>
        GET,

        /// <summary>
        ///     The POST.
        /// </summary>
        POST,

        /// <summary>
        ///     The DELETE.
        /// </summary>
        DELETE,

        /// <summary>
        ///     The PUT.
        /// </summary>
        PUT,

        /// <summary>
        ///     HEAD
        /// </summary>
        HEAD,

        /// <summary>
        ///     OPTIONS
        /// </summary>
        OPTIONS,

        /// <summary>
        ///     PATCH
        /// </summary>
        PATCH,
    }

    public enum HttpParamMode
    {
        REST,
        QUERYSTRING,
        BODY,
        XFORM,
        RESTVALUE,
        MULTIPART
    }

    public enum ServiceStatus
    {
        /// <summary>
        ///     The success.
        /// </summary>
        Success,

        /// <summary>
        ///     The error.
        /// </summary>
        Error,
    }

    public enum MetaData
    {
        MimeType,
        FileName,
        ParamMode
    }
}
