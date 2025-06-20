namespace QuizzApp.Models
{
    public class Question
    {
        public Guid Id { get; set; }
        public required string Text { get; set; }

        //Quiz relationship
        public Guid QuizId { get; set; }
        public Quiz? Quiz { get; set; } // Navigation property to the Quiz

        //Options
        public required List<Option> Options { get; set; }
    }
}
