namespace Personal.Blog.Domain.ConfigModels
{
    public class BlogDbSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string PostsCollectionName { get; set; } = null!;
    }
}
