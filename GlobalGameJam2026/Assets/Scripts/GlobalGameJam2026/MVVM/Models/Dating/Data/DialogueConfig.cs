using Newtonsoft.Json;
using System.Collections.Generic;

namespace GlobalGameJam2026.MVVM.Models.Dating.Data
{
    public class DialogueConfig
    {
        [JsonProperty]
        private int maxRedFlags;
        [JsonProperty]
        private int maxQuestions;
        [JsonProperty]
        private List<DialogueQuestionData> questions;


        public int MaxRedFlags => maxRedFlags;
        public int MaxQuestions => maxQuestions;
        public List<DialogueQuestionData> Questions => questions;
    }
}