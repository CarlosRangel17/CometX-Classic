using System;

namespace CometX.Application.Extensions.General
{
    public static class ValidationExtension
    {
        public static bool CheckURLValid(this string url)
        {
            Uri uriResult;
            return Uri.TryCreate(url, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
