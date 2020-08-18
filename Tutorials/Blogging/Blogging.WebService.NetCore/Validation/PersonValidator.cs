using Blogging.ServiceModel;

using FluentValidation;

namespace Blogging.WebService.Validation
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            this.RuleFor(x => x.FirstName).NotEmpty();
            this.RuleFor(x => x.LastName).NotEmpty();
        }
    }
}
