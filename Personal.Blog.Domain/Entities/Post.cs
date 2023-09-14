using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Personal.Blog.Domain.Entities
{
    public class Post
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? Summary { get; set; }

        public string? Slug { get; set; }
        public string? Path { get; set; }

        public DateTime? CreateDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
        public string? Layout { get; set; }
        public string? Bibliography { get; set; }
        public bool? IsDraft { get; set; } = false;
        public List<string> Tags { get; set; } = null!;
        public string Author { get; set; } = null!;
        public PrevNext Previous { get; set; } = new PrevNext();
        public PrevNext Next { get; set; } = new PrevNext();
        public string Content { get; set; } = null!;
    }

    public class PrevNext
    {
        public string Title { get; set; }
        public string Id { get; set; }
    }
}


//title: 'New features in v1'
//date: 2021 - 08 - 07T15: 32:14Z
//lastmod: '2021-02-01'
//tags: ['next-js', 'tailwind', 'guide']
//draft: false
//summary: 'An overview of the new features released in v1 - code block copy, multiple authors, frontmatter layout and more'
//layout: PostSimple
//bibliography: references - data.bib