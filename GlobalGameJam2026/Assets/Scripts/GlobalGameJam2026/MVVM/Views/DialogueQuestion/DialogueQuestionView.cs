using Cysharp.Threading.Tasks;
using kekchpek.Auxiliary;
using TMPro;
using UnityEngine;
using UnityMVVM;

namespace GlobalGameJam2026.MVVM.Views.DialogueQuestion
{
    public class DialogueQuestionView : ViewBehaviour<IDialogueQuestionViewModel>
    {
        private const string ShowSequence = "Show";
        private const string HideSequence = "Hide";
        private const float TypingSpeed = 0.03f;
        
        [SerializeField] private TextMeshProUGUI _displayText;
        [SerializeField] private AnimationController _animationController;
        [SerializeField] private GameObject _bubbleContainer;

        /// <summary>
        /// Shows the bubble with animation.
        /// </summary>
        public async UniTask ShowBubble()
        {
            _bubbleContainer.SetActive(true);
            
            if (_animationController != null && _animationController.HasSequence(ShowSequence))
            {
                await _animationController.PlaySequence(ShowSequence);
            }
        }

        /// <summary>
        /// Hides the bubble with animation.
        /// </summary>
        public async UniTask HideBubble()
        {
            if (_animationController != null && _animationController.HasSequence(HideSequence))
            {
                await _animationController.PlaySequence(HideSequence);
            }
            
            _bubbleContainer.SetActive(false);
            _displayText.text = string.Empty;
        }

        /// <summary>
        /// Types text with animation.
        /// </summary>
        public async UniTask TypeText(string text)
        {
            _displayText.text = string.Empty;
            
            foreach (char c in text)
            {
                _displayText.text += c;
                await UniTask.WaitForSeconds(TypingSpeed);
            }
        }

        /// <summary>
        /// Sets text instantly.
        /// </summary>
        public void SetText(string text)
        {
            _displayText.text = text;
        }
    }
}
