using NotificationService.Core.Common.Utils;

namespace NotificationService.Core.Tests.Utils;

public class HttpUtilTests
{
    [Theory]
    [MemberData(nameof(GetFullPathTestData))]
    public void GetFullPath_ShouldReturnCorrectPath(string host, string uri, string expected)
    {
        string actual = HttpUtil.GetFullPath(host, uri);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetFullPathWithQueryStringTestData))]
    public void GetFullPath_WithQueryString_ShouldReturnCorrectPath(
        string host,
        string uri,
        Dictionary<string, string> queryString,
        string expected)
    {
        string actual = HttpUtil.GetFullPath(host, uri, queryString);

        Assert.Equal(expected, actual);
    }

    #region TestData
    public static TheoryData<string, string, string> GetFullPathTestData()
    {
        return new()
        {
            { "10.0.0.1", "get-catalogs", "10.0.0.1/get-catalogs" },
            { "http://10.0.0.1", "/get-catalogs", "http://10.0.0.1/get-catalogs" },
            { "https://10.0.0.1/", "get-catalogs", "https://10.0.0.1/get-catalogs" },
            { "10.0.0.1/", "/get-catalogs", "10.0.0.1/get-catalogs" }
        };
    }

    public static TheoryData<string, string, Dictionary<string, string>, string> GetFullPathWithQueryStringTestData()
    {
        var queryStringForTestCaseNumberOne = new Dictionary<string, string>
        {
            { "key1", "value1" }
        };

        var queryStringForTestCaseNumberTwo = new Dictionary<string, string>
        {
            { "key1", "value1" },
            { "key2", "value2" }
        };

        return new()
        {
            { "10.0.0.1", "get-catalogs",  queryStringForTestCaseNumberOne, "10.0.0.1/get-catalogs?key1=value1" },
            { "https://google.com/", "/search", queryStringForTestCaseNumberTwo, "https://google.com/search?key1=value1&key2=value2" }
        };
    }
    #endregion
}