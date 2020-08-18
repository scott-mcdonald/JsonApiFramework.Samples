using Blogging.ServiceModel;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blogging.WebService
{
    public class BloggingDataContext : DbContext
    {
        #region Constructors
        public BloggingDataContext(DbContextOptions options)
            : base(options)
        {
        }
        #endregion

        #region Tables
        public DbSet<Article> ArticleTable { get; set; }
        public DbSet<Blog>    BlogTable    { get; set; }
        public DbSet<Comment> CommentTable { get; set; }
        public DbSet<Person>  PersonTable  { get; set; }
        #endregion

        #region DbContext Overrides
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Article
            var articleModelBuilder = modelBuilder.Entity<Article>();
            ConfigureArticle(articleModelBuilder);

            // Blog
            var blogModelBuilder = modelBuilder.Entity<Blog>();
            ConfigureBlog(blogModelBuilder);

            // Comment
            var commentModelBuilder = modelBuilder.Entity<Comment>();
            ConfigureComment(commentModelBuilder);

            // Person
            var personModelBuilder = modelBuilder.Entity<Person>();
            ConfigurePerson(personModelBuilder);

            base.OnModelCreating(modelBuilder);
        }
        #endregion

        #region Private Methods
        private static void ConfigureArticle(EntityTypeBuilder<Article> builder)
        {
            // Table
            builder.ToTable("article");

            builder.HasKey(x => x.ArticleId)
                   .HasName("article_pkey");

            // Columns
            builder.Property(x => x.ArticleId)
                   .HasColumnName("article_id")
                   .IsRequired()
                   .ValueGeneratedNever();

            builder.Property(x => x.Title)
                   .HasColumnName("title")
                   .IsRequired();

            builder.Property(x => x.Text)
                   .HasColumnName("text")
                   .IsRequired();

            builder.Property(x => x.BlogId)
                   .HasColumnName("blog_id")
                   .IsRequired();

            builder.Property(x => x.AuthorId)
                   .HasColumnName("author_id")
                   .IsRequired();

            // Relationships
            builder.HasOne(x => x.Blog)
                   .WithMany(x => x.Articles)
                   .HasForeignKey(x => x.BlogId)
                   .HasConstraintName("article_blog_id_fkey");

            builder.HasOne(x => x.Author)
                   .WithMany(x => x.Articles)
                   .HasForeignKey(x => x.AuthorId)
                   .HasConstraintName("article_author_id_fkey");

            // Indexes
            builder.HasIndex(x => x.BlogId)
                   .HasName("article_ix_blog_id");

            builder.HasIndex(x => x.AuthorId)
                   .HasName("article_ix_author_id");
        }

        private static void ConfigureBlog(EntityTypeBuilder<Blog> builder)
        {
            // Table
            builder.ToTable("blog");

            builder.HasKey(x => x.BlogId)
                   .HasName("blog_pkey");

            // Columns
            builder.Property(x => x.BlogId)
                   .HasColumnName("blog_id")
                   .IsRequired()
                   .ValueGeneratedNever();

            builder.Property(x => x.Name)
                   .HasColumnName("name")
                   .IsRequired();

            // Relationships

            // Indexes
        }

        private static void ConfigureComment(EntityTypeBuilder<Comment> builder)
        {
            // Table
            builder.ToTable("comment");

            builder.HasKey(x => x.CommentId)
                   .HasName("comment_pkey");

            // Columns
            builder.Property(x => x.CommentId)
                   .HasColumnName("comment_id")
                   .IsRequired()
                   .ValueGeneratedNever();

            builder.Property(x => x.Body)
                   .HasColumnName("body")
                   .IsRequired();

            builder.Property(x => x.ArticleId)
                   .HasColumnName("article_id")
                   .IsRequired();

            builder.Property(x => x.AuthorId)
                   .HasColumnName("author_id");

            // Relationships
            builder.HasOne(x => x.Article)
                   .WithMany(x => x.Comments)
                   .HasForeignKey(x => x.ArticleId)
                   .HasConstraintName("comment_article_id_fkey");

            builder.HasOne(x => x.Author)
                   .WithMany(x => x.Comments)
                   .HasForeignKey(x => x.AuthorId)
                   .HasConstraintName("comment_author_id_fkey");

            // Indexes
            builder.HasIndex(x => x.ArticleId)
                   .HasName("comment_ix_article_id");

            builder.HasIndex(x => x.AuthorId)
                   .HasName("comment_ix_author_id");
        }

        private static void ConfigurePerson(EntityTypeBuilder<Person> builder)
        {
            // Table
            builder.ToTable("person");

            builder.HasKey(x => x.PersonId)
                   .HasName("person_pkey");

            // Columns
            builder.Property(x => x.PersonId)
                   .HasColumnName("person_id")
                   .IsRequired()
                   .ValueGeneratedNever();

            builder.Property(x => x.FirstName)
                   .HasColumnName("first_name")
                   .IsRequired();

            builder.Property(x => x.LastName)
                   .HasColumnName("last_name")
                   .IsRequired();

            builder.Property(x => x.Twitter)
                   .HasColumnName("twitter");

            // Relationships

            // Indexes
        }
        #endregion
    }
}