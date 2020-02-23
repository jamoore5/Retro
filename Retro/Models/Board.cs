using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Retro.Interfaces;

namespace Retro.Models
{
    public class Board : IBoard
    {
        public long  Id { get; set; }

        [Required]
        public string Name { get; set; }

        public IEnumerable<Column> Columns { get; set; }

        public IBoard Clone()
        {
            return new Board
            {
                Id = this.Id,
                Name = this.Name
            };
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}