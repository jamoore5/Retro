using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Retro.Interfaces;
using Retro.Models;
using Xunit;

namespace Retro.Tests.Integration
{
    public abstract class ControllerTestBase : IDisposable
    {
        private readonly WebApplicationFactory<Startup> _factory;
        protected HttpClient Client { get; }

        protected ControllerTestBase()
        {
            _factory = new WebApplicationFactory<Startup>();

            // Create an HttpClient which is setup for the test host
            Client = _factory.CreateClient();
        }

        public void Dispose()
        {
            _factory.Dispose();
            Client.Dispose();
        }

        protected IBoardService BoardService {get => (IBoardService) _factory.Services.GetService(typeof(IBoardService));}
        protected IColumnService ColumnService {get => (IColumnService) _factory.Services.GetService(typeof(IColumnService));}
        protected ICardService CardService {get => (ICardService) _factory.Services.GetService(typeof(ICardService));}

        protected IBoard CreateBoard()
        {
            var board = new Board {Name = "New Board"};
            BoardService.AddBoard(board);
            return board;
        }

        protected IColumn AddColumn(IBoard board)
        {
            var column = new Column {Name = "Test Column"};
            ColumnService.AddColumn(board.Id, column);
            return column;
        }

        protected ICard AddCard(IColumn column)
        {
            var card = new Card {Text = "Test Card"};
            CardService.AddCard(column.BoardId, column.Id, card);
            return card;
        }

        protected async Task<HttpResponseMessage> ClientPostAsync<T>(string endpoint, T item)
        {
            // Arrange
            var json = JsonSerializer.Serialize(item);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/vnd.api+json");

            // Act
            return await Client.PostAsync(endpoint, stringContent);
        }


    }
}