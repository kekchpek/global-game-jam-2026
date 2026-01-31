using System;
using System.Collections.Generic;
using AsyncReactAwait.Bindable;
using GlobalGameJam2026.MVVM.Models.Dating.Data;
using UnityMVVM.ViewModelCore;

namespace GlobalGameJam2026.MVVM.Views.DatingScreen
{
    public interface IDatingScreenViewModel : IViewModel
    {
        /// <summary>
        /// Current question text.
        /// </summary>
        IBindable<string> CurrentQuestionText { get; }
        
        /// <summary>
        /// Current options.
        /// </summary>
        IBindable<IReadOnlyList<DialogueOptionData>> CurrentOptions { get; }
        
        /// <summary>
        /// Event fired when answer flow should start.
        /// Parameters: isCorrect, reactionText, nextQuestionText, nextOptions, isGameEnd, isWin
        /// </summary>
        event Action<AnswerFlowData> AnswerFlowStarted;
        
        /// <summary>
        /// Selects an answer option.
        /// </summary>
        void SelectOption(int optionIndex);
        
        /// <summary>
        /// Called when the answer flow animation sequence is complete.
        /// </summary>
        void OnAnswerFlowComplete();
    }
    
    public class AnswerFlowData
    {
        public bool IsCorrect { get; }
        public string ReactionText { get; }
        public string NextQuestionText { get; }
        public IReadOnlyList<DialogueOptionData> NextOptions { get; }
        public bool IsGameEnd { get; }
        public bool IsWin { get; }
        
        public AnswerFlowData(
            bool isCorrect, 
            string reactionText, 
            string nextQuestionText,
            IReadOnlyList<DialogueOptionData> nextOptions,
            bool isGameEnd,
            bool isWin)
        {
            IsCorrect = isCorrect;
            ReactionText = reactionText;
            NextQuestionText = nextQuestionText;
            NextOptions = nextOptions;
            IsGameEnd = isGameEnd;
            IsWin = isWin;
        }
    }
}
