using AsyncReactAwait.Bindable;
using GlobalGameJam2026.MVVM.Models.Dating.Data;

namespace GlobalGameJam2026.MVVM.Models.Dating
{
    public class DatingModel : IDatingMutableModel
    {
        private readonly Mutable<DialogueQuestionData> _currentQuestion = new();
        private readonly Mutable<int> _greenFlagCount = new(0);
        private readonly Mutable<int> _redFlagCount = new(0);
        private readonly Mutable<int> _maxRedFlags = new(0);
        private readonly Mutable<int> _maxQuestions = new(0);
        private readonly Mutable<int> _questionsAnswered = new(0);
        private readonly Mutable<DatingGameState> _gameState = new(DatingGameState.Playing);

        public IBindable<DialogueQuestionData> CurrentQuestion => _currentQuestion;
        public IBindable<int> GreenFlagCount => _greenFlagCount;
        public IBindable<int> RedFlagCount => _redFlagCount;
        public IBindable<int> MaxRedFlags => _maxRedFlags;
        public IBindable<int> MaxQuestions => _maxQuestions;
        public IBindable<int> QuestionsAnswered => _questionsAnswered;
        public IBindable<DatingGameState> GameState => _gameState;

        public void SetCurrentQuestion(DialogueQuestionData question)
        {
            _currentQuestion.Value = question;
        }

        public void AddGreenFlag()
        {
            _greenFlagCount.Value++;
        }

        public void AddRedFlag()
        {
            _redFlagCount.Value++;
        }

        public void SetMaxRedFlags(int maxRedFlags)
        {
            _maxRedFlags.Value = maxRedFlags;
        }

        public void SetMaxQuestions(int maxQuestions)
        {
            _maxQuestions.Value = maxQuestions;
        }

        public void IncrementQuestionsAnswered()
        {
            _questionsAnswered.Value++;
        }

        public void SetGameState(DatingGameState state)
        {
            _gameState.Value = state;
        }

        public void Reset()
        {
            _currentQuestion.Value = null;
            _greenFlagCount.Value = 0;
            _redFlagCount.Value = 0;
            _questionsAnswered.Value = 0;
            _gameState.Value = DatingGameState.Playing;
        }
    }
}
