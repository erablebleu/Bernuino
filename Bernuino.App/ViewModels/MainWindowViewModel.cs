using Bernuino.Core.UI;
using Bernuino.Core.UI.Adapters;
using System.IO;
using System.Text.Json;
using System.Windows.Input;

namespace Bernuino.App.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private StitchAdapter _stitch;
        private ICommand _openCommand;
        private ICommand _saveCommand;
        public StitchAdapter Stitch { get => _stitch; set => Set(ref _stitch, value); }
        public ICommand OpenCommand => _openCommand ?? (_openCommand = new RelayCommand(OnOpenCommand));
        public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new RelayCommand(OnSaveCommand));
        public MainWindowViewModel()
        {
            var adapter = new StitchAdapter();
            adapter.Points.Add(new PointAdapter { X = 0, Y = 0.5 });
            adapter.Points.Add(new PointAdapter { X = 1, Y = 0.8 });
            Stitch = adapter;
        }
        private string _fileName = "bernuino.stt";
        private void OnOpenCommand()
        {
            try
            {
                Stitch = JsonSerializer.Deserialize<StitchAdapter>(File.ReadAllText(_fileName));
            }
            catch { }
        }
        private void OnSaveCommand()
        {
            try
            {
                File.WriteAllText(_fileName, JsonSerializer.Serialize(Stitch));
                var stitch = Stitch.GetStitch();
                string cpp = string.Empty;
                cpp += $"_countX = {stitch.Points.Count-1};\r\n";
                cpp += $"_countY = {stitch.Points.Count-1};\r\n";
                cpp += $"_x = new float(_countX);\r\n";
                cpp += $"_y = new float(_countY);\r\n";
                for (int i = 0; i< stitch.Points.Count - 1; i++)
                {
                    cpp += $"_x[{i}] = {stitch.Points[i].X.ToString().Replace(",", ".")};\r\n";
                    cpp += $"_y[{i}] = {stitch.Points[i].Y.ToString().Replace(",", ".")};\r\n";
                }
                File.WriteAllText("bernuino.h", cpp);
            }
            catch { }
        }
    }
}
