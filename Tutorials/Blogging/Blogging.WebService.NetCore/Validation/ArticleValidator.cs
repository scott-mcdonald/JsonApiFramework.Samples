using Blogging.ServiceModel;

using FluentValidation;

namespace Blogging.WebService.Validation
{
    public class ArticleValidator : AbstractValidator<Article>
    {
        public ArticleValidator()
        {
            this.RuleFor(x => x.Title).NotEmpty();
            this.RuleFor(x => x.Text).NotEmpty();
            this.RuleFor(x => x.BlogId).NotEmpty();
            this.RuleFor(x => x.AuthorId).NotEmpty();
        }
    }
}
