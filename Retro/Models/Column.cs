using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Retro.Interfaces;

namespace Retro.Models
{
    public class Column : IColumn
    {
        public string Id { get; set; }

        public long BoardId { get; set; }

        [Required]
        public string Name { get; set; }

        public IEnumerable<Card> Cards { get; set; }

        public Column Clone()
        {
            return new Column
            {
                Id = this.Id,
                BoardId = this.BoardId,
                Name = this.Name
            };
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}