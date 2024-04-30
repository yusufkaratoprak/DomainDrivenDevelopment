using FluentValidation;
using green.flux.Domain;

namespace green.flux.Validation
{


	public class GroupValidator : AbstractValidator<Group>
	{
		public GroupValidator()
		{
			
			RuleFor(group => group.Name)
				.NotEmpty().WithMessage("Name is required.");

			RuleFor(group => group.Capacity)
				.GreaterThan(0).WithMessage("Capacity must be greater than zero.");
		}
	}
}

