using Cysharp.Threading.Tasks;

namespace GlobalGameJam2026.MVVM.Models.Dating
{
    public interface IDatingService
    {
        UniTask Initialize();
        bool SelectAnswer(int optionIndex); 
        void SelectNextQuestion();
        string GetEndDialogue(bool won);
        void MaskSwap();
    }
}
