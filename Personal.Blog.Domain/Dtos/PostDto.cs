namespace Personal.Blog.Domain.Dtos
{
    public class PostDto
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? Summary { get; set; }

        public string? Slug { get; set; }

        public DateTime? CreateDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
        public string? Layout { get; set; }
        public string? Bibliography { get; set; }
        public bool IsDraft { get; set; }
        public List<string> Tags { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string Content { get; set; } = null!;
    }
}
