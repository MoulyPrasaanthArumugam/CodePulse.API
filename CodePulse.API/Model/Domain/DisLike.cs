namespace CodePulse.API.Model.Domain
{
    public class DisLike
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }

        public Guid ContentId { get; set; }

        public Content Content { get; set; }
    }
}
