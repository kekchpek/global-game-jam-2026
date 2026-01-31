using Cysharp.Threading.Tasks;
using kekchpek.Auxiliary;
using UnityEngine;
using UnityMVVM;

namespace GlobalGameJam2026.MVVM.Views.WinComics
{
    public class WinComicsView : ViewBehaviour<IWinComicsViewModel>
    {
        private const string WinSequenceName = "Win";
        
        [SerializeField] private AnimationController _animationController;

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            PlayWinAnimation().Forget();
        }

        private async UniTaskVoid PlayWinAnimation()
        {
            await _animationController.PlaySequence(WinSequenceName);
            ViewModel.OnAnimationComplete();
        }
    }
}
