using System.Collections.Generic;
using AsyncReactAwait.Bindable;
using GlobalGameJam2026.MVVM.Models.Dating.Data;
using kekchpek.Auxiliary.ReactiveList;
using kekchpek.GameSaves;
using kekchpek.SaveSystem;

namespace GlobalGameJam2026.MVVM.Models.Dating
{
    public class DatingModel : IDatingMutableModel
    {
        private readonly IGameSaveManager _gameSaveManager;
        private ISaveDataProvider _saveDataProvider;

        private readonly Mutable<DialogueQuestionData> _currentQuestion = new();
        private readonly Mutable<int> _maxRedFlags = new(0);
        private readonly Mutable<int> _maxQuestions = new(0);
        private readonly Mutable<DatingGameState> _gameState = new(DatingGameState.Playing);

        private IMutable<int> _greenFlagCount;
        private IMutable<int> _redFlagCount;
        private IMutable<int> _questionsAnswered;
        private IMutable<string> _currentQuestionId;
        private IMutable<MutableList<bool>> _answeredQuestions;
        private IMutable<MutableList<string>> _usedQuestionIds;

        public IBindable<DialogueQuestionData> CurrentQuestion => _currentQuestion;
        public IBindable<int> GreenFlagCount => GetOrCreateSavedInt(ref _greenFlagCount, "GreenFlagCount", 0);
        public IBindable<int> RedFlagCount => GetOrCreateSavedInt(ref _redFlagCount, "RedFlagCount", 0);
        public IBindable<int> MaxRedFlags => _maxRedFlags;
        public IBindable<int> MaxQuestions => _maxQuestions;
        public IBindable<int> QuestionsAnswered => GetOrCreateSavedInt(ref _questionsAnswered, "QuestionsAnswered", 0);
        public IBindable<DatingGameState> GameState => _gameState;
        public IBindableList<bool> AnsweredQuestions => GetOrCreateSavedBoolList().Value;
        public IReadOnlyCollection<string> UsedQuestionIds => GetOrCreateSavedStringList().Value;

        public DatingModel(IGameSaveManager gameSaveManager)
        {
            _gameSaveManager = gameSaveManager;
            //_gameSaveManager.IsInitialized.Bind(OnSaveInitialized);
        }

        private void OnSaveInitialized(bool initialized)
        {
            if (initialized)
            {
                _saveDataProvider = _gameSaveManager.GameDataProvider;
            }
        }

        public void SetCurrentQuestion(DialogueQuestionData question)
        {
            _currentQuestion.Value = question;
            if (question != null)
            {
                GetOrCreateSavedString(ref _currentQuestionId, "CurrentQuestionId").Value = question.Id;
            }
        }

        public void AddGreenFlag()
        {
            GetOrCreateSavedInt(ref _greenFlagCount, "GreenFlagCount", 0).Value++;
            GetOrCreateSavedBoolList().Value.Add(true);
        }

        public void AddRedFlag()
        {
            GetOrCreateSavedInt(ref _redFlagCount, "RedFlagCount", 0).Value++;
            GetOrCreateSavedBoolList().Value.Add(false);
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
            GetOrCreateSavedInt(ref _questionsAnswered, "QuestionsAnswered", 0).Value++;
        }

        public void SetGameState(DatingGameState state)
        {
            _gameState.Value = state;
        }

        public void AddUsedQuestionId(string questionId)
        {
            GetOrCreateSavedStringList().Value.Add(questionId);
        }

        private IMutable<int> GetOrCreateSavedInt(ref IMutable<int> field, string key, int defaultValue)
        {
            if (field == null)
            {
                if (_saveDataProvider != null)
                {
                    field = _saveDataProvider.DeserializeAndCaptureStructValue($"Dating/{key}", defaultValue);
                }
                else
                {
                    field = new Mutable<int>(defaultValue);
                }
            }
            return field;
        }

        private IMutable<string> GetOrCreateSavedString(ref IMutable<string> field, string key)
        {
            if (field == null)
            {
                if (_saveDataProvider != null)
                {
                    field = _saveDataProvider.DeserializeAndCaptureCustomValue<string>($"Dating/{key}", () => null);
                }
                else
                {
                    field = new Mutable<string>(null);
                }
            }
            return field;
        }

        private IMutable<MutableList<bool>> GetOrCreateSavedBoolList()
        {
            if (_answeredQuestions == null)
            {
                if (_saveDataProvider != null)
                {
                    _answeredQuestions = _saveDataProvider.DeserializeAndCaptureCustomValue<MutableList<bool>>(
                        "Dating/AnsweredQuestions", () => new MutableList<bool>());
                }
                else
                {
                    _answeredQuestions = new Mutable<MutableList<bool>>(new MutableList<bool>());
                }
            }
            return _answeredQuestions;
        }

        private IMutable<MutableList<string>> GetOrCreateSavedStringList()
        {
            if (_usedQuestionIds == null)
            {
                if (_saveDataProvider != null)
                {
                    _usedQuestionIds = _saveDataProvider.DeserializeAndCaptureCustomValue<MutableList<string>>(
                        "Dating/UsedQuestionIds", () => new MutableList<string>());
                }
                else
                {
                    _usedQuestionIds = new Mutable<MutableList<string>>(new MutableList<string>());
                }
            }
            return _usedQuestionIds;
        }
    }
}
