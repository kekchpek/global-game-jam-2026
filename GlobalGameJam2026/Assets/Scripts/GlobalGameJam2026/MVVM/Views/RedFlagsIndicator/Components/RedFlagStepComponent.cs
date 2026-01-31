using Cysharp.Threading.Tasks;
using kekchpek.Auxiliary;
using UnityEngine;

namespace GlobalGameJam2026.MVVM.Views.RedFlagsIndicator.Components
{
    public enum RedFlagState
    {
        None,
        Current,
        Correct,
        Incorrect
    }
    
    public class RedFlagStepComponent : MonoBehaviour
    {
        [SerializeField] private AnimationController _animationController;
        [SerializeField] private GameObject _currentLayout;
        [SerializeField] private GameObject _correctLayout;
        [SerializeField] private GameObject _incorrectLayout;
        
        public RedFlagState CurrentState { get; private set; } = RedFlagState.None;

        public async UniTask SetCurrent()
        {
            if (CurrentState == RedFlagState.Current) return;
            CurrentState = RedFlagState.Current;
            
            HideOverlays();
            _currentLayout.SetActive(true);
            
            await PlayAnimation("Current");
        }

        public async UniTask SetCorrect()
        {
            if (CurrentState == RedFlagState.Correct) return;
            CurrentState = RedFlagState.Correct;
            
            // Keep CurrentLayout visible, only hide incorrect
            _incorrectLayout.SetActive(false);
            _correctLayout.SetActive(true);
            
            await PlayAnimation("Correct");
        }
        
        public async UniTask SetIncorrect()
        {
            if (CurrentState == RedFlagState.Incorrect) return;
            CurrentState = RedFlagState.Incorrect;
            
            // Keep CurrentLayout visible, only hide correct
            _correctLayout.SetActive(false);
            _incorrectLayout.SetActive(true);
            
            await PlayAnimation("Incorrect");
        }

        public void SetCurrentInstant()
        {
            CurrentState = RedFlagState.Current;
            HideOverlays();
            _currentLayout.SetActive(true);
        }

        public void SetCorrectInstant()
        {
            CurrentState = RedFlagState.Correct;
            // Keep CurrentLayout visible
            _incorrectLayout.SetActive(false);
            _currentLayout.SetActive(true);
            _correctLayout.SetActive(true);
        }

        public void SetIncorrectInstant()
        {
            CurrentState = RedFlagState.Incorrect;
            // Keep CurrentLayout visible
            _correctLayout.SetActive(false);
            _currentLayout.SetActive(true);
            _incorrectLayout.SetActive(true);
        }

        public void SetNotReached()
        {
            CurrentState = RedFlagState.None;
            HideOverlays();
        }

        private void HideOverlays()
        {
            _currentLayout.SetActive(false);
            _correctLayout.SetActive(false);
            _incorrectLayout.SetActive(false);
        }

        private async UniTask PlayAnimation(string animationName)
        {
            if (_animationController != null && _animationController.HasSequence(animationName))
            {
                _animationController.InterruptCurrentAnimation();
                await _animationController.PlaySequence(animationName);
            }
        }
    }
}