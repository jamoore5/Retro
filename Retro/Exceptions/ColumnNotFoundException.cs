namespace Retro.Exceptions
{
    public class ColumnNotFoundException : NotFoundException
    {
        private const string Name = "Column";

        public ColumnNotFoundException(string id) : base(Name, id)
        {
        }
    }
}