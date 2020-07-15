namespace Cognizant.Hackathon.Core.Model
{
    public static class Extensions
    {
        public static string TrimString(this string value)
        {
            if (!string.IsNullOrEmpty(value))
                value = value.Trim();

            return value;
        }

        public static string TrimLowerString(this string value)
        {
            if (!string.IsNullOrEmpty(value))
                value = value.Trim().ToLower();

            return value;
        }
    }
}
