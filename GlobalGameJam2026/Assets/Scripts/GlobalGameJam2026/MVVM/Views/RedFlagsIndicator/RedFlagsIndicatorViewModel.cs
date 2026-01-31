using System.Collections.Generic;
using AsyncReactAwait.Bindable;
using AsyncReactAwait.Bindable.BindableExtensions;
using GlobalGameJam2026.MVVM.Models.Dating;
using UnityMVVM.ViewModelCore;

namespace GlobalGameJam2026.MVVM.Views.RedFlagsIndicator
{
    public class RedFlagsIndicatorViewModel : ViewModel, IRedFlagsIndicatorViewModel
    {
        private readonly IDatingModel _datingModel;
        private readonly IMutable<IReadOnlyList<bool>> _steps = new Mutable<IReadOnlyList<bool>>(new List<bool>());
        private readonly IMutable<int> _totalStepsCount = new Mutable<int>(0);

        public IBindable<IReadOnlyList<bool>> Steps => _steps;
        public IBindable<int> TotalStepsCount => _totalStepsCount;

        public RedFlagsIndicatorViewModel(IDatingModel datingModel)
        {
            _datingModel = datingModel;
            _datingModel.MaxQuestions.Bind(OnMaxQuestionsChanged);
            _datingModel.AnsweredQuestions.LastAdded.Bind(OnAnsweredQuestionsChanged);
        }

        private void OnMaxQuestionsChanged(int maxQuestions)
        {
            _totalStepsCount.Set(maxQuestions);
        }

        private void OnAnsweredQuestionsChanged(bool _)
        {
            _steps.ForceSet(_datingModel.AnsweredQuestions);
        }

        protected override void OnDestroyInternal()
        {
            base.OnDestroyInternal();
            _datingModel.MaxQuestions.Unbind(OnMaxQuestionsChanged);
            _datingModel.AnsweredQuestions.LastAdded.Unbind(OnAnsweredQuestionsChanged);
        }
    }
}