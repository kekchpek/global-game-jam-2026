using Cysharp.Threading.Tasks;
using GlobalGameJam2026.Static;
using UnityMVVM.ViewManager;
using UnityMVVM.ViewModelCore;

namespace GlobalGameJam2026.MVVM.Views.WinComics
{
    public class WinComicsViewModel : ViewModel, IWinComicsViewModel
    {
        private readonly IViewManager _viewManager;

        public WinComicsViewModel(IViewManager viewManager)
        {
            _viewManager = viewManager;
        }

        public void OnAnimationComplete()
        {
            OpenDatingScreenAsync().Forget();
        }

        private async UniTaskVoid OpenDatingScreenAsync()
        {
            await _viewManager.Open(LayerNames.Screen, ViewNames.DatingScreen);
        }
    }
}
