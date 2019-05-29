using System;

namespace UriComparer
{
    public class UriComparer
    {
        private const string _openCurlyBraceCode = "%7B";
        private const string _closeCurlyBraceCode = "%7D";
        private const string _slashCode = "/";

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
            try
            {
                if (!UriTemplate.Authority.Equals(urlToCompare.Authority))
                    return false;

                if (UriTemplate.AbsoluteUri.IndexOf("*") == UriTemplate.AbsoluteUri.Length - 1)
                    return UriTemplate.IsBaseOf(urlToCompare);

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
            catch
            {
                throw;
            }
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
                if (!queryIsMatch && IsTemplateBracket(queryPartsTemplate[i]))
                {                    
                    string parameterName = queryPartsTemplate[i].Split("=")[0];
                    queryIsMatch = queryPartsToCompare[i].IndexOf(parameterName, StringComparison.InvariantCultureIgnoreCase) >= 0;
                }
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
                string templateSegment = urlTemplateSegments[i];
                isMatch &= (templateSegment.Equals(urlToCompareSegments[i], StringComparison.InvariantCultureIgnoreCase));

                if (!isMatch)
                {
                    if (templateSegment == "*")
                        return UriMatchLevel.AllSubUrlAllowed;

                    isMatch = IsTemplateBracket(templateSegment) && (!string.IsNullOrEmpty(urlToCompareSegments[i]) && !urlToCompareSegments[i].Equals(_slashCode));
                }
            }

            return isMatch ? UriMatchLevel.Match : UriMatchLevel.NotMatch;
        }

        private bool IsTemplateBracket(string templateSegment)
        {
            int openCurlyBraces = templateSegment.IndexOf(_openCurlyBraceCode);
            int closeCurlyBraces = templateSegment.IndexOf(_closeCurlyBraceCode);
            return openCurlyBraces >= 0 && closeCurlyBraces >= 0 && openCurlyBraces + _openCurlyBraceCode.Length < closeCurlyBraces;
        }
    }
}
