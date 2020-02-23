using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Retro.Exceptions;
using Retro.Interfaces;
using Retro.Models;
using Xunit;

namespace Retro.Tests.Integration
{
    public class CardsTests : ControllerTestBase
    {
        [Fact]
        public async Task GetCards_Test()
        {
            // Arrange
            var board = CreateBoard();
            var column = AddColumn(board);
            var card1 = AddCard(column);
            var card2 = AddCard(column);
            var card3 = AddCard(column);

            // Act
            var response = await Client.GetAsync($"/boards/{board.Id}/columns/{column.Id}/cards");

            // Assert
            var data = await AssertResponse.SuccessAsync(response);

            var cards = JsonSerializer.Deserialize<IEnumerable<Card>>(data, new JsonSerializerOptions()).ToList();
            Assert.Equal(3, cards.Count());
            Assert.True(cards.Exists(x => x.Id == card1.Id));
            Assert.True(cards.Exists(x => x.Id == card2.Id));
            Assert.True(cards.Exists(x => x.Id == card3.Id));
        }

        [Fact]
        public async Task GetCard_Test()
        {
            // Arrange
            var board = CreateBoard();
            var column = AddColumn(board);
            var card = AddCard(column);

            // Act
            var response = await Client.GetAsync($"/boards/{board.Id}/columns/{column.Id}/cards/{card.Id}");

            // Assert
            var data = await AssertResponse.SuccessAsync(response);

            var actual = JsonSerializer.Deserialize<Card>(data, new JsonSerializerOptions());
            Assert.Equal(card.Id, actual.Id);
        }

        [Fact]
        public async Task CreateCard_Test()
        {
            // Arrange
            var board = CreateBoard();
            var column = AddColumn(board);
            var card = new Card {Text = "Test Card"};

            // Act
            var response = await ClientPostAsync($"/boards/{board.Id}/columns/{column.Id}/cards/", card);

            // Assert
            var data = await AssertResponse.SuccessAsync(response);

            var actual = JsonSerializer.Deserialize<Card>(data, new JsonSerializerOptions());
            Assert.Equal(board.Id, actual.BoardId);
            Assert.Equal(column.Id, actual.ColumnId);
            Assert.True(actual.Id != null && actual.Id != Guid.Empty);
            Assert.Equal(card.Text, actual.Text);
        }

        [Fact]
        public async Task CreateCard_ClientGeneratedId_Test()
        {
            // Arrange
            var board = CreateBoard();
            var column = AddColumn(board);
            var card = new Card {Id = Guid.NewGuid(), Text = "Test Card"};

            // Act
            var response = await ClientPostAsync($"/boards/{board.Id}/columns/{column.Id}/cards/", card);

            // Assert
            await AssertResponse.BadRequestAsync(response);
        }

        [Fact]
        public async Task CreateCard_RequireText_Test()
        {
            // Arrange
            var board = CreateBoard();
            var column = AddColumn(board);
            var card = new Card {};

            // Act
            var response = await ClientPostAsync($"/boards/{board.Id}/columns/{column.Id}/cards/", card);

            // Assert
            await AssertResponse.BadRequestAsync(response);
        }

        [Fact]
        public async Task CreateCard_NonExistentColumn_Test()
        {
            // Arrange
            var board = CreateBoard();
            var card = new Card {Text = "Test Card"};

            // Act
            var response = await ClientPostAsync($"/boards/{board.Id}/columns/101/cards/", card);

            // Assert
            await AssertResponse.BadRequestAsync(response);
        }

        [Fact]
        public async Task DeleteCard_Test()
        {
            // Arrange
            var board = CreateBoard();
            var column = AddColumn(board);
            var card = AddCard(column);

            // Act
            var response = await Client.DeleteAsync($"/boards/{board.Id}/columns/{column.Id}/cards/{card.Id}");

            // Assert
            await AssertResponse.NoContentSuccessAsync(response);
            Assert.Throws<CardNotFoundException>(() => CardService.GetCard(board.Id, column.Id, card.Id));
        }

        [Fact]
        public async Task DeleteCard_NonExistentCard_Test()
        {
            // Arrange
            var board = CreateBoard();
            var column = AddColumn(board);

            // Act
            var response = await Client.DeleteAsync($"/boards/{board.Id}/columns/{column.Id}/cards/101");

            // Assert
            await AssertResponse.BadRequestAsync(response);
        }
    }
}