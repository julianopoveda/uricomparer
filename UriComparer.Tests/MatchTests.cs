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

        [Fact]
        public void CompareUrlWithWildCardAtEndOfUrlShouldBeTrue()
        {
            UriComparer uriTemplate = new UriComparer("https://example.com/auth/*");
            Uri uriToCompare = new Uri("https://example.com/auth/loggin?user=john&password=123");
            Assert.True(uriTemplate.Match(uriToCompare));
        }

        [Fact]
        public void CompareUrlWithCurlyBracesOnPathShouldBeTrue()
        {
            UriComparer uriTemplate = new UriComparer("https://example.com/{controller}?somevalue=3");
            Uri uriToCompare = new Uri("https://example.com/Name?someValue=3");
            Assert.True(uriTemplate.Match(uriToCompare));
        }

        [Fact]
        public void CompareUrlWithCurlyBracesOnPathAndUriCompareInThatSegmentIsEmptyShouldBeFalse()
        {
            UriComparer uriTemplate = new UriComparer("https://example.com/{controller}?somevalue=3");
            Uri uriToCompare = new Uri("https://example.com//nome?someValue=3");
            Assert.False(uriTemplate.Match(uriToCompare));
        }

        [Fact]
        public void CompareUrlWithOnlyCurlyBraceOpenOrCloseOnPathShouldBeFalse()
        {
            UriComparer uriTemplate = new UriComparer("https://example.com/{controller/action?somevalue=3");
            Uri uriToCompare = new Uri("https://example.com/Name/action?someValue=3");
            Assert.False(uriTemplate.Match(uriToCompare));

            uriTemplate = new UriComparer("https://example.com/path}/action?somevalue=3");
            Assert.False(uriTemplate.Match(uriToCompare));
        }

        [Fact]
        public void CompareUrlWithInvertedCurlyBracesOnPathShouldBeFalse()
        {
            UriComparer uriTemplate = new UriComparer("https://example.com/}controller{/action?somevalue=3");
            Uri uriToCompare = new Uri("https://example.com/Name/action?someValue=3");
            Assert.False(uriTemplate.Match(uriToCompare));            
        }

        [Fact]
        public void CompareUrlWithBlankCurlyBracesOnPathShouldBeFalse()
        {
            UriComparer uriTemplate = new UriComparer("https://example.com/{}/action?somevalue=3");
            Uri uriToCompare = new Uri("https://example.com/Name/action?someValue=3");
            Assert.False(uriTemplate.Match(uriToCompare));
        }

        [Fact]
        public void CompareUrlWithCurlyBracesOnQueryShouldBeTrue()
        {
            UriComparer uriTemplate = new UriComparer("https://example.com/Name?somevalue=3&id={numberHere}");
            Uri uriToCompare = new Uri("https://example.com/Name?someValue=3&id=19ad80fa-96de-4dad-8eb4-86c0e74e2306");
            Assert.True(uriTemplate.Match(uriToCompare));
        }

        [Fact]
        public void CompareUrlWithOnlyCurlyBraceOpenOrCloseOnQueryShouldBeFalse()
        {
            UriComparer uriTemplate = new UriComparer("https://example.com/controller/action?somevalue={number");
            Uri uriToCompare = new Uri("https://example.com/controller/action?someValue=3");
            Assert.False(uriTemplate.Match(uriToCompare));

            uriTemplate = new UriComparer("https://example.com/controller/action?somevalue=here}");
            Assert.False(uriTemplate.Match(uriToCompare));
        }

        [Fact]
        public void CompareUrlWithInvertedCurlyBracesOnQueryShouldBeFalse()
        {
            UriComparer uriTemplate = new UriComparer("https://example.com/action?somevalue=}number{");
            Uri uriToCompare = new Uri("https://example.com/action?someValue=3");
            Assert.False(uriTemplate.Match(uriToCompare));
        }

        [Fact]
        public void CompareUrlWithBlankCurlyBracesOnQueryShouldBeFalse()
        {
            UriComparer uriTemplate = new UriComparer("https://example.com/controller/action?somevalue={}");
            Uri uriToCompare = new Uri("https://example.com/controller/action?someValue=3");
            Assert.False(uriTemplate.Match(uriToCompare));
        }

        [Fact]
        public void CompareUrlWithCurlyBracesAndWrongQueryParameterNameShouldBeFalse()
        {
            UriComparer uriTemplate = new UriComparer("https://example.com/controller/action?carname={name}");
            Uri uriToCompare = new Uri("https://example.com/controller/action?someValue=3");
            Assert.False(uriTemplate.Match(uriToCompare));
        }
        [Fact]
        public void CompareUrlWithCurlyBracesOnQueryAndUriCompareInThatSegmentIsEmptyShouldBeFalse()
        {
            UriComparer uriTemplate = new UriComparer("https://example.com/Name?somevalue=3&id={numberHere}");
            Uri uriToCompare = new Uri("https://example.com/Name?someValue=");
            Assert.False(uriTemplate.Match(uriToCompare));
        }
    }
}
