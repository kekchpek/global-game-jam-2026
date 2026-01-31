using UnityEngine;
using kekchpek.Auxiliary;

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
        
        public RedFlagState CurrentState { get; private set; } = RedFlagState.None;

        public void SetCurrent() {
            if (CurrentState == RedFlagState.Current) return;
            CurrentState = RedFlagState.Current;
            PlayAnimation("Current");
        }

        public void SetCorrect() {
            if (CurrentState == RedFlagState.Correct) return;
            CurrentState = RedFlagState.Correct;
            PlayAnimation("Correct");
        }
        
        public void SetIncorrect() {
            if (CurrentState == RedFlagState.Incorrect) return;
            CurrentState = RedFlagState.Incorrect;
            PlayAnimation("Incorrect");
        }

        private void PlayAnimation(string animationName) {
            _animationController.InterruptCurrentAnimation();
            _animationController.PlaySequence(animationName);
        }
    }
}