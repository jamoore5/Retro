using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Retro.Models;
using Xunit;

namespace Retro.Tests.Integration
{
    public class ColumnsTests : IDisposable
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public ColumnsTests()
        {
            _factory = new WebApplicationFactory<Startup>();

            // Create an HttpClient which is setup for the test host
            _client = _factory.CreateClient();
        }

        public void Dispose()
        {
            _factory.Dispose();
            _client.Dispose();
        }

        [Fact]
        public async Task GetColumnsTest()
        {
            // Act
            var response = await _client.GetAsync("/boards/1/columns");

            // Assert
            var data = await AssertResponse.AssertSuccess(response);

            var columns = JsonSerializer.Deserialize<IEnumerable<Column>>(data, new JsonSerializerOptions());
            Assert.Equal(3, columns.Count());
        }

        [Fact]
        public async Task GetColumns_BoardExistButNoColumns_Test()
        {
            // Act
            var response = await _client.GetAsync("/boards/2/columns");

            // Assert
            var data = await AssertResponse.AssertSuccess(response);

            var columns = JsonSerializer.Deserialize<IEnumerable<Column>>(data, new JsonSerializerOptions());
            Assert.Empty(columns);
        }

        [Fact]
        public async Task GetColumns_NonExistingBoard_Test()
        {
            // Act
            var response = await _client.GetAsync("/boards/5/columns");

            // Assert
            await AssertResponse.AssertNotFound(response);
        }

        [Fact]
        public async Task GetColumnTest()
        {
            // Act
            var response = await _client.GetAsync("/boards/1/columns/start");

            // Assert
            var data = await AssertResponse.AssertSuccess(response);

            var column = JsonSerializer.Deserialize<Column>(data, new JsonSerializerOptions());
            Assert.Equal(1, column.BoardId);
            Assert.Equal("Start", column.Id);
        }

        [Fact]
        public async Task GetColumn_BoardExistButNoColumns_Test()
        {
            // Act
            var response = await _client.GetAsync("/boards/2/columns/start");

            // Assert
            await AssertResponse.AssertNotFound(response);
        }

        [Fact]
        public async Task GetColumn_NonExistingBoard_Test()
        {
            // Act
            var response = await _client.GetAsync("/boards/5/columns/start");

            // Assert
            await AssertResponse.AssertNotFound(response);
        }

    }
}