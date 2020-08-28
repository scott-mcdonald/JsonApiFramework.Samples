using System.Collections.Generic;
using System.Linq;

using Blogging.ServiceModel;
using Blogging.WebService.ErrorHandling;

using Exceptionless;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using RandomNameGeneratorLibrary;

namespace Blogging.WebService
{
    public class BloggingRepository
    {
        #region Public Constructor
        public BloggingRepository()
        {
            var options = CreateContextOptions();
            this.DbContextOptions = options;

            this.Seed();
        }
        #endregion

        #region Create, Update, and Delete Methods
        public Blog CreateBlog(Blog blog)
        {
            using var dataContext = this.CreateDataContext();

            blog.BlogId = this.NextBlogId++;
            dataContext.BlogTable.Add(blog);
            dataContext.SaveChanges();

            return blog;
        }

        public void DeleteBlog(long blogId)
        {
            using var dataContext = this.CreateDataContext();

            var blog = dataContext.BlogTable
                                  .SingleOrDefault(x => x.BlogId == blogId);
            if (blog == null)
            {
                throw new ApiNotFoundException("blogs", $"{blogId}");
            }

            dataContext.BlogTable.Remove(blog);
            dataContext.SaveChanges();
        }

        public Article CreateArticle(Article article)
        {
            using var dataContext = this.CreateDataContext();

            article.ArticleId = this.NextArticleId++;
            dataContext.ArticleTable.Add(article);
            dataContext.SaveChanges();

            return article;
        }

        public Article UpdateArticle(Article article)
        {
            using var dataContext = this.CreateDataContext();

            dataContext.ArticleTable.Update(article);
            dataContext.SaveChanges();

            return article;
        }

        public void DeleteArticle(long articleId)
        {
            using var dataContext = this.CreateDataContext();

            var article = dataContext.ArticleTable
                                     .SingleOrDefault(x => x.ArticleId == articleId);
            if (article == null)
            {
                throw new ApiNotFoundException("articles", $"{articleId}");
            }

            dataContext.ArticleTable.Remove(article);
            dataContext.SaveChanges();
        }

        public Comment CreateComment(Comment comment)
        {
            using var dataContext = this.CreateDataContext();

            comment.CommentId = this.NextCommentId++;
            dataContext.CommentTable.Add(comment);
            dataContext.SaveChanges();

            return comment;
        }

        public void DeleteComment(long commentId)
        {
            using var dataContext = this.CreateDataContext();

            var comment = dataContext.CommentTable
                                     .SingleOrDefault(x => x.CommentId == commentId);
            if (comment == null)
            {
                throw new ApiNotFoundException("comments", $"{commentId}");
            }

            dataContext.CommentTable.Remove(comment);
            dataContext.SaveChanges();
        }

        public Person CreatePerson(Person person)
        {
            using var dataContext = this.CreateDataContext();

            person.PersonId = this.NextPersonId++;
            dataContext.PersonTable.Add(person);
            dataContext.SaveChanges();

            return person;
        }

        public void DeletePerson(long personId)
        {
            using var dataContext = this.CreateDataContext();

            var person = dataContext.PersonTable
                                    .SingleOrDefault(x => x.PersonId == personId);
            if (person == null)
            {
                throw new ApiNotFoundException("people", $"{personId}");
            }

            dataContext.PersonTable.Remove(person);
            dataContext.SaveChanges();
        }
        #endregion

        #region Retrieve Methods
        // Collections
        public IEnumerable<Article> GetArticles()
        {
            using var dataContext = this.CreateDataContext();

            var articles = dataContext.ArticleTable
                                      .OrderBy(x => x.ArticleId)
                                      .ToList();
            return articles;
        }

        public IEnumerable<Blog> GetBlogs()
        {
            using var dataContext = this.CreateDataContext();

            var blogs = dataContext.BlogTable
                                   .OrderBy(x => x.BlogId)
                                   .ToList();
            return blogs;
        }

        public IEnumerable<Comment> GetComments()
        {
            using var dataContext = this.CreateDataContext();

            var comments = dataContext.CommentTable
                                      .OrderBy(x => x.CommentId)
                                      .ToList();
            return comments;
        }

        public IEnumerable<Person> GetPeople()
        {
            using var dataContext = this.CreateDataContext();

            var people = dataContext.PersonTable
                                    .OrderBy(x => x.PersonId)
                                    .ToList();
            return people;
        }

        public Article GetArticle(long articleId)
        {
            using var dataContext = this.CreateDataContext();

            var article = dataContext.ArticleTable
                                     .SingleOrDefault(x => x.ArticleId == articleId);
            if (article != null)
            {
                return article;
            }

            throw new ApiNotFoundException("articles", $"{articleId}");
        }

        public Blog GetBlog(long blogId)
        {
            using var dataContext = this.CreateDataContext();

            var blog = dataContext.BlogTable
                                  .SingleOrDefault(x => x.BlogId == blogId);
            if (blog != null)
            {
                return blog;
            }

            throw new ApiNotFoundException("blogs", $"{blogId}");
        }

        public Comment GetComment(long commentId)
        {
            using var dataContext = this.CreateDataContext();

            var comment = dataContext.CommentTable
                                     .SingleOrDefault(x => x.CommentId == commentId);
            if (comment != null)
            {
                return comment;
            }

            throw new ApiNotFoundException("comments", $"{commentId}");
        }

        public Person GetPerson(long personId)
        {
            using var dataContext = this.CreateDataContext();

            var person = dataContext.PersonTable
                                    .SingleOrDefault(x => x.PersonId == personId);
            if (person != null)
            {
                return person;
            }

            throw new ApiNotFoundException("people", $"{personId}");
        }

        public Blog GetArticleToBlog(long articleId)
        {
            using var dataContext = this.CreateDataContext();

            var article = dataContext.ArticleTable
                                     .Include(x => x.Blog)
                                     .SingleOrDefault(x => x.ArticleId == articleId);
            if (article == null)
            {
                throw new ApiNotFoundException("articles", $"{articleId}");
            }

            var blog = article.Blog;
            return blog;
        }

        public IEnumerable<Comment> GetArticleToComments(long articleId)
        {
            using var dataContext = this.CreateDataContext();

            var article = dataContext.ArticleTable
                                     .Include(x => x.Comments)
                                     .SingleOrDefault(x => x.ArticleId == articleId);
            if (article == null)
            {
                throw new ApiNotFoundException("articles", $"{articleId}");
            }

            var comments = article.Comments;
            return comments;
        }

        public Person GetArticleToAuthor(long articleId)
        {
            using var dataContext = this.CreateDataContext();

            var article = dataContext.ArticleTable
                                     .Include(x => x.Author)
                                     .SingleOrDefault(x => x.ArticleId == articleId);
            if (article == null)
            {
                throw new ApiNotFoundException("articles", $"{articleId}");
            }

            var author = article.Author;
            return author;
        }

        public IEnumerable<Article> GetBlogToArticles(long blogId)
        {
            using var dataContext = this.CreateDataContext();

            var blog = dataContext.BlogTable
                                  .Include(x => x.Articles)
                                  .SingleOrDefault(x => x.BlogId == blogId);
            if (blog == null)
            {
                throw new ApiNotFoundException("blogs", $"{blogId}");
            }

            var articles = blog.Articles;
            return articles;
        }

        public Article GetCommentToArticle(long commentId)
        {
            using var dataContext = this.CreateDataContext();

            var comment = dataContext.CommentTable
                                     .Include(x => x.Article)
                                     .SingleOrDefault(x => x.CommentId == commentId);
            if (comment == null)
            {
                throw new ApiNotFoundException("blogs", $"{commentId}");
            }

            var article = comment.Article;
            return article;
        }

        public Person GetCommentToAuthor(long commentId)
        {
            using var dataContext = this.CreateDataContext();

            var comment = dataContext.CommentTable
                                     .Include(x => x.Author)
                                     .SingleOrDefault(x => x.CommentId == commentId);
            if (comment == null)
            {
                throw new ApiNotFoundException("blogs", $"{commentId}");
            }

            var author = comment.Author;
            return author;
        }

        public IEnumerable<Article> GetPersonToArticles(long personId)
        {
            using var dataContext = this.CreateDataContext();

            var person = dataContext.PersonTable
                                    .Include(x => x.Articles)
                                    .SingleOrDefault(x => x.PersonId == personId);
            if (person == null)
            {
                throw new ApiNotFoundException("blogs", $"{personId}");
            }

            var articles = person.Articles;
            return articles;
        }

        public IEnumerable<Comment> GetPersonToComments(long personId)
        {
            using var dataContext = this.CreateDataContext();

            var person = dataContext.PersonTable
                                    .Include(x => x.Comments)
                                    .SingleOrDefault(x => x.PersonId == personId);
            if (person == null)
            {
                throw new ApiNotFoundException("blogs", $"{personId}");
            }

            var comments = person.Comments;
            return comments;
        }
        #endregion

        #region Private Methods
        private void Seed()
        {
            using var dataContext = this.CreateDataContext();

            dataContext.Database.EnsureDeleted();
            dataContext.Database.EnsureCreated();

            // Create random blogs.
            var numberOfBlogs = RandomData.GetInt(4, 8);
            this.NextBlogId = numberOfBlogs + 1;
            for (var i = 1; i <= numberOfBlogs; i++)
            {
                var blog = new Blog
                {
                    BlogId = i,
                    Name   = RandomData.GetTitleWords()
                };

                dataContext.BlogTable.Add(blog);
            }

            dataContext.SaveChanges();

            // Create random people.
            var personNameGenerator = new PersonNameGenerator();
            var numberOfPersons     = RandomData.GetInt(16, 32);
            this.NextPersonId = numberOfPersons + 1;
            for (var i = 1; i <= numberOfPersons; i++)
            {
                var firstName = personNameGenerator.GenerateRandomFirstName();
                var lastName  = personNameGenerator.GenerateRandomLastName();
                var twitter   = $"@{firstName.First()}{lastName}".ToLowerInvariant();
                var person = new Person
                {
                    PersonId  = i,
                    FirstName = firstName,
                    LastName  = lastName,
                    Twitter   = twitter
                };

                dataContext.PersonTable.Add(person);
            }

            dataContext.SaveChanges();

            // Create random articles.
            var numberOfArticles = RandomData.GetInt(8, 16);
            this.NextArticleId = numberOfArticles + 1;
            for (var i = 1; i <= numberOfArticles; i++)
            {
                var blogId   = RandomData.GetLong(1, numberOfBlogs);
                var authorId = RandomData.GetLong(1, numberOfPersons);
                var article = new Article
                {
                    BlogId    = blogId,
                    AuthorId  = authorId,
                    ArticleId = i,
                    Title     = RandomData.GetTitleWords(),
                    Text      = RandomData.GetParagraphs()
                };

                dataContext.ArticleTable.Add(article);
            }

            dataContext.SaveChanges();

            // Create random comments.
            var numberOfComments = RandomData.GetInt(16, 32);
            this.NextCommentId = numberOfComments + 1;
            for (var i = 1; i <= numberOfComments; i++)
            {
                var articleId = RandomData.GetLong(1, numberOfArticles);
                var authorId  = RandomData.GetLong(0, 2) != 0 ? RandomData.GetLong(1, numberOfPersons) : new long?();
                var comment = new Comment
                {
                    ArticleId = articleId,
                    AuthorId  = authorId,
                    CommentId = i,
                    Body      = RandomData.GetSentence()
                };

                dataContext.CommentTable.Add(comment);
            }

            dataContext.SaveChanges();
        }
        #endregion

        #region Private Properties
        private DbContextOptions DbContextOptions { get; }

        private long NextArticleId { get; set; }
        private long NextBlogId    { get; set; }
        private long NextCommentId { get; set; }
        private long NextPersonId  { get; set; }
        #endregion

        #region Factory Methods
        private static DbContextOptions CreateContextOptions()
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<BloggingDataContext>();

            // Detailed Errors Enabled
            dbContextOptionsBuilder.EnableDetailedErrors();

            // Sensitive Data Logging Enabled
            dbContextOptionsBuilder.EnableSensitiveDataLogging();

            // Sqlite EntityFrameworkCore Configuration
            var connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                DataSource = ":memory:"
            };
            var connectionString = connectionStringBuilder.ConnectionString;

            var connection = new SqliteConnection(connectionString);
            connection.Open();

            dbContextOptionsBuilder.UseSqlite(connection);

            var dbContextOptions = dbContextOptionsBuilder.Options;
            return dbContextOptions;
        }

        private BloggingDataContext CreateDataContext()
        {
            return new BloggingDataContext(this.DbContextOptions);
        }
        #endregion
    }
}