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
        private const int MaxLoseCount = 5;
        
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
        private IMutable<MutableList<string>> _redFlagQuestionIds;
        private IMutable<int> _loseCount;
        private IMutable<bool> _isGameOver;

        public IBindable<DialogueQuestionData> CurrentQuestion => _currentQuestion;
        public IBindable<int> GreenFlagCount => GetOrCreateSavedInt(ref _greenFlagCount, "GreenFlagCount", 0);
        public IBindable<int> RedFlagCount => GetOrCreateSavedInt(ref _redFlagCount, "RedFlagCount", 0);
        public IBindable<int> MaxRedFlags => _maxRedFlags;
        public IBindable<int> MaxQuestions => _maxQuestions;
        public IBindable<int> QuestionsAnswered => GetOrCreateSavedInt(ref _questionsAnswered, "QuestionsAnswered", 0);
        public IBindable<DatingGameState> GameState => _gameState;
        public IBindableList<bool> AnsweredQuestions => GetOrCreateSavedBoolList().Value;
        public IReadOnlyCollection<string> UsedQuestionIds => GetOrCreateSavedUsedQuestionIds().Value;
        public IReadOnlyCollection<string> RedFlagQuestionIds => GetOrCreateSavedRedFlagQuestionIds().Value;
        public IBindable<int> LoseCount => GetOrCreateSavedInt(ref _loseCount, "LoseCount", 0);
        public IBindable<bool> IsGameOver => GetOrCreateSavedBool(ref _isGameOver, "IsGameOver", false);

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

        public void AddRedFlag(string questionId)
        {
            GetOrCreateSavedInt(ref _redFlagCount, "RedFlagCount", 0).Value++;
            GetOrCreateSavedBoolList().Value.Add(false);
            GetOrCreateSavedRedFlagQuestionIds().Value.Add(questionId);
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
            GetOrCreateSavedUsedQuestionIds().Value.Add(questionId);
        }

        public void RemoveUsedQuestionIds(IEnumerable<string> questionIds)
        {
            var usedQuestions = GetOrCreateSavedUsedQuestionIds().Value;
            foreach (var id in questionIds)
            {
                usedQuestions.Remove(id);
            }
        }

        public void ClearUsedQuestionIds()
        {
            GetOrCreateSavedUsedQuestionIds().Value.Clear();
        }

        public void ClearRedFlagQuestionIds()
        {
            GetOrCreateSavedRedFlagQuestionIds().Value.Clear();
        }

        public void ResetProgress()
        {
            GetOrCreateSavedInt(ref _greenFlagCount, "GreenFlagCount", 0).Value = 0;
            GetOrCreateSavedInt(ref _redFlagCount, "RedFlagCount", 0).Value = 0;
            GetOrCreateSavedInt(ref _questionsAnswered, "QuestionsAnswered", 0).Value = 0;
            GetOrCreateSavedBoolList().Value.Clear();
        }

        public void IncrementLoseCount()
        {
            var loseCount = GetOrCreateSavedInt(ref _loseCount, "LoseCount", 0);
            loseCount.Value++;
            
            if (loseCount.Value >= MaxLoseCount)
            {
                GetOrCreateSavedBool(ref _isGameOver, "IsGameOver", false).Value = true;
            }
        }

        public void RestartGame()
        {
            GetOrCreateSavedInt(ref _greenFlagCount, "GreenFlagCount", 0).Value = 0;
            GetOrCreateSavedInt(ref _redFlagCount, "RedFlagCount", 0).Value = 0;
            GetOrCreateSavedInt(ref _questionsAnswered, "QuestionsAnswered", 0).Value = 0;
            GetOrCreateSavedBoolList().Value.Clear();            
            GetOrCreateSavedInt(ref _loseCount, "LoseCount", 0).Value = 0;
            GetOrCreateSavedBool(ref _isGameOver, "IsGameOver", false).Value = false;            
            GetOrCreateSavedUsedQuestionIds().Value.Clear();
            GetOrCreateSavedRedFlagQuestionIds().Value.Clear();
            _gameState.Value = DatingGameState.Playing;
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

        private IMutable<bool> GetOrCreateSavedBool(ref IMutable<bool> field, string key, bool defaultValue)
        {
            if (field == null)
            {
                if (_saveDataProvider != null)
                {
                    field = _saveDataProvider.DeserializeAndCaptureStructValue($"Dating/{key}", defaultValue);
                }
                else
                {
                    field = new Mutable<bool>(defaultValue);
                }
            }
            return field;
        }

        private IMutable<MutableList<string>> GetOrCreateSavedUsedQuestionIds()
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

        private IMutable<MutableList<string>> GetOrCreateSavedRedFlagQuestionIds()
        {
            if (_redFlagQuestionIds == null)
            {
                if (_saveDataProvider != null)
                {
                    _redFlagQuestionIds = _saveDataProvider.DeserializeAndCaptureCustomValue<MutableList<string>>(
                        "Dating/RedFlagQuestionIds", () => new MutableList<string>());
                }
                else
                {
                    _redFlagQuestionIds = new Mutable<MutableList<string>>(new MutableList<string>());
                }
            }
            return _redFlagQuestionIds;
        }
    }
}
