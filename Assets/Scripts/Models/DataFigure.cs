namespace Models
{
    [System.Serializable]
    public class DataFigure
    {
        public int IdFigureProp;
        public int IdColorProp;
        public int IdImageProp;

        public DataFigure(int idFigure, int idColor, int idImage)
        {
            IdFigureProp = idFigure;
            IdColorProp = idColor;
            IdImageProp = idImage;
        }
    }
}