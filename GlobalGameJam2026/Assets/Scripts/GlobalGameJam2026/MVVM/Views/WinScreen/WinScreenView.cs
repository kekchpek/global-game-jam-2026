using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityMVVM;

namespace GlobalGameJam2026.MVVM.Views.WinScreen
{
    public class WinScreenView : ViewBehaviour<IWinScreenViewModel>
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Image _fadeOverlay;
        [SerializeField] private float _fadeDuration = 0.5f;

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            _restartButton.onClick.AddListener(OnRestartButtonClicked);
            
            // Initialize fade overlay (fully opaque - screen starts black)
            if (_fadeOverlay != null)
            {
                var color = _fadeOverlay.color;
                color.a = 1f;
                _fadeOverlay.color = color;
                _fadeOverlay.gameObject.SetActive(true);
            }
            
            // Play fade-in animation
            PlayFadeIn().Forget();
        }

        private void OnRestartButtonClicked()
        {
            OnRestartClickedAsync().Forget();
        }
        
        private async UniTaskVoid OnRestartClickedAsync()
        {
            await PlayFadeOut();
            ViewModel.OnRestartClicked();
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

        protected override void OnDestroy()
        {
            _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
            base.OnDestroy();
        }
    }
}
