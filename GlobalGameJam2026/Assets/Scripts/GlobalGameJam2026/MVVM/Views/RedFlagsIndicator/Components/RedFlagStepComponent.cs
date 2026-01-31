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
        [SerializeField] private GameObject _unreachedLayout;
        [SerializeField] private GameObject _currentLayout;
        [SerializeField] private GameObject _correctLayout;
        [SerializeField] private GameObject _incorrectLayout;
        
        public RedFlagState CurrentState { get; private set; } = RedFlagState.None;

        public async UniTask SetCurrent()
        {
            if (CurrentState == RedFlagState.Current) return;
            CurrentState = RedFlagState.Current;
            
            HideAll();
            _currentLayout.SetActive(true);
            
            await PlayAnimation("Current");
        }

        public async UniTask SetCorrect()
        {
            if (CurrentState == RedFlagState.Correct) return;
            CurrentState = RedFlagState.Correct;
            
            HideAll();
            _correctLayout.SetActive(true);
            
            await PlayAnimation("Correct");
        }
        
        public async UniTask SetIncorrect()
        {
            if (CurrentState == RedFlagState.Incorrect) return;
            CurrentState = RedFlagState.Incorrect;
            
            HideAll();
            _incorrectLayout.SetActive(true);
            
            await PlayAnimation("Incorrect");
        }

        public void SetCurrentInstant()
        {
            CurrentState = RedFlagState.Current;
            HideAll();
            _currentLayout.SetActive(true);
        }

        public void SetNotReached()
        {
            CurrentState = RedFlagState.None;
            HideAll();
            _unreachedLayout.SetActive(true);
        }

        private void HideAll()
        {
            _unreachedLayout.SetActive(false);
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