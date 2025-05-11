using Models;
using UnityEngine;
using Zenject;

namespace Services
{
    public class PropsServices
    {
        private ConfigPropsLibrary _propsLibrary;

        [Inject]
        public PropsServices(ConfigPropsLibrary propsLibrary)
        {
            _propsLibrary = propsLibrary;
        }

        public Sprite GetShape(DataFigure dataFigure)
        {
            return _propsLibrary.Figures[dataFigure.IdFigureProp].Shape;
        }

        public Figure GetFigure(DataFigure dataFigure)
        {
            return _propsLibrary.Figures[dataFigure.IdFigureProp].Figure;
        }

        public Color GetColor(DataFigure dataFigure)
        {
            return _propsLibrary.Colors[dataFigure.IdColorProp].BackGroundColor;
        }

        public Sprite GetIcon(DataFigure dataFigure)
        {
            return _propsLibrary.Icons[dataFigure.IdImageProp].Icon;
        }
    }
}