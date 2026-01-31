using System;
using System.Collections.Generic;
using AsyncReactAwait.Bindable;
using AsyncReactAwait.Bindable.BindableExtensions;
using GlobalGameJam2026.MVVM.Models.Dating;
using GlobalGameJam2026.MVVM.Models.Dating.Data;
using GlobalGameJam2026.Static;
using UnityMVVM.ViewManager;
using UnityMVVM.ViewModelCore;

namespace GlobalGameJam2026.MVVM.Views.DatingScreen
{
    public class DatingScreenViewModel : ViewModel, IDatingScreenViewModel
    {
        private readonly IDatingService _datingService;
        private readonly IDatingModel _datingModel;
        private readonly IViewManager _viewManager;
        
        private readonly IMutable<string> _currentQuestionText = new Mutable<string>(string.Empty);
        private readonly IMutable<IReadOnlyList<DialogueOptionData>> _currentOptions = 
            new Mutable<IReadOnlyList<DialogueOptionData>>(Array.Empty<DialogueOptionData>());

        public IBindable<string> CurrentQuestionText => _currentQuestionText;
        public IBindable<IReadOnlyList<DialogueOptionData>> CurrentOptions => _currentOptions;
        
        public event Action<AnswerFlowData> AnswerFlowStarted;

        public DatingScreenViewModel(
            IDatingService datingService, 
            IDatingModel datingModel,
            IViewManager viewManager)
        {
            _datingService = datingService;
            _datingModel = datingModel;
            _viewManager = viewManager;
            
            // Start the first question
            _datingService.SelectNextQuestion();
            InitializeFirstQuestion();
        }

        private void InitializeFirstQuestion()
        {
            var question = _datingModel.CurrentQuestion.Value;
            if (question != null)
            {
                _currentQuestionText.Set(question.Question);
                _currentOptions.Set(question.Options);
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
            var reactionText = selectedOption.ReactionText;
            
            var gameState = _datingModel.GameState.Value;
            
            // Check if game ended
            if (gameState == DatingGameState.Won)
            {
                var endDialogue = _datingService.GetEndDialogue(true);
                var flowData = new AnswerFlowData(isCorrect, reactionText, endDialogue, null, true, true);
                AnswerFlowStarted?.Invoke(flowData);
                return;
            }
            
            if (gameState == DatingGameState.Lost)
            {
                var endDialogue = _datingService.GetEndDialogue(false);
                var flowData = new AnswerFlowData(isCorrect, reactionText, endDialogue, null, true, false);
                AnswerFlowStarted?.Invoke(flowData);
                return;
            }
            
            // Game continues - select next question
            _datingService.SelectNextQuestion();
            var nextQuestion = _datingModel.CurrentQuestion.Value;
            var nextQuestionText = nextQuestion?.Question ?? string.Empty;
            var nextOptions = nextQuestion?.Options;
            
            var continueFlowData = new AnswerFlowData(isCorrect, reactionText, nextQuestionText, nextOptions, false, false);
            AnswerFlowStarted?.Invoke(continueFlowData);
        }

        public async void OnAnswerFlowComplete()
        {
            var gameState = _datingModel.GameState.Value;
            
            if (gameState == DatingGameState.Won)
            {
                await _viewManager.Open(LayerNames.Screen, ViewNames.WinComics);
            }
            else if (gameState == DatingGameState.Lost)
            {
                await _viewManager.Open(LayerNames.Screen, ViewNames.LoseComics);
            }
        }
    }
}
