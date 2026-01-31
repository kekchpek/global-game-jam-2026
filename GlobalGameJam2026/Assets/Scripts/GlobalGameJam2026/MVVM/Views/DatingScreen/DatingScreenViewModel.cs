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
        private const float EndGameDisplayDelaySeconds = 3f;
        
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
            
            HandleReactionAsync(context).Forget();
        }

        private async UniTaskVoid HandleReactionAsync(GirlReactionContext context)
        {
            await UniTask.WaitForSeconds(ReactionDisplayDelaySeconds);
            
            // If this is already an end game context, close the view after delay
            if (context.Reaction == GirlReaction.Win || context.Reaction == GirlReaction.Lose)
            {
                await UniTask.WaitForSeconds(EndGameDisplayDelaySeconds);
                await Close();
                return;
            }
            
            var gameState = _datingModel.GameState.Value;
            
            if (gameState == DatingGameState.Won)
            {
                var endDialogue = _datingService.GetEndDialogue(true);
                var endContext = new GirlReactionContext(GirlReaction.Win, endDialogue);
                ContextSelected?.Invoke(endContext);
            }
            else if (gameState == DatingGameState.Lost)
            {
                var endDialogue = _datingService.GetEndDialogue(false);
                var endContext = new GirlReactionContext(GirlReaction.Lose, endDialogue);
                ContextSelected?.Invoke(endContext);
            }
            else
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
