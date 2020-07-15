using System;
using System.IO;

namespace Cognizant.Hackathon.Common.Helpers
{
    using Newtonsoft.Json;
    internal class CommonTextWriter : JsonTextWriter, IDisposable
    {
        public CommonTextWriter(TextWriter textWriter) : base(textWriter)
        {
        }

        public override void WriteValue(double value)
        {
            //double minValue = 0.01;
            //value = Math.Max(value, minValue);
            var sOut = String.Format("{0:0.00}", value);
            base.WriteValue(sOut);
        }

        public void Dispose()
        {
            // do nothing
        }
    }
}
