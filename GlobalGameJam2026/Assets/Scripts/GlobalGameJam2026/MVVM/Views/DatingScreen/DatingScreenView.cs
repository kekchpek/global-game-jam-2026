using Cysharp.Threading.Tasks;
using GlobalGameJam2026.MVVM.Views.DialogueOptions;
using GlobalGameJam2026.MVVM.Views.DialogueQuestion;
using GlobalGameJam2026.MVVM.Views.RedFlagsIndicator;
using UnityEngine;
using UnityEngine.UI;
using UnityMVVM;
using kekchpek.Auxiliary;
using TMPro;

namespace GlobalGameJam2026.MVVM.Views.DatingScreen
{
    public class DatingScreenView : ViewBehaviour<IDatingScreenViewModel>
    {
        [SerializeField] private DialogueQuestionView _questionView;
        [SerializeField] private DialogueOptionsView _optionsView;
        [SerializeField] private RedFlagsIndicatorView _redFlagsView;
        [SerializeField] private Button _nextButton;
        [SerializeField] private AnimationController _girlAnimController;
        [SerializeField] private string _goodReactionSequence;
        [SerializeField] private string _badReactionSequence;
        [SerializeField] private Image _fadeOverlay;
        [SerializeField] private float _fadeDuration = 0.5f;
        [SerializeField] private TextMeshProUGUI _currentDateText;

        private UniTaskCompletionSource _nextButtonTcs;

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            
            ViewModel.AnswerFlowStarted += OnAnswerFlowStarted;
            _optionsView.OptionSelected += OnOptionSelected;
            _nextButton.onClick.AddListener(OnNextButtonClicked);
            
            // Hide next button initially
            _nextButton.gameObject.SetActive(false);
            
            // Initialize fade overlay (fully opaque - screen starts black)
            if (_fadeOverlay != null)
            {
                var color = _fadeOverlay.color;
                color.a = 1f;
                _fadeOverlay.color = color;
                _fadeOverlay.gameObject.SetActive(true);
            }
            
            // Initialize first question
            SmartBind(ViewModel.CurrentQuestionText, OnCurrentQuestionTextChanged);
            SmartBind(ViewModel.CurrentOptions, OnCurrentOptionsChanged);
            SmartBind(ViewModel.CurrentDate, OnCurrentDateChanged);
            
            // Play fade-in animation
            PlayFadeIn().Forget();
        }

        private void OnNextButtonClicked()
        {
            _nextButtonTcs?.TrySetResult();
        }

        private void OnCurrentQuestionTextChanged()
        {
            _questionView.SetText(ViewModel.CurrentQuestionText.Value);
        }

        private void OnCurrentOptionsChanged()
        {
            _optionsView.SetOptions(ViewModel.CurrentOptions.Value);
        }

        private void OnCurrentDateChanged()
        {
            _currentDateText.text = "Date: " + ViewModel.CurrentDate.Value.ToString();
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
            if(_girlAnimController != null)
            {
                _girlAnimController.InterruptCurrentAnimation();
                await PlaySequence(flowData.IsCorrect);
                _girlAnimController.PlaySequenceLooped("Idle");
            }
            
            // Step 3: Show her bubble
            await _questionView.ShowBubble();
            
            // Step 4: Type her response text
            if (!string.IsNullOrEmpty(flowData.ReactionText))
            {
                await _questionView.TypeText(flowData.ReactionText);
            }

            await UniTask.WaitForSeconds(0.5f);
            
            // Step 5: Show checkmark or red flag
            await _redFlagsView.ShowResult(flowData.IsCorrect);

            // Step 5.5: Wait for Next button click
            _nextButtonTcs = new UniTaskCompletionSource();
            _nextButton.gameObject.SetActive(true);
            await _nextButtonTcs.Task;
            _nextButton.gameObject.SetActive(false);
            
            // Step 6: Type next question
            if (!string.IsNullOrEmpty(flowData.NextQuestionText))
            {
                await _questionView.TypeText(flowData.NextQuestionText);
            }

            await UniTask.WaitForSeconds(1f);
            
            // Step 7: Set and show answer options (if game continues)
            if (!flowData.IsGameEnd && flowData.NextOptions != null && flowData.NextOptions.Count > 0)
            {
                _optionsView.SetOptions(flowData.NextOptions);
                await _optionsView.ShowOptions();
            }
            else
            {
                await UniTask.WaitForSeconds(1.5f);
                _nextButtonTcs = new UniTaskCompletionSource();
                _nextButton.gameObject.SetActive(true);
                await _nextButtonTcs.Task;
                _nextButton.gameObject.SetActive(false);
            }
            
            // Fade to black before transitioning to Win/Lose comics
            if (flowData.IsGameEnd)
            {
                await PlayFadeOut();
                await UniTask.WaitForSeconds(1f);
            }
            
            // Notify ViewModel that flow is complete
            ViewModel.OnAnswerFlowComplete();
        }
        
        private async UniTask PlayFadeOut()
        {
            if (_fadeOverlay == null) return;
            
            _fadeOverlay.gameObject.SetActive(true);
            
            // Fade the overlay from alpha 0 to 1 (transparent to black)
            float elapsed = 0f;
            var color = _fadeOverlay.color;
            color.a = 0f;
            _fadeOverlay.color = color;
            
            while (elapsed < _fadeDuration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Clamp01(elapsed / _fadeDuration);
                color.a = alpha;
                _fadeOverlay.color = color;
                await UniTask.Yield();
            }
            
            // Ensure final alpha is exactly 1
            color.a = 1f;
            _fadeOverlay.color = color;
        }
        
        private async UniTaskVoid PlayFadeIn()
        {
            if (_fadeOverlay == null) return;
            
            // Fade the overlay from alpha 1 to 0 (black to transparent)
            float elapsed = 0f;
            var color = _fadeOverlay.color;
            color.a = 1f;
            _fadeOverlay.color = color;
            
            while (elapsed < _fadeDuration)
            {
                elapsed += Time.deltaTime;
                float alpha = 1f - Mathf.Clamp01(elapsed / _fadeDuration);
                color.a = alpha;
                _fadeOverlay.color = color;
                await UniTask.Yield();
            }
            
            // Ensure final alpha is exactly 0
            color.a = 0f;
            _fadeOverlay.color = color;
            _fadeOverlay.gameObject.SetActive(false);
        }

        private async UniTask PlaySequence(bool isGood)
        {
            var sequence = isGood ? _goodReactionSequence : _badReactionSequence;
            await _girlAnimController.PlaySequence(sequence);
        }

        protected override void OnViewModelClear()
        {
            ViewModel.AnswerFlowStarted -= OnAnswerFlowStarted;
            base.OnViewModelClear();
        }

        protected override void OnDestroy()
        {
            _optionsView.OptionSelected -= OnOptionSelected;
            _nextButton.onClick.RemoveListener(OnNextButtonClicked);
            base.OnDestroy();
        }
    }
}
