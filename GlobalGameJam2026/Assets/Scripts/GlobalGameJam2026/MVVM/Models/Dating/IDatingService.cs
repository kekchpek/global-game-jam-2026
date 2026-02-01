using Cysharp.Threading.Tasks;

namespace GlobalGameJam2026.MVVM.Models.Dating
{
    public interface IDatingService
    {
        bool IsGameOver { get; }
        bool WillNextLoseCauseGameOver { get; }
        UniTask Initialize();
        bool SelectAnswer(int optionIndex); 
        void SelectNextQuestion();
        string GetEndDialogue(bool won);
        void MaskSwap();
        void MarkGameOver();
        void ResetGame();
    }
}
