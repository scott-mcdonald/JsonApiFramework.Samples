using System;
using System.Collections.Generic;
using System.Linq;

using Blogging.ServiceModel;

using Exceptionless;

using RandomNameGeneratorLibrary;

namespace Blogging.WebService
{
    public static class BloggingRepository
    {
        #region Public Methods

        // Collections
        public static IEnumerable<Article> GetArticles()
        { return Articles; }

        public static IEnumerable<Blog> GetBlogs()
        { return Blogs; }

        public static IEnumerable<Comment> GetComments()
        { return Comments; }

        public static IEnumerable<Person> GetPeople()
        { return People; }

        // Object in Collection by Primary Key
        public static Article GetArticle(long articleId)
        {
            var article = Articles.Single(x => x.ArticleId == articleId);
            return article;
        }

        public static Blog GetBlog(long blogId)
        {
            var blog = Blogs.Single(x => x.BlogId == blogId);
            return blog;
        }

        public static Comment GetComment(long commentId)
        {
            var comment = Comments.Single(x => x.CommentId == commentId);
            return comment;
        }

        public static Person GetPerson(long personId)
        {
            var person = People.Single(x => x.PersonId == personId);
            return person;
        }

        // Object to Related Objects by Primary Key
        public static Blog GetArticleToBlog(long articleId)
        {
            var articleToBlogId = Articles.Single(x => x.ArticleId == articleId)
                                          .BlogId;
            var articleToBlog = GetBlog(articleToBlogId);
            return articleToBlog;
        }

        public static IEnumerable<Comment> GetArticleToComments(long articleId)
        {
            var articleToComments = Comments.Where(x => x.ArticleId == articleId)
                                            .ToList();
            return articleToComments;
        }

        public static Person GetArticleToAuthor(long articleId)
        {
            var articleToAuthorId = Articles.Single(x => x.ArticleId == articleId)
                                          .AuthorId;
            var articleToAuthor = GetPerson(articleToAuthorId);
            return articleToAuthor;
        }

        public static IEnumerable<Article> GetBlogToArticles(long blogId)
        {
            var blogToArticles = Articles.Where(x => x.BlogId == blogId)
                                         .ToList();
            return blogToArticles;
        }

        public static Article GetCommentToArticle(long commentId)
        {
            var commentToArticleId = Comments.Single(x => x.CommentId == commentId)
                                             .ArticleId;
            var commentToArticle = GetArticle(commentToArticleId);
            return commentToArticle;
        }

        public static Person GetCommentToAuthor(long commentId)
        {
            var commentToAuthorId = Comments.Single(x => x.CommentId == commentId)
                                            .AuthorId;
            var commentToAuthor = GetPerson(commentToAuthorId);
            return commentToAuthor;
        }

        public static IEnumerable<Article> GetPersonToArticles(long authorId)
        {
            var personToArticles = Articles.Where(x => x.AuthorId == authorId)
                                           .ToList();
            return personToArticles;
        }

        public static IEnumerable<Comment> GetPersonToComments(long authorId)
        {
            var personToComments = Comments.Where(x => x.AuthorId == authorId)
                                           .ToList();
            return personToComments;
        }
        #endregion

        #region Private Properties
        private static List<Blog> Blogs { get; set; }
        private static List<Article> Articles { get; set; }
        private static List<Comment> Comments { get; set; }
        private static List<Person> People { get; set; }
        #endregion

        #region Private Methods
        private static void Seed()
        {
            // Create random blogs.
            var numberOfBlogs = RandomData.GetInt(4, 8);
            for (var i = 1; i <= numberOfBlogs; i++)
            {
                var blog = new Blog
                    {
                        BlogId = i,
                        Name = RandomData.GetTitleWords()
                    };
                Blogs.Add(blog);
            }

            // Create random people.
            var personNameGenerator = new PersonNameGenerator();
            var numberOfPersons = RandomData.GetInt(16, 32);
            for (var i = 1; i <= numberOfPersons; i++)
            {
                var firstName = personNameGenerator.GenerateRandomFirstName();
                var lastName = personNameGenerator.GenerateRandomLastName();
                var twitter = String.Format("@{0}{1}", firstName.First(), lastName).ToLowerInvariant();
                var person = new Person
                    {
                        PersonId = i,
                        FirstName = firstName,
                        LastName = lastName,
                        Twitter = twitter
                    };
                People.Add(person);
            }

            // Create random articles.
            var numberOfArticles = RandomData.GetInt(8, 16);
            for (var i = 1; i <= numberOfArticles; i++)
            {
                var blogId = RandomData.GetLong(1, numberOfBlogs);
                //var authorId = RandomData.GetLong(1, numberOfPersons);
                var authorId = 1;
                var article = new Article
                    {
                        BlogId = blogId,
                        AuthorId = authorId,
                        ArticleId = i,
                        Title = RandomData.GetTitleWords(),
                        Text = RandomData.GetParagraphs()
                    };
                Articles.Add(article);
            }

            // Create random comments.
            var numberOfComments = RandomData.GetInt(16, 32);
            for (var i = 1; i <= numberOfComments; i++)
            {
                var articleId = RandomData.GetLong(1, numberOfArticles);
                var authorId = RandomData.GetLong(1, numberOfPersons);
                var comment = new Comment
                    {
                        ArticleId = articleId,
                        AuthorId = authorId,
                        CommentId = i,
                        Body = RandomData.GetSentence()
                    };
                Comments.Add(comment);
            }
        }
        #endregion

        #region Static Constructor
        static BloggingRepository()
        {
            Blogs = new List<Blog>();
            Articles = new List<Article>();
            Comments = new List<Comment>();
            People = new List<Person>();

            Seed();
        }
        #endregion
    }
}
