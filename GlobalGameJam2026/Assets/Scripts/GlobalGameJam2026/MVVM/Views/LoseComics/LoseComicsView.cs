using Cysharp.Threading.Tasks;
using kekchpek.Auxiliary;
using UnityEngine;
using UnityMVVM;

namespace GlobalGameJam2026.MVVM.Views.LoseComics
{
    public class LoseComicsView : ViewBehaviour<ILoseComicsViewModel>
    {
        private const string LoseSequenceName = "Lose";
        
        [SerializeField] private AnimationController _animationController;

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            PlayLoseAnimation().Forget();
        }

        private async UniTaskVoid PlayLoseAnimation()
        {
            await _animationController.PlaySequence(LoseSequenceName);
            ViewModel.OnAnimationComplete();
        }
    }
}
