using System;

namespace UriComparer
{
    public class UriComparer
    {
        private enum UriMatchLevel : short
        {
            Match,
            NotMatch,
            AllSubUrlAllowed
        }

        public Uri UriTemplate { get; }
        public UriComparer(string uriTemplate)
        {
            if (string.IsNullOrEmpty(uriTemplate))
                throw new ArgumentNullException("uriTemplate", "uriTemplate must not be null or empty.");

            UriTemplate = new Uri(uriTemplate);
        }

        public bool Match(Uri urlToCompare)
        {
            if (!UriTemplate.Authority.Equals(urlToCompare.Authority))
                return false;

            //here isMatch certainly is true;
            UriMatchLevel isMatch = CompareSegments(UriTemplate.Segments, urlToCompare.Segments);

            if (isMatch == UriMatchLevel.Match)
            {
                bool queryIsMatch = UriTemplate.Query.Equals(urlToCompare.Query, StringComparison.InvariantCultureIgnoreCase);
                if (queryIsMatch)
                    return queryIsMatch;

                queryIsMatch = CompareQueries(UriTemplate.Query, urlToCompare.Query);

                return isMatch == UriMatchLevel.Match && queryIsMatch;
            }

            return isMatch != UriMatchLevel.NotMatch;
        }

        private bool CompareQueries(string queryTemplate, string queryToCompare)
        {
            string[] queryPartsTemplate = queryTemplate.Split("&");
            string[] queryPartsToCompare = queryToCompare.Split("&");

            int queryPartCount = queryPartsTemplate.Length > queryPartsTemplate.Length ? queryPartsTemplate.Length : queryPartsTemplate.Length;

            bool queryIsMatch = true;
            for (int i = 0; i < queryPartCount && queryIsMatch; i++)
            {
                queryIsMatch &= (queryPartsTemplate[i].Equals(queryPartsToCompare[i], StringComparison.InvariantCultureIgnoreCase));
            }

            return queryIsMatch;
        }


        //Maybe is better return a enum: Matches, NotMatch and AllSubUrlAllowed
        private UriMatchLevel CompareSegments(string[] urlTemplateSegments, string[] urlToCompareSegments)
        {
            int segmentsCount = urlToCompareSegments.Length > urlTemplateSegments.Length ? urlTemplateSegments.Length : urlToCompareSegments.Length;

            bool isMatch = true;

            for (int i = 0; i < segmentsCount && isMatch; i++)
            {
                isMatch &= (urlTemplateSegments[i].Equals(urlToCompareSegments[i], StringComparison.InvariantCultureIgnoreCase));
                if (!isMatch)
                {
                    isMatch = urlTemplateSegments[i] == "*";
                    if (isMatch)
                        return UriMatchLevel.AllSubUrlAllowed;
                }
            }

            return isMatch ? UriMatchLevel.Match : UriMatchLevel.NotMatch;
        }
    }
}
