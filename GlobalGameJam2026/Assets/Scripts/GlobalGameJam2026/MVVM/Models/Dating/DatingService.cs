using System.Collections.Generic;
using System.Linq;
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

            _model.SetMaxRedFlags(_config.MaxRedFlags);
            _model.SetMaxQuestions(_config.MaxQuestions);
            _availableQuestions = new List<DialogueQuestionData>(_config.Questions);
        }

        public bool SelectAnswer(int optionIndex)
        {
            var currentQuestion = _model.CurrentQuestion.Value;
            if (currentQuestion == null || optionIndex < 0 || optionIndex >= currentQuestion.Options.Count)
            {
                return false;
            }

            var selectedOption = currentQuestion.Options[optionIndex];
            var isCorrect = selectedOption.IsCorrect;

            if (isCorrect)
            {
                _model.AddGreenFlag();
            }
            else
            {
                _model.AddRedFlag(currentQuestion.Id);
            }

            _model.IncrementQuestionsAnswered();

            if (_model.RedFlagCount.Value >= _model.MaxRedFlags.Value)
            {
                _model.SetGameState(DatingGameState.Lost);
            }
            else if (_model.QuestionsAnswered.Value >= _model.MaxQuestions.Value)
            {
                _model.SetGameState(DatingGameState.Won);
            }

            return isCorrect;
        }

        public void SelectNextQuestion()
        {
            if (_availableQuestions == null || _availableQuestions.Count == 0)
            {
                _availableQuestions = new List<DialogueQuestionData>(_config.Questions);
            }

            var unusedQuestions = _availableQuestions.FindAll(q => !_model.UsedQuestionIds.Contains(q.Id));

            if (unusedQuestions.Count == 0)
            {
                _model.ClearUsedQuestionIds();
                unusedQuestions = _availableQuestions;
            }

            var randomIndex = Random.Range(0, unusedQuestions.Count);
            var selectedQuestion = unusedQuestions[randomIndex];

            _model.AddUsedQuestionId(selectedQuestion.Id);
            _model.SetCurrentQuestion(selectedQuestion);
        }

        public string GetEndDialogue(bool won)
        {
            return won 
            ? _config.WinDialogues[Random.Range(0, _config.WinDialogues.Count)] 
            : _config.LoseDialogues[Random.Range(0, _config.LoseDialogues.Count)];
        }

        public void MaskSwap()
        {
            _model.IncrementLoseCount();
            
            if (_model.IsGameOver.Value)
            {
                RestartGame();
                return;
            }
            
            var redFlagIds = _model.RedFlagQuestionIds.Take(2).ToList();
            
            _model.RemoveUsedQuestionIds(redFlagIds);
            _model.ClearRedFlagQuestionIds();
            _model.ResetProgress();
            _model.SetGameState(DatingGameState.Playing);
        }

        private void RestartGame()
        {
            _model.RestartGame();            
            _availableQuestions = new List<DialogueQuestionData>(_config.Questions);
        }
    }
}
