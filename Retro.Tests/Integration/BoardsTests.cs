using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Retro.Exceptions;
using Retro.Interfaces;
using Retro.Models;
using Xunit;

namespace Retro.Tests.Integration
{
    public class BoardsTests : ControllerTestBase
    {
        [Fact]
        public async Task GetBoards_Test()
        {
            // Arrange
            var board1 = CreateBoard();
            var board2 = CreateBoard();
            var board3 = CreateBoard();

            // Act
            var response = await Client.GetAsync("/boards");

            // Assert
            var data = await AssertResponse.SuccessAsync(response);

            var boards = JsonSerializer.Deserialize<IEnumerable<Board>>(data, new JsonSerializerOptions()).ToList();
            Assert.Equal(3, boards.Count());
            Assert.True(boards.Exists(x => x.Id == board1.Id));
            Assert.True(boards.Exists(x => x.Id == board2.Id));
            Assert.True(boards.Exists(x => x.Id == board3.Id));
        }

        [Fact]
        public async Task GetExistingBoard_Test()
        {
            // Arrange
            var board = CreateBoard();

            // Act
            var response = await Client.GetAsync($"/boards/{board.Id}");

            // Assert
            var data = await AssertResponse.SuccessAsync(response);

            var actual = JsonSerializer.Deserialize<Board>(data, new JsonSerializerOptions());
            Assert.Equal(board.Id, actual.Id);
        }

        [Fact]
        public async Task GetNonExistingBoard_Test()
        {
            // Act
            var response = await Client.GetAsync("/boards/101");

            // Assert
            await AssertResponse.NotFoundAsync(response);
        }

        [Fact]
        public async Task CreateBoard_Test()
        {
            // Arrange
            var board = new Board {Name = "Test Board"};

            // Act
            var response = await ClientPostAsync("/boards", board);

            // Assert
            var data = await AssertResponse.SuccessAsync(response);

            var createdBoard = JsonSerializer.Deserialize<Board>(data, new JsonSerializerOptions());
            Assert.Equal(4, createdBoard.Id);
            Assert.Equal("Test Board", createdBoard.Name);
        }

        [Fact]
        public async Task CreateBoard_NameRequired_Test()
        {
            // Arrange
            var board = new Board();

            // Act
            var response = await ClientPostAsync("/boards", board);

            // Assert
            await AssertResponse.BadRequestAsync(response);
        }

        [Fact]
        public async Task CreateBoard_ClientGeneratedId_Test()
        {
            // Arrange
            var board = new Board{Id = 101, Name = "Test Board"};

            // Act
            var response = await ClientPostAsync("/boards", board);

            // Assert
            await AssertResponse.BadRequestAsync(response);
        }

        [Fact]
        public async Task DeleteBoard_Test()
        {
            // Arrange
            var board = CreateBoard();
            var column = AddColumn(board);
            AddColumn(board);
            AddColumn(board);
            AddCard(column);
            AddCard(column);
            AddCard(column);

            // Act
            var response = await Client.DeleteAsync($"/boards/{board.Id}");

            await AssertResponse.NoContentSuccessAsync(response);

            Assert.Throws<BoardNotFoundException>(() => BoardService.GetBoard(board.Id));
            Assert.Throws<BoardNotFoundException>(() => ColumnService.GetColumns(board.Id));
            Assert.Throws<BoardNotFoundException>(() => CardService.GetCards(board.Id, column.Id));
        }

        [Fact]
        public async Task DeleteBoard_NonExistingBoardTest()
        {
            // Act
            var response = await Client.DeleteAsync("/boards/101");

            await AssertResponse.BadRequestAsync(response);
        }
    }
}