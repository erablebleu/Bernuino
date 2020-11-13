using System.Threading.Tasks;

namespace Bernuino.Core.UI
{
   public abstract class ViewModelBase : AdapterBase
   {
      #region Fields

      private bool _isLoaded;

      #endregion

      #region Constructors

      #endregion

      #region Events

      #endregion

      #region Properties

      public bool IsLoaded
      {
         get => _isLoaded;
         set => Set(ref _isLoaded, value);
      }

      #endregion

      #region Methods

      public virtual Task Load(params object[] args)
      {
         return new Task(() => { });
      }

      public virtual Task Unload()
      {
         return new Task(() => { });
      }

      #endregion
   }
}
