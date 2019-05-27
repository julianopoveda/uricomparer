using System;

namespace UriComparer
{
    public class UriComparer
    {
        public Uri UriTemplate { get; }
        public UriComparer(string uriTemplate)
        {
            if (string.IsNullOrEmpty(uriTemplate))
                throw new ArgumentNullException("uriTemplate", "uriTemplate must not be null or empty.");

            UriTemplate = new Uri(uriTemplate);
        }

        public bool Match(Uri urlToCompare)
        {
            bool isMatch = UriTemplate.Authority.Equals(urlToCompare.Authority);
            if (!isMatch)
                return false;

            string[] urlToCompareSegments = urlToCompare.Segments;
            string[] urlTemplateSegments = UriTemplate.Segments;

            int segmentsCount = urlToCompareSegments.Length > urlTemplateSegments.Length ? urlTemplateSegments.Length : urlToCompareSegments.Length;


            //here isMatch certainly is true;
            for (int i = 0; i < segmentsCount && isMatch; i++)
            {
                isMatch &= (urlTemplateSegments[i].Equals(urlToCompareSegments[i], StringComparison.InvariantCultureIgnoreCase));
                if (!isMatch)
                {
                    isMatch = urlTemplateSegments[i] == "*";
                    if (isMatch)
                        return true;
                }
            }

            if (isMatch)
            {
                string[] urlTemplateQueryParts = UriTemplate.Query.Split("&");
                string[] urlToCompareQueryParts = urlToCompare.Query.Split("&");

                int queryPartCount = urlTemplateQueryParts.Length > urlTemplateQueryParts.Length ? urlTemplateQueryParts.Length : urlTemplateQueryParts.Length;
                bool queryIsMatch = UriTemplate.Query.Equals(urlToCompare.Query, StringComparison.InvariantCultureIgnoreCase);
                if (queryIsMatch)
                    return queryIsMatch;

                queryIsMatch = true;
                for (int i = 0; i < queryPartCount && queryIsMatch; i++)
                {
                    queryIsMatch &= (urlTemplateQueryParts[i].Equals(urlToCompareQueryParts[i], StringComparison.InvariantCultureIgnoreCase));
                }

                return isMatch && queryIsMatch;
            }

            return isMatch;
        }

    }
}
