using System;
using Newtonsoft.Json;

namespace TestMakerFree.ViewModels
{
    [JsonObject(MemberSerialization.OptOut)]
    public class QuestionViewModel
    {
        #region Constructor
        public QuestionViewModel(){

        }
        #endregion

        #region Properties
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string Text { get; set; }
        public string Notes { get; set; }
        public int Type { get; set; } = 1;
        public int Flags { get; set; } = 1;
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        #endregion
    }
}