using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Retro.Tests.Integration
{
    public static class AssertResponse
    {
        private const string JsonApiMediaType = "application/vnd.api+json";

        public static async Task<string> AssertSuccess(HttpResponseMessage response)
        {
            Assert.True(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
            Assert.Equal(JsonApiMediaType, response.Content.Headers.ContentType.MediaType);

            var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            Assert.True(json.RootElement.TryGetProperty("data", out var data));

            return data.GetRawText();
        }

        public static async Task AssertNotFound(HttpResponseMessage response)
        {
            Assert.False(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
            Assert.Equal(JsonApiMediaType, response.Content.Headers.ContentType.MediaType);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            Assert.True(json.RootElement.TryGetProperty("errors", out _), "Expected the contents to be wrapped in an `errors` property");
        }
    }
}