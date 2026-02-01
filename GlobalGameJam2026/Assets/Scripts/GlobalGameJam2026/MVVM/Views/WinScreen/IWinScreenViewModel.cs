using UnityMVVM.ViewModelCore;

namespace GlobalGameJam2026.MVVM.Views.WinScreen
{
    public interface IWinScreenViewModel : IViewModel
    {
        /// <summary>
        /// Called when restart button is clicked.
        /// </summary>
        void OnRestartClicked();
    }
}
