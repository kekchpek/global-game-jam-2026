using System;
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

        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            SmartBind(ViewModel.TotalStepsCount, OnTotalStepsCountChanged);
            SmartBind(ViewModel.Steps, OnStepsChanged);
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
            UpdateSteps();
        }

        private void OnStepsChanged()
        {
            UpdateSteps();
        }

        private void UpdateSteps()
        {
            var stepsState = ViewModel.Steps.Value;
            for (int i = 0; i < _stepsObjects.Length; i++)
            {
                if (i < stepsState.Count)
                {
                    if (stepsState[i])
                    {
                        _stepsObjects[i].SetCorrect();
                    }
                    else
                    {
                        _stepsObjects[i].SetIncorrect();
                    }
                }
                else if (i == stepsState.Count)
                {
                    _stepsObjects[i].SetCurrent();
                }
            }
        }
    }
}