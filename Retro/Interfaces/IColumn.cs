using System;
using System.Collections.Generic;
using Retro.Models;

namespace Retro.Interfaces
{
    public interface IColumn : ICloneable
    {
        string Id { get; set; }
        long BoardId { get; set; }
        string Name { get; set; }

        IEnumerable<Card> Cards { get; set; }
        Column Clone();
    }
}