using System;
using System.Collections.Generic;
using Retro.Models;

namespace Retro.Interfaces
{
    public interface IBoard : ICloneable
    {
        long Id { get; set; }
        string Name { get; set; }
        IEnumerable<Column> Columns { get; set; }

        public IBoard Clone();
    }
}