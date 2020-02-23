using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Retro.Interfaces;
using Retro.Models;

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
    }
}