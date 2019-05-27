using System;

namespace UriComparer
{
    public class UriComparer
    {
        public Uri UriTemplate { get; }
        public UriComparer(string uriTemplate)
        {
            if(string.IsNullOrEmpty(uriTemplate))
                throw new ArgumentNullException("uriTemplate", "uriTemplate must not be null or empty.");

            UriTemplate = new Uri(uriTemplate);
        }

        public bool Match(Uri urlToCompare)
        {
            return UriTemplate == urlToCompare;
        }
    }
}
