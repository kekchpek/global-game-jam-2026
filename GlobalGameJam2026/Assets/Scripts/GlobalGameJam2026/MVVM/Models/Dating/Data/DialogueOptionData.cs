using Newtonsoft.Json;

namespace GlobalGameJam2026.MVVM.Models.Dating.Data
{
    public class DialogueOptionData
    {
        [JsonProperty]
        private string questionText;

        [JsonProperty]
        private bool isCorrect;

        [JsonProperty]
        private string reactionText;

        public string QuestionText => questionText;
        public bool IsCorrect => isCorrect;
        public string ReactionText => reactionText;
    }
}
