using UnityEngine;

namespace GlobalGameJam2026.MVVM.Views.RedFlagsIndicator.Components
{
    public class RedFlagStepComponent : MonoBehaviour
    {

        [SerializeField] private GameObject _currentLayout;
        [SerializeField] private GameObject _correctLayout;
        [SerializeField] private GameObject _incorrectLayout;
        [SerializeField] private GameObject _notReachedLayout;


        public void SetCurrent() {
            _correctLayout.SetActive(false);
            _incorrectLayout.SetActive(false);
            _notReachedLayout.SetActive(false);
            _currentLayout.SetActive(true);
        }

        public void SetCorrect() {
            _currentLayout.SetActive(false);
            _correctLayout.SetActive(true);
            _incorrectLayout.SetActive(false);
            _notReachedLayout.SetActive(false);
        }
        
        public void SetIncorrect() {
            _currentLayout.SetActive(false);
            _correctLayout.SetActive(false);
            _incorrectLayout.SetActive(true);
            _notReachedLayout.SetActive(false);
        }
        
        public void SetNotReached() {
            _currentLayout.SetActive(false);
            _correctLayout.SetActive(false);
            _incorrectLayout.SetActive(false);
            _notReachedLayout.SetActive(true);
        }
    }
}