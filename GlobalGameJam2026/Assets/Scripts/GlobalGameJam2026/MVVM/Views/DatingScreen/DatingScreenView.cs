using Cysharp.Threading.Tasks;
using GlobalGameJam2026.MVVM.Views.DialogueOptions;
using GlobalGameJam2026.MVVM.Views.DialogueQuestion;
using GlobalGameJam2026.MVVM.Views.RedFlagsIndicator;
using UnityEngine;
using UnityMVVM;

namespace GlobalGameJam2026.MVVM.Views.DatingScreen
{
    public class DatingScreenView : ViewBehaviour<IDatingScreenViewModel>
    {
        [SerializeField] private DialogueQuestionView _questionView;
        [SerializeField] private DialogueOptionsView _optionsView;
        [SerializeField] private RedFlagsIndicatorView _redFlagsView;

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            
            ViewModel.AnswerFlowStarted += OnAnswerFlowStarted;
            _optionsView.OptionSelected += OnOptionSelected;
            
            // Initialize first question
            SmartBind(ViewModel.CurrentQuestionText, OnCurrentQuestionTextChanged);
            SmartBind(ViewModel.CurrentOptions, OnCurrentOptionsChanged);
        }

        private void OnCurrentQuestionTextChanged()
        {
            _questionView.SetText(ViewModel.CurrentQuestionText.Value);
        }

        private void OnCurrentOptionsChanged()
        {
            _optionsView.SetOptions(ViewModel.CurrentOptions.Value);
        }

        private void OnOptionSelected(int optionIndex)
        {
            ViewModel.SelectOption(optionIndex);
        }

        private void OnAnswerFlowStarted(AnswerFlowData flowData)
        {
            ExecuteAnswerFlow(flowData).Forget();
        }

        private async UniTaskVoid ExecuteAnswerFlow(AnswerFlowData flowData)
        {
            // Step 1: Hide all bubbles (options and question)
            var hideOptionsTask = _optionsView.HideOptions();
            var hideQuestionTask = _questionView.HideBubble();
            await UniTask.WhenAll(hideOptionsTask, hideQuestionTask);
            
            // Step 2: Girl reacts (skip for now - будет добавлено позже)
            
            // Step 3: Show her bubble
            await _questionView.ShowBubble();
            
            // Step 4: Type her response text
            if (!string.IsNullOrEmpty(flowData.ReactionText))
            {
                await _questionView.TypeText(flowData.ReactionText);
            }
            
            // Step 5: Show checkmark or red flag
            await _redFlagsView.ShowResult(flowData.IsCorrect);
            
            // Step 6: Type next question
            if (!string.IsNullOrEmpty(flowData.NextQuestionText))
            {
                await _questionView.TypeText(flowData.NextQuestionText);
            }
            
            // Step 7: Set and show answer options (if game continues)
            if (!flowData.IsGameEnd && flowData.NextOptions != null && flowData.NextOptions.Count > 0)
            {
                _optionsView.SetOptions(flowData.NextOptions);
                await _optionsView.ShowOptions();
            }
            
            // Notify ViewModel that flow is complete
            ViewModel.OnAnswerFlowComplete();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            ViewModel.AnswerFlowStarted -= OnAnswerFlowStarted;
            _optionsView.OptionSelected -= OnOptionSelected;
        }
    }
}
