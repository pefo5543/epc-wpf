using System;

namespace EpcDashboard.Helpers
{
    public static class EBMSHelper
    {
        public static bool IsValidUrl(this string source)
        {
            Uri uriResult;
            return Uri.TryCreate(source, UriKind.Absolute, out uriResult) && uriResult.Scheme == Uri.UriSchemeHttp;
        }

        public static void OpenUriInBrowser(string uri)
        {
            System.Diagnostics.Process.Start(uri);
        }
    }
}
