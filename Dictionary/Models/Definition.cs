using System;

namespace Dictionary.Models
{
    public class Definition
    {
        public Int64 Id { get; set; }
        public string EnglishWord { get; set; }
        public string PartOfSpeech { get; set; }
        public string MalayalamDefinition { get; set; }
    }
}