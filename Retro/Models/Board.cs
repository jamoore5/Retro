using System.ComponentModel.DataAnnotations;
using Retro.Interfaces;

namespace Retro.Models
{
    public class Board : IBoard
    {
        public long  Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}