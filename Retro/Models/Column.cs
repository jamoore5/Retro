using Retro.Interfaces;

namespace Retro.Models
{
    public class Column : IColumn
    {
        public string Id { get; set; }

        public long BoardId { get; set; }
        public string Name { get; set; }
    }
}