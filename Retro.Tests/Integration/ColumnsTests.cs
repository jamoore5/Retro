using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Retro.Models;
using Xunit;

namespace Retro.Tests.Integration
{
    public class ColumnsTests  : ControllerTestBase
    {
        [Fact]
        public async Task GetColumnsTest()
        {
            // Arrange
            var board = CreateBoard();
            AddColumn(board);
            AddColumn(board);
            AddColumn(board);

            // Act
            var response = await Client.GetAsync($"/boards/{board.Id}/columns");

            // Assert
            var data = await AssertResponse.SuccessAsync(response);

            var columns = JsonSerializer.Deserialize<IEnumerable<Column>>(data, new JsonSerializerOptions());
            Assert.Equal(3, columns.Count());
        }

        [Fact]
        public async Task GetColumns_BoardExistButNoColumns_Test()
        {
            // Arrange
            var board = CreateBoard();

            // Act
            var response = await Client.GetAsync($"/boards/{board.Id}/columns");

            // Assert
            var data = await AssertResponse.SuccessAsync(response);

            var columns = JsonSerializer.Deserialize<IEnumerable<Column>>(data, new JsonSerializerOptions());
            Assert.Empty(columns);
        }

        [Fact]
        public async Task GetColumns_NonExistingBoard_Test()
        {
            // Act
            var response = await Client.GetAsync("/boards/101/columns");

            // Assert
            await AssertResponse.NotFoundAsync(response);
        }

        [Fact]
        public async Task GetColumn_Test()
        {
            // Arrange
            var board = CreateBoard();
            ColumnService.AddColumn(board.Id, new Column{Id = "Start", Name = "Start"});

            // Act
            var response = await Client.GetAsync($"/boards/{board.Id}/columns/start");

            // Assert
            var data = await AssertResponse.SuccessAsync(response);

            var column = JsonSerializer.Deserialize<Column>(data, new JsonSerializerOptions());
            Assert.Equal(board.Id, column.BoardId);
            Assert.Equal("Start", column.Id);
        }

        [Fact]
        public async Task GetColumn_BoardExistButNoColumns_Test()
        {
            // Arrange
            var board = CreateBoard();

            // Act
            var response = await Client.GetAsync($"/boards/{board.Id}/columns/start");

            // Assert
            await AssertResponse.NotFoundAsync(response);
        }

        [Fact]
        public async Task GetColumn_NonExistingBoard_Test()
        {
            // Act
            var response = await Client.GetAsync("/boards/101/columns/start");

            // Assert
            await AssertResponse.NotFoundAsync(response);
        }

        [Fact]
        public async Task CreateColumn_Test()
        {
            // Arrange
            var board = CreateBoard();
            var column = new Column{ Name = "Test Column"};

            // Act
            var response = await ClientPostAsync($"/boards/{board.Id}/columns", column);

            // Assert
            var data = await AssertResponse.SuccessAsync(response);

            var createdColumn = JsonSerializer.Deserialize<Column>(data, new JsonSerializerOptions());
            Assert.Equal(board.Id, createdColumn.BoardId);
            Assert.Equal("Test Column", createdColumn.Name);
            Assert.False(string.IsNullOrEmpty(createdColumn.Id), "Expected the Id to be set");
        }

        [Fact]
        public async Task CreateColumn_NameRequired_Test()
        {
            // Arrange
            var board = CreateBoard();
            var column = new Column();

            // Act
            var response = await ClientPostAsync($"/boards/{board.Id}/columns", column);

            // Assert
            await AssertResponse.BadRequestAsync(response);
        }

        [Fact]
        public async Task CreateBoard_ClientGeneratedId_Test()
        {
            // Arrange
            var board = CreateBoard();
            var column = new Column{Id = "Test", Name = "Test Column"};

            // Act
            var response = await ClientPostAsync($"/boards/{board.Id}/columns", column);

            // Assert
            var data = await AssertResponse.SuccessAsync(response);

            var createdColumn = JsonSerializer.Deserialize<Column>(data, new JsonSerializerOptions());
            Assert.Equal(board.Id, createdColumn.BoardId);
            Assert.Equal("Test Column", createdColumn.Name);
            Assert.Equal("Test", createdColumn.Id);

        }

        [Fact]
        public async Task CreateBoard_NonExistingBoard_Test()
        {
            // Arrange
            var column = new Column{Id = "Test", Name = "Test Column"};

            // Act
            var response = await ClientPostAsync("/boards/101/columns", column);

            // Assert
            await AssertResponse.BadRequestAsync(response);
        }

        [Fact]
        public async Task DeleteColumn_Test()
        {
            // Arrange
            var board = CreateBoard();
            var column = AddColumn(board);

            // Act
            var response = await Client.DeleteAsync($"/boards/{board.Id}/columns/{column.Id}");

            // Assert
            await AssertResponse.NoContentSuccessAsync(response);

            Assert.Empty(ColumnService.GetColumns(board.Id));
        }

        [Fact]
        public async Task DeleteColumn_NonExistingColumn_Test()
        {
            // Arrange
            var board = CreateBoard();

            // Act
            var response = await Client.DeleteAsync($"/boards/{board.Id}/columns/start");

            // Assert
            await AssertResponse.BadRequestAsync(response);
        }

        [Fact]
        public async Task DeleteColumn_NonExistingBoard_Test()
        {
            // Act
            var response = await Client.DeleteAsync($"/boards/101/columns/start");

            // Assert
            await AssertResponse.BadRequestAsync(response);
        }
    }
}