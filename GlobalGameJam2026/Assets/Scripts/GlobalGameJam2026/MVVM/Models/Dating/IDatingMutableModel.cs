using System.Collections.Generic;
using GlobalGameJam2026.MVVM.Models.Dating.Data;

namespace GlobalGameJam2026.MVVM.Models.Dating
{
    public interface IDatingMutableModel : IDatingModel
    {
        void SetCurrentQuestion(DialogueQuestionData question);
        void AddGreenFlag();
        void AddRedFlag(string questionId);
        void SetMaxRedFlags(int maxRedFlags);
        void SetMaxQuestions(int maxQuestions);
        void IncrementQuestionsAnswered();
        void SetGameState(DatingGameState state);
        void AddUsedQuestionId(string questionId);
        void RemoveUsedQuestionIds(IEnumerable<string> questionIds);
        void ClearUsedQuestionIds();
        void ClearRedFlagQuestionIds();
        void ResetProgress();
        void IncrementLoseCount();
        void RestartGame();
    }
}
