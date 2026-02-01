using Cysharp.Threading.Tasks;
using GlobalGameJam2026.MVVM.Models.Dating;
using GlobalGameJam2026.Static;
using UnityMVVM.ViewManager;
using UnityMVVM.ViewModelCore;

namespace GlobalGameJam2026.MVVM.Views.LoseComics
{
    public class LoseComicsViewModel : ViewModel, ILoseComicsViewModel
    {
        private readonly IViewManager _viewManager;
        private readonly IDatingService _datingService;
        public LoseComicsViewModel(
            IViewManager viewManager,
            IDatingService datingService)
        {
            _viewManager = viewManager;
            _datingService = datingService;
        }

        public void OnAnimationComplete()
        {
            _datingService.MaskSwap();
            OpenDatingScreenAsync().Forget();
        }

        private async UniTaskVoid OpenDatingScreenAsync()
        {
            await _viewManager.Open(LayerNames.Screen, ViewNames.DatingScreen);
        }
    }
}
