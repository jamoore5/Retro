using Retro.Interfaces;

namespace Retro.Models
{
    public class Board : IBoard
    {
        public long  Id { get; set; }
        public string Name { get; set; }
    }
}