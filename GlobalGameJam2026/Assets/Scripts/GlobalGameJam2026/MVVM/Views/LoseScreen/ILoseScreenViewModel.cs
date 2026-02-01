using UnityMVVM.ViewModelCore;

namespace GlobalGameJam2026.MVVM.Views.LoseScreen
{
    public interface ILoseScreenViewModel : IViewModel
    {
        /// <summary>
        /// Called when restart button is clicked.
        /// </summary>
        void OnRestartClicked();
    }
}
