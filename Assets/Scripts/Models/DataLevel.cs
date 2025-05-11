using Extensions;
using System.Collections.Generic;

namespace Models
{
    public class DataLevel
    {
        private ConfigPropsLibrary _configPropsLibrary;

        public int CurrentLevel;
        public int Score;
        public List<DataFigure> Figures = new List<DataFigure>();
        public List<DataFigure> SelectedFigures = new();

        public DataLevel(ConfigPropsLibrary configPropsLibrary)
        {
            _configPropsLibrary = configPropsLibrary;
        }

        public void GenerateDataFigures()
        {
            for (int i = 0; i < _configPropsLibrary.Figures.Count; i++)
                for (int j = 0; j < _configPropsLibrary.Colors.Count; j++)
                {
                    Figures.Add(new DataFigure(i, j, 0));
                }
            Figures.Shuffle();
        }
    }
}