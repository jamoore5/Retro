using System;
using System.ComponentModel.DataAnnotations;
using Retro.Interfaces;

namespace Retro.Models
{
    public class Card : ICard
    {
        public long BoardId { get; set; }
        public string ColumnId { get; set; }
        public Guid Id { get; set; }
        [Required]
        public string Text { get; set; }
    }
}