using System.ComponentModel.DataAnnotations;
using System;
using Xunit;
using UriComparer;

namespace UriComparer.Tests
{
    public class MatchTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void UriComparerShouldThrowsExceptionIfStringIsNullOrEmpty(string url)
        {
            Assert.Throws<ArgumentNullException>(() => new UriComparer(url));
        }

        [Fact]
        public void TwoStaticUrlShouldBeSame()
        {
            UriComparer uriTemplate = new UriComparer("https://example.com/name?somevalue=3");
            Uri uriToCompare = new Uri("https://example.com/name?somevalue=3");
            Assert.True(uriTemplate.Match(uriToCompare));
        }

        [Fact]
        public void TwoStaticUrlShouldNotBeSame()
        {
            UriComparer uriTemplate = new UriComparer("https://example.com/name?somevalue=3");
            Uri uriToCompare = new Uri("https://example.com/name?somevalue=2");
            Assert.False(uriTemplate.Match(uriToCompare));

            Uri secondUriToCompare = new Uri("https://example.com.br/pinumber/3");
            Assert.False(uriTemplate.Match(secondUriToCompare));
        }

        [Fact]
        public void TwoStaticUrlWithDifferentCasesShouldBeSame()
        {
            UriComparer uriTemplate = new UriComparer("https://example.com/name?somevalue=3");
            Uri uriToCompare = new Uri("https://example.com/Name?someValue=3");
            Assert.True(uriTemplate.Match(uriToCompare));
        }
    }
}
