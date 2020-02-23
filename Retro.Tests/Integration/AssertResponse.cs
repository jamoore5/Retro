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

        public static async Task<string> Success(HttpResponseMessage response)
        {
            Assert.True(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
            Assert.Equal(JsonApiMediaType, response.Content.Headers.ContentType.MediaType);

            var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            Assert.True(json.RootElement.TryGetProperty("data", out var data));

            return data.GetRawText();
        }

        public static async Task NoContentSuccess(HttpResponseMessage response)
        {
            Assert.True(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
            Assert.Null(response.Content.Headers.ContentType);
            Assert.Equal(string.Empty, await response.Content.ReadAsStringAsync());
        }

        public static async Task NotFound(HttpResponseMessage response)
        {
            await AssertFailure(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        public static async Task BadRequest(HttpResponseMessage response)
        {
            await AssertFailure(response);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        private static async Task AssertFailure(HttpResponseMessage response)
        {
            Assert.False(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
            Assert.Equal(JsonApiMediaType, response.Content.Headers.ContentType.MediaType);

            var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            Assert.True(json.RootElement.TryGetProperty("errors", out _), "Expected the contents to be wrapped in an `errors` property");
        }
    }
}