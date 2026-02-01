using Cysharp.Threading.Tasks;
using GlobalGameJam2026.MVVM.Models.Dating;
using GlobalGameJam2026.Static;
using UnityMVVM.ViewManager;
using UnityMVVM.ViewModelCore;

namespace GlobalGameJam2026.MVVM.Views.WinScreen
{
    public class WinScreenViewModel : ViewModel, IWinScreenViewModel
    {
        private readonly IViewManager _viewManager;
        private readonly IDatingService _datingService;

        public WinScreenViewModel(
            IViewManager viewManager,
            IDatingService datingService)
        {
            _viewManager = viewManager;
            _datingService = datingService;
        }

        public void OnRestartClicked()
        {
            RestartGameAsync().Forget();
        }

        private async UniTaskVoid RestartGameAsync()
        {
            // Reset the dating game state
            _datingService.ResetGame();
            
            // Open dating screen
            await _viewManager.Open(LayerNames.Screen, ViewNames.DatingScreen);
        }
    }
}
