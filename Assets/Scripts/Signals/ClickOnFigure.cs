using Models;

namespace Signals
{
    public struct ClickOnFigure
    {
        public DataFigure dataFigure;

        public ClickOnFigure(DataFigure data)
        {
            dataFigure = data;
        }
    }
}