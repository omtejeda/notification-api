using NotificationService.Core.Common.Utils;

namespace NotificationService.Core.Tests.Utils;

public class HttpUtilTests
{
    [Theory]
    [InlineData("10.0.0.1", "get-catalogs", "10.0.0.1/get-catalogs")]
    [InlineData("http://10.0.0.1", "/get-catalogs", "http://10.0.0.1/get-catalogs")]
    [InlineData("https://10.0.0.1/", "get-catalogs", "https://10.0.0.1/get-catalogs")]
    [InlineData("10.0.0.1/", "/get-catalogs", "10.0.0.1/get-catalogs")]
    public void GetFullPath_ShouldReturnCorrectPath(string host, string uri, string expected)
    {
        string actual = HttpUtil.GetFullPath(host, uri);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetQueryStringTestData))]
    public void GetFullPath_WithQueryString_ShouldReturnCorrectPath(
        string host,
        string uri,
        Dictionary<string, string> queryString,
        string expected)
    {
        string actual = HttpUtil.GetFullPath(host, uri, queryString);

        Assert.Equal(expected, actual);
    }

    public static IEnumerable<object[]> GetQueryStringTestData()
    {
        yield return new object[]
        {
            "10.0.0.1",
            "get-catalogs",
            new Dictionary<string, string>
            {
                {"key1", "value1"}
            },
            "10.0.0.1/get-catalogs?key1=value1"
        };

        yield return new object[]
        {
            "https://google.com/",
            "/search",
            new Dictionary<string, string>
            { 
                { "key1", "value1" }, 
                { "key2", "value2" } 
            },
            "https://google.com/search?key1=value1&key2=value2"
        };
    }
}