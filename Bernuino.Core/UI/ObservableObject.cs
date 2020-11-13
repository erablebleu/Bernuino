using System.ComponentModel;

namespace Bernuino.Core.UI
{
   public abstract class ObservableObject : INotifyPropertyChanged
   {
      #region Fields

      #endregion

      #region Constructors

      public ObservableObject()
      {

      }

      #endregion

      #region Events

      public event PropertyChangedEventHandler PropertyChanged;

      #endregion

      #region Methods

      public void RaisePropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         PropertyChanged?.Invoke(sender, e);
      }

      public void RaisePropertyChanged(string propertyName)
      {
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }

      #endregion
   }
}
