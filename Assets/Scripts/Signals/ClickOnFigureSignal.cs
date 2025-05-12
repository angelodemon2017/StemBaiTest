using Models;

namespace Signals
{
    public struct ClickOnFigureSignal
    {
        public DataFigure dataFigure;

        public ClickOnFigureSignal(DataFigure data)
        {
            dataFigure = data;
        }
    }
}