using Bernuino.Core.UI;
using Bernuino.Core.UI.Adapters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bernuino.UI
{/*
    internal class PointAdapter : AdapterBase
    {
        public double X { get; set; }
        public double Y { get; set; }
        public ObservableCollection<LineAdapter> Lines { get; set; }
        public PointAdapter()
        {
            Lines = new ObservableCollection<LineAdapter>();
        }
    }
    */
    public class LineAdapter : AdapterBase
    {
        private PointAdapter _pointA;
        private PointAdapter _pointB;
        public PointAdapter PointA { get => _pointA; set => Set(ref _pointA, value); }
        public PointAdapter PointB { get => _pointB; set => Set(ref _pointB, value); }
    }


    /// <summary>
    /// Interaction logic for StitchEditor.xaml
    /// </summary>
    public partial class StitchEditor : UserControl
    {
        public static readonly DependencyProperty StitchProperty = DependencyProperty.Register(
          "Stitch", typeof(StitchAdapter), typeof(StitchEditor), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPropertyChanged));
        
        public ObservableCollection<LineAdapter> Lines { get; set; }
        public StitchAdapter Stitch
        {
            get { return (StitchAdapter)this.GetValue(StitchProperty); }
            set { this.SetValue(StitchProperty, value); Update(); }
        }
        public StitchEditor()
        {
            Lines = new ObservableCollection<LineAdapter>();
            InitializeComponent();
            lines.ItemsSource = Lines;
        }
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((StitchEditor)d).Update();
        }
        public void Update()
        {
            var stitch = Stitch;
            Lines.Clear();

            if (stitch is null)
                return;

            for (int i = 0; i < stitch.Points.Count - 1; i++)
                Lines.Add(new LineAdapter { PointA = stitch.Points[i], PointB = stitch.Points[i + 1] });

            _lastPoint = stitch.Points.Last();
        }

        private const double _ellipseWidth = 0.05;
        private PointAdapter _moveAdapter;
        private PointAdapter _lastPoint;
        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_moveAdapter is null)
                return;

            var pos = e.GetPosition(canvas);
            _moveAdapter.X = pos.Y - _ellipseWidth / 2;
            _moveAdapter.Y = pos.X - _ellipseWidth / 2;
        }
        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var s = sender as Ellipse;
            _moveAdapter = s.DataContext as PointAdapter;
            e.Handled = true;
        }
        private void canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _moveAdapter = null;
        }
        private void canvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(canvas);

            _moveAdapter = Stitch.Points.FirstOrDefault(p => Math.Pow(pos.X - p.Y, 2) + Math.Pow(pos.Y - p.X, 2) < Math.Pow(_ellipseWidth, 2));

            if (!(_moveAdapter is null))
                return;

            _moveAdapter = new PointAdapter { X = pos.Y - _ellipseWidth / 2, Y = pos.X - _ellipseWidth / 2 };
            Lines.Add(new LineAdapter { PointA = _lastPoint, PointB = _moveAdapter });
            Stitch.Points.Add(_moveAdapter);
            _lastPoint = _moveAdapter;
        }
    }
}
