using System;
using System.Collections.Generic;
using AsyncReactAwait.Bindable;
using AsyncReactAwait.Bindable.BindableExtensions;
using GlobalGameJam2026.MVVM.Models.Dating;
using GlobalGameJam2026.MVVM.Models.Dating.Data;
using GlobalGameJam2026.MVVM.Views.DatingScreen;
using kekchpek.Auxiliary.Contexts;
using UnityMVVM.ViewModelCore;

namespace GlobalGameJam2026.MVVM.Views.DialogueOptions
{
    public class DialogueOptionsViewModel : ViewModel, 
        IDialogueOptionsViewModel, 
        IContextSelectorViewModel<GirlReactionContext>
    {
        private readonly IDatingModel _datingModel;
        private readonly IDatingService _datingService;
        private readonly IMutable<IReadOnlyList<DialogueOptionData>> _options = new Mutable<IReadOnlyList<DialogueOptionData>>(Array.Empty<DialogueOptionData>());

        public IBindable<IReadOnlyList<DialogueOptionData>> Options => _options;
        
        public event Action<GirlReactionContext> ContextSelected;

        public DialogueOptionsViewModel(
            IDatingModel datingModel,
            IDatingService datingService)
        {
            _datingModel = datingModel;
            _datingService = datingService;
            
            _datingModel.CurrentQuestion.Bind(OnCurrentQuestionChanged);
        }

        private void OnCurrentQuestionChanged(DialogueQuestionData question)
        {
            if (question != null)
            {
                _options.Set(question.Options);
            }
            else
            {
                _options.Set(Array.Empty<DialogueOptionData>());
            }
        }

        public void SelectOption(int optionIndex)
        {
            var currentQuestion = _datingModel.CurrentQuestion.Value;
            if (currentQuestion == null || optionIndex < 0 || optionIndex >= currentQuestion.Options.Count)
            {
                return;
            }
            
            var selectedOption = currentQuestion.Options[optionIndex];
            var isCorrect = _datingService.SelectAnswer(optionIndex);
            
            // Hide options until next question appears
            _options.Set(Array.Empty<DialogueOptionData>());
            
            // Check if game ended after this answer
            var gameState = _datingModel.GameState.Value;
            if (gameState == DatingGameState.Won)
            {
                var endDialogue = _datingService.GetEndDialogue(true);
                var context = new GirlReactionContext(GirlReaction.Win, endDialogue);
                ContextSelected?.Invoke(context);
            }
            else if (gameState == DatingGameState.Lost)
            {
                var endDialogue = _datingService.GetEndDialogue(false);
                var context = new GirlReactionContext(GirlReaction.Lose, endDialogue);
                ContextSelected?.Invoke(context);
            }
            else
            {
                var reaction = isCorrect ? GirlReaction.Good : GirlReaction.Bad;
                var context = new GirlReactionContext(reaction, selectedOption.ReactionText);
                ContextSelected?.Invoke(context);
            }
        }

        protected override void OnDestroyInternal()
        {
            base.OnDestroyInternal();
            _datingModel.CurrentQuestion.Unbind(OnCurrentQuestionChanged);
        }
    }
}
