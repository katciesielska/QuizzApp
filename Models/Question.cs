namespace QuizzApp.Models
{
    public class Question
    {
        public Guid Id { get; set; }
        public required string Text { get; set; }

        //Options
        public required List<Option> Options { get; set; }
        public required Guid CorrectOption { get; set; }
    }
}
