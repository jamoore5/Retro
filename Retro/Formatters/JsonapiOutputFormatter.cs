using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Retro.Formatters
{
    public class JsonapiOutputFormatter : TextOutputFormatter
    {
        private readonly SystemTextJsonOutputFormatter _formatter;
        public JsonapiOutputFormatter()
        {
            SupportedMediaTypes.Add("application/vnd.api+json");
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
            _formatter = new SystemTextJsonOutputFormatter(new JsonSerializerOptions());
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;

            var responseJson = FormatData(context);

            await response.WriteAsync(responseJson);
        }

        private string FormatData(OutputFormatterWriteContext context)
        {
            var stream = WriteFormattedResponseToStream(context);
            return ReadFormattedDataFromStream(stream);
        }

        private static string ReadFormattedDataFromStream(MemoryStream stream)
        {
            string formattedData;
            using (var reader = new StreamReader(stream))
            {
                formattedData = reader.ReadToEnd();
            }

            return formattedData;
        }

        private MemoryStream WriteFormattedResponseToStream(OutputFormatterWriteContext context)
        {
            var data = context.Object;
            var options = _formatter.SerializerOptions;

            var stream = new MemoryStream();

            using (var writer = new Utf8JsonWriter(stream))
            {
                if (context.ContentTypeIsServerDefined)
                    WriteWrappedData(writer, data, options, isError: (data is ProblemDetails));
                else
                    JsonSerializer.Serialize(writer, data, options);

                stream.Flush();
            }

            stream.Position = 0;
            return stream;
        }

        private static void WriteWrappedData(Utf8JsonWriter writer, object content, JsonSerializerOptions options, bool isError = false)
        {
            var rootProperty = (isError) ? "errors" : "data";
            var wrappedData = new Dictionary<string, object>{{rootProperty, content}};
            JsonSerializer.Serialize(writer, wrappedData, options);
        }
    }
}