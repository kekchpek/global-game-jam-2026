using System;
using Cysharp.Threading.Tasks;
using GlobalGameJam2026.MVVM.Views.RedFlagsIndicator.Components;
using UnityEngine;
using UnityMVVM;

namespace GlobalGameJam2026.MVVM.Views.RedFlagsIndicator
{
    public class RedFlagsIndicatorView : ViewBehaviour<IRedFlagsIndicatorViewModel>
    {
        [SerializeField] private RedFlagStepComponent _stepPrefab;
        [SerializeField] private Transform _stepsContainer;
        
        private RedFlagStepComponent[] _stepsObjects = Array.Empty<RedFlagStepComponent>();
        private int _currentStepIndex = 0;

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            SmartBind(ViewModel.TotalStepsCount, OnTotalStepsCountChanged);
        }

        private void OnTotalStepsCountChanged()
        {
            foreach (var step in _stepsObjects)
            {
                Destroy(step.gameObject);
            }
            _stepsObjects = new RedFlagStepComponent[ViewModel.TotalStepsCount.Value];
            for (int i = 0; i < ViewModel.TotalStepsCount.Value; i++)
            {
                _stepsObjects[i] = Instantiate(_stepPrefab, _stepsContainer);
            }
            
            // Set first step as current
            _currentStepIndex = 0;
            if (_stepsObjects.Length > 0)
            {
                _stepsObjects[0].SetCurrentInstant();
            }
        }

        /// <summary>
        /// Shows the result (checkmark or red flag) with animation on the current step.
        /// </summary>
        public async UniTask ShowResult(bool isCorrect)
        {
            if (_currentStepIndex >= _stepsObjects.Length)
            {
                return;
            }
            
            var currentStep = _stepsObjects[_currentStepIndex];
            
            // Animate the current step to correct or incorrect
            if (isCorrect)
            {
                await currentStep.SetCorrect();
            }
            else
            {
                await currentStep.SetIncorrect();
            }
            
            // Move to next step
            _currentStepIndex++;
            
            // Set next step as current (if exists)
            if (_currentStepIndex < _stepsObjects.Length)
            {
                await _stepsObjects[_currentStepIndex].SetCurrent();
            }
        }
    }
}