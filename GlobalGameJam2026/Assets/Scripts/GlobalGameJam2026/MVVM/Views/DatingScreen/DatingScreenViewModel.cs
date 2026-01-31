using System;
using Cysharp.Threading.Tasks;
using GlobalGameJam2026.MVVM.Models.Dating;
using GlobalGameJam2026.MVVM.Models.Dating.Data;
using kekchpek.Auxiliary.Contexts;
using UnityMVVM.ViewModelCore;

namespace GlobalGameJam2026.MVVM.Views.DatingScreen
{
    public class DatingScreenViewModel : ViewModel, 
        IDatingScreenViewModel, 
        IContextedViewModel<GirlReactionContext>,
        IContextSelectorViewModel<GirlReactionContext>
    {
        private const float ReactionDisplayDelaySeconds = 2f;
        
        private readonly ViewModelContext<GirlReactionContext> _girlReactionContext;
        private readonly IDatingService _datingService;
        private readonly IDatingModel _datingModel;

        public event Action<GirlReactionContext> ContextSelected;

        public DatingScreenViewModel(IDatingService datingService, IDatingModel datingModel)
        {
            _datingService = datingService;
            _datingModel = datingModel;
            _datingService.SelectNextQuestion();
            _girlReactionContext = new ViewModelContext<GirlReactionContext>(this, GirlReactionContext.None);
        }

        public void SetContext(GirlReactionContext context)
        {
            if (context.Reaction == GirlReaction.None)
            {
                return;
            }
            
            HandleReactionAsync().Forget();
        }

        private async UniTaskVoid HandleReactionAsync()
        {
            await UniTask.WaitForSeconds(ReactionDisplayDelaySeconds);
            
            if (_datingModel.GameState.Value == DatingGameState.Playing)
            {
                ContextSelected?.Invoke(GirlReactionContext.None);
                _datingService.SelectNextQuestion();
            }
        }

        protected override void OnDestroyInternal()
        {
            base.OnDestroyInternal();
            _girlReactionContext.Dispose();
        }
    }
}