namespace Retro.Interfaces
{
    public interface IColumn
    {
        string Id { get; set; }
        long BoardId { get; set; }
        string Name { get; set; }
    }
}