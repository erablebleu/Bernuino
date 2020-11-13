using System.Runtime.CompilerServices;

namespace Bernuino.Core.UI
{
   public abstract class AdapterBase : ObservableObject
   {
      #region Fields

      private bool _isDirty;

      #endregion

      #region Constructors

      public AdapterBase()
      {

      }

      #endregion

      #region Events

      #endregion

      #region Properties

      public bool IsDirty
      {
         get => _isDirty;
         set => Set(ref _isDirty, value);
      }

      #endregion

      #region Methods

      public bool Set<T>(ref T obj, T value, [CallerMemberName] string propertyName = null)
      {
         if (obj != null
             && obj.Equals(value))
            return false;

         obj = value;
         RaisePropertyChanged(propertyName);
         return true;
      }

      public bool SetDirty<T>(ref T obj, T value, [CallerMemberName] string propertyName = null)
      {
         bool res = Set(ref obj, value, propertyName);

         if (res)
            IsDirty = true;

         return res;
      }

      #endregion
   }
}
