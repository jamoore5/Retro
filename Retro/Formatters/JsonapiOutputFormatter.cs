using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Retro.Interfaces;

namespace Retro.Formatters
{
    public class JsonapiOutputFormatter : TextOutputFormatter
    {
        private SystemTextJsonOutputFormatter _formatter;
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
                    if (data is ProblemDetails)
                        WriteErrors(writer, data, options);
                    else
                        WriteData(writer, data, options);
                else
                    JsonSerializer.Serialize(writer, data, options);

                stream.Flush();
            }

            stream.Position = 0;
            return stream;
        }

        private static void WriteData(Utf8JsonWriter writer, object data, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteStartArray("data");

            if (data is IEnumerable<object> dataItems)
            {
                foreach (var item in dataItems)
                {
                    JsonSerializer.Serialize(writer, item, options);
                }
            }
            else
            {
                JsonSerializer.Serialize(writer, data, options);
            }

            writer.WriteEndArray();
            writer.WriteEndObject();
        }

        private static void WriteErrors(Utf8JsonWriter writer, object data, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteStartArray("errors");
            JsonSerializer.Serialize(writer, data, options);
            writer.WriteEndArray();
            writer.WriteEndObject();
        }

    }
}