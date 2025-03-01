using Newtonsoft.Json;
using NotificationService.Application.Features.Providers.Libraries.JSONParser;

namespace NotificationService.Application.Tests.Features.Providers.Libraries.JSONParser;

public class JSONParserTests
{
    [Fact]
    public void Transform_GivenValidJsonBodyDefinition_ShouldGenerateCorrectJson()
    {
        // Arrange
        var expected = new
        {
            applicationName = "Notification API",
            author = "omtejeda",
            releaseDate = "2023-03-27",
            versions = new List<object>
            {
                new { version = "1.0", date = "2023-03-27" },
                new { version = "2.0", date = "2023-05-04" }
            }
        };
        var expectedJson = JsonConvert.SerializeObject(expected);

        // Runtime metadata
        var metadata = new List<Metadata> 
        {
            new()
            {
                Key = "applicationName",
                Value = "Notification API"
            },
            new()
            {
                Key = "author",
                Value = "omtejeda"
            },
            new()
            {
                Key = "releaseDate",
                Value = "2023-03-27"
            },
            new()
            {
                Key = "versions[0].version",
                Value = "1.0"
            },
            new()
            {
                Key = "versions[0].date",
                Value = "2023-03-27"
            },
            new()
            {
                Key = "versions[1].version",
                Value = "2.0"
            },
            new()
            {
                Key = "versions[1].date",
                Value = "2023-05-04"
            }
        };

        // Json Definition
        var jsonBody = new JsonBody
        {
            RootIs = DataType.Object,
            Definition = [
                new JsonKey
                {
                    PropertyName = "applicationName",
                    DataType = DataType.String,
                    IsRequired = true,
                },
                new JsonKey
                {
                    PropertyName = "author",
                    DataType = DataType.String,
                    IsRequired = true,
                },
                new JsonKey
                {
                    PropertyName = "releaseDate",
                    DataType = DataType.Date,
                    IsRequired = true,
                },
                new JsonKey
                {
                    PropertyName = "versions",
                    DataType = DataType.Array,
                    IsRequired = true,
                    Childs = [
                        new JsonKey
                        {
                            PropertyName = "",
                            DataType = DataType.Object,
                            IsRequired = true,
                            Childs =
                            [
                                new JsonKey
                                {
                                    PropertyName = "version",
                                    DataType = DataType.String,
                                    IsRequired = true
                                },
                                new JsonKey
                                {
                                    PropertyName = "date",
                                    DataType = DataType.Date,
                                    IsRequired = true
                                }
                            ]
                        }
                    ]
                }
            ],
            Metadata = metadata
        };

        // Act
        var actualJson = jsonBody
            .Prepare()
            .Transform();

        // Assert
        Assert.Equal(expectedJson, actualJson);
    }
}