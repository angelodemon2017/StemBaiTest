namespace Props
{
    public class Prop : IIndexer
    {
        public int Id;

        public int Index
        {
            get
            {
                return Id;
            }
            set 
            {
                Id = value;
            }
        }
    }
}