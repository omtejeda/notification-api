using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Api.Utils;
using NotificationService.Api.Attributes;
using System.Collections.Generic;
using System.Linq;

using NotificationService.Core.Providers.Libraries.JSONParser;

namespace NotificationService.Api.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [ApiVersion(ApiVersions.v1)]
    [Route(Routes.ControllerRoute)]
    public class TestController : ApiController
    {

        [HttpPost("json-parser")]
        public async Task<IActionResult> TestJsonParser([FromQuery] string metadataString)
        {
            var jsonBody = new JsonBody
            {
                RootIs = DataType.Object,
                Definition = new List<JsonKey>
                {
                    new()
                    {
                        PropertyName = "chat",
                        DataType = DataType.Object,
                        IsRequired = true, 
                        Childs = new()
                        {
                            new() { PropertyName = "channelId", DataType = DataType.String, IsRequired = true },
                            new() { PropertyName = "contactId", DataType = DataType.String, IsRequired = true }
                        }
                    },

                    new()
                    {
                        PropertyName = "miObjectoDinamico",
                        DataType = DataType.DynamicObject,
                        IsRequired = false
                    },

                    new()
                    {
                        PropertyName = "messages",
                        DataType = DataType.Array,
                        IsRequired = true,
                        Childs = new()
                        {
                            new() 
                            {
                                PropertyName = string.Empty,
                                DataType = DataType.Object, 
                                IsRequired = true,
                                Childs = new()
                                {
                                    new() { PropertyName = "text", DataType = DataType.String, IsRequired = false },

                                    // media
                                    new() { 
                                        PropertyName = "media", DataType = DataType.Object, IsRequired = false,
                                        Childs = new()
                                        {
                                            new() { PropertyName = "mimeType", DataType = DataType.String, IsRequired = false },
                                            new() { PropertyName = "url", DataType = DataType.String, IsRequired = false }
                                        }
                                    }
                                    //end media
                                }
                            },
                        }
                    }
                }
            };
            var preview = jsonBody.Transform();
            var metadataList = new List<Metadata>();
            
            if (!string.IsNullOrEmpty(metadataString))
            {
                metadataList = metadataString
                    .Split(';')
                    .Select
                    (
                        x => new Metadata
                        {
                            Key = x.Split('=').FirstOrDefault(),
                            Value = x.Split('=').LastOrDefault() 
                        }
                    )
                    .ToList();
            }

            jsonBody.Metadata = metadataList;

            jsonBody.Prepare();
            return Ok(jsonBody.Transform());
        }
    }
}