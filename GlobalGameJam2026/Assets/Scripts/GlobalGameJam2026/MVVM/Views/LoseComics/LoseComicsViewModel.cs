using Cysharp.Threading.Tasks;
using GlobalGameJam2026.MVVM.Models.Dating;
using GlobalGameJam2026.Static;
using AsyncReactAwait.Bindable;
using UnityMVVM.ViewManager;
using UnityMVVM.ViewModelCore;

namespace GlobalGameJam2026.MVVM.Views.LoseComics
{
    public class LoseComicsViewModel : ViewModel, ILoseComicsViewModel
    {
        private readonly IViewManager _viewManager;
        private readonly IDatingService _datingService;
        private readonly IDatingModel _datingModel;
        public IBindable<int> CurrentMask => _datingModel.CurrentDate;
        
        public LoseComicsViewModel(
            IViewManager viewManager,
            IDatingService datingService,
            IDatingModel datingModel)
        {
            _viewManager = viewManager;
            _datingService = datingService;
            _datingModel = datingModel;
        }

        public void OnAnimationComplete()
        {
            // Perform mask swap and continue to next date
            _datingService.MaskSwap();
            OpenDatingScreenAsync().Forget();
        }

        private async UniTaskVoid OpenDatingScreenAsync()
        {
            await _viewManager.Open(LayerNames.Screen, ViewNames.DatingScreen);
        }
    }
}
