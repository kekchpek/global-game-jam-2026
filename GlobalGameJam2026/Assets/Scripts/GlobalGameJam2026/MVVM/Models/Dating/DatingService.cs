using System.Collections.Generic;
using AssetsSystem;
using Cysharp.Threading.Tasks;
using GlobalGameJam2026.MVVM.Models.Dating.Data;
using Newtonsoft.Json;
using UnityEngine;

namespace GlobalGameJam2026.MVVM.Models.Dating
{
    public class DatingService : IDatingService
    {
        private const string DialogueConfigPath = "Configs/DialogueConfig";

        private readonly IDatingMutableModel _model;
        private readonly IAssetsModel _assetsModel;
        private DialogueConfig _config;
        private List<DialogueQuestionData> _availableQuestions;
        private HashSet<string> _usedQuestionIds = new HashSet<string>();

        public DatingService(
            IDatingMutableModel model,
            IAssetsModel assetsModel)
        {
            _model = model;
            _assetsModel = assetsModel;
        }

        public async UniTask Initialize()
        {
            var configJson = await _assetsModel.LoadAsset<TextAsset>(DialogueConfigPath);
            _config = JsonConvert.DeserializeObject<DialogueConfig>(configJson.text);
            _assetsModel.ReleaseLoadedAssets(DialogueConfigPath);
            _availableQuestions = new List<DialogueQuestionData>(_config.Questions);

            _model.SetMaxRedFlags(_config.MaxRedFlags);
            _model.SetMaxQuestions(_config.MaxQuestions);
            SelectNextQuestion();
        }

        public bool SelectAnswer(int optionIndex)
        {
            if (_model.GameState.Value != DatingGameState.Playing)
            {
                return;
            }

            var currentQuestion = _model.CurrentQuestion.Value;
            if (currentQuestion == null || optionIndex < 0 || optionIndex >= currentQuestion.Options.Count)
            {
                return;
            }

            var selectedOption = currentQuestion.Options[optionIndex];
            
            if (selectedOption.IsCorrect)
            {
                _model.AddGreenFlag();
            }
            else
            {
                _model.AddRedFlag();
            }

            _model.IncrementQuestionsAnswered();

            if (_model.RedFlagCount.Value >= _model.MaxRedFlags.Value)
            {
                _model.SetGameState(DatingGameState.Lost);
            }

            if (_model.QuestionsAnswered.Value >= _model.MaxQuestions.Value)
            {
                _model.SetGameState(DatingGameState.Won);
            }

            return selectedOption.IsCorrect;
        }

        public void SelectNextQuestion()
        {
            if (_availableQuestions.Count == 0)
            {
                _availableQuestions = new List<DialogueQuestionData>(_config.Questions);
                _usedQuestionIds.Clear();
            }

            var unusedQuestions = _availableQuestions.FindAll(q => !_usedQuestionIds.Contains(q.Id));
            
            if (unusedQuestions.Count == 0)
            {
                _usedQuestionIds.Clear();
                unusedQuestions = _availableQuestions;
            }

            var randomIndex = Random.Range(0, unusedQuestions.Count);
            var selectedQuestion = unusedQuestions[randomIndex];
            
            _usedQuestionIds.Add(selectedQuestion.Id);
            _model.SetCurrentQuestion(selectedQuestion);
        }
    }
}
