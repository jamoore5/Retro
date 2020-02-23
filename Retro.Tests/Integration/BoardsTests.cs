using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Retro.Models;
using Xunit;

namespace Retro.Tests.Integration
{
    public class BoardsTests : IDisposable
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public BoardsTests()
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
        public async Task GetBoardsTest()
        {
            // Act
            var response = await _client.GetAsync("/boards");

            // Assert
            var data = await AssertResponse.AssertSuccess(response);

            var boards = JsonSerializer.Deserialize<IEnumerable<Board>>(data, new JsonSerializerOptions());
            Assert.Equal(3, boards.Count());
        }

        [Fact]
        public async Task GetExistingBoardTest()
        {
            // Act
            var response = await _client.GetAsync("/boards/1");

            // Assert
            var data = await AssertResponse.AssertSuccess(response);

            var board = JsonSerializer.Deserialize<Board>(data, new JsonSerializerOptions());
            Assert.Equal(1, board.Id);
        }

        [Fact]
        public async Task GetNonExistingBoardTest()
        {
            // Act
            var response = await _client.GetAsync("/boards/5");

            // Assert
            await AssertResponse.AssertNotFound(response);
        }

        [Fact]
        public async Task CreateBoard()
        {
            var board = new Board {Name = "Test Board"};
            var json = JsonSerializer.Serialize(board);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/vnd.api+json");

            // Act
            var response = await _client.PostAsync("/boards", stringContent);

            var data = await AssertResponse.AssertSuccess(response);

            var createdBoard = JsonSerializer.Deserialize<Board>(data, new JsonSerializerOptions());
            Assert.Equal(4, createdBoard.Id);
            Assert.Equal("Test Board", createdBoard.Name);
        }

        [Fact]
        public async Task CreateBoard_NameRequired()
        {
            var board = new Board();
            var json = JsonSerializer.Serialize(board);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/vnd.api+json");

            // Act
            var response = await _client.PostAsync("/boards", stringContent);

            // Assert
            await AssertResponse.AssertBadRequest(response);
        }

        [Fact]
        public async Task CreateBoard_ClientGeneratedId()
        {
            var board = new Board{Id = 101, Name = "Test Board"};
            var json = JsonSerializer.Serialize(board);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/vnd.api+json");

            // Act
            var response = await _client.PostAsync("/boards", stringContent);

            // Assert
            await AssertResponse.AssertBadRequest(response);
        }
    }
}