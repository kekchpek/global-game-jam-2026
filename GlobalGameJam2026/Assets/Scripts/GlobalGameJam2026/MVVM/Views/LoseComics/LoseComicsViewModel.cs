using Cysharp.Threading.Tasks;
using GlobalGameJam2026.Static;
using UnityMVVM.ViewManager;
using UnityMVVM.ViewModelCore;

namespace GlobalGameJam2026.MVVM.Views.LoseComics
{
    public class LoseComicsViewModel : ViewModel, ILoseComicsViewModel
    {
        private readonly IViewManager _viewManager;

        public LoseComicsViewModel(IViewManager viewManager)
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
