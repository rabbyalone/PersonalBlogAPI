namespace Personal.Blog.Domain.ConfigModels
{
    public class BlogDbSettings
    {
        public string MongoConnection { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string PostCollectionName { get; set; } = null!;
    }
}
