namespace Bernuino.Core.UI.Adapters
{
    public class PointAdapter : AdapterBase
    {
        private double _x;
        private double _y;
        public double X { get => _x; set => Set(ref _x, value); }
        public double Y { get => _y; set => Set(ref _y, value); }
    }
}
