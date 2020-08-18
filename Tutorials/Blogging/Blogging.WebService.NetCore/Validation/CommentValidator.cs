using Blogging.ServiceModel;

using FluentValidation;

namespace Blogging.WebService.Validation
{
    public class CommentValidator : AbstractValidator<Comment>
    {
        public CommentValidator()
        {
            this.RuleFor(x => x.Body).NotEmpty();
            this.RuleFor(x => x.ArticleId).NotEmpty();
        }
    }
}
