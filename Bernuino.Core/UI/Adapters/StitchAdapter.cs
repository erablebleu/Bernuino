using System.Collections.ObjectModel;
using System.Drawing;

namespace Bernuino.Core.UI.Adapters
{
    public class StitchAdapter : AdapterBase
    {
        private ObservableCollection<PointAdapter> _points;
        public ObservableCollection<PointAdapter> Points { get => _points; set => Set(ref _points, value); }
        public StitchAdapter()
        {
            Points = new ObservableCollection<PointAdapter>();
        }
        public Stitch GetStitch()
        {
            var result = new Stitch();

            for(int i = 0; i < Points.Count; i++)
            {
                var point = new Point { Y = Points[i].Y - 1 };
                if (i == Points.Count - 1)
                    point.X = 0.5;
                else
                    point.X = Points[i + 1].X - Points[i].X;
                result.Points.Add(point);
            }

            return result;
        }
    }
}
