using System.Collections.Generic;
using AsyncReactAwait.Bindable;
using GlobalGameJam2026.MVVM.Models.Dating.Data;
using kekchpek.Auxiliary.ReactiveList;

namespace GlobalGameJam2026.MVVM.Models.Dating
{
    public interface IDatingModel
    {
        IBindable<DialogueQuestionData> CurrentQuestion { get; }
        IBindable<int> GreenFlagCount { get; }
        IBindable<int> RedFlagCount { get; }
        IBindable<int> MaxRedFlags { get; }
        IBindable<int> MaxQuestions { get; }
        IBindable<int> QuestionsAnswered { get; }
        IBindable<DatingGameState> GameState { get; }
        IBindableList<bool> AnsweredQuestions { get; }
        IReadOnlyCollection<string> UsedQuestionIds { get; }
    }
}
