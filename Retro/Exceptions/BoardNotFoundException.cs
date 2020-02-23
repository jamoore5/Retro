namespace Retro.Exceptions
{
    public class BoardNotFoundException : NotFoundException
    {
        private const string Name = "Board";

        public BoardNotFoundException(long id) : base(Name, id.ToString())
        {
        }
    }
}