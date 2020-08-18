using Blogging.ServiceModel;

using FluentValidation;

namespace Blogging.WebService.Validation
{
    public class BlogValidator : AbstractValidator<Blog>
    {
        public BlogValidator()
        {
            this.RuleFor(x => x.Name).NotEmpty();
        }
    }
}
