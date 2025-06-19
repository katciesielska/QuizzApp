namespace QuizzApp.Models
{
    public class Option
    {
        public Guid Id { get; set; }
        public required string Text { get; set; }

        // FK to the Question
        public Guid QuestionId { get; set; }
    }
}
