using FluentValidation;
using green.flux.Domain;

namespace green.flux.Validation
{
	public class ChargeStationValidator : AbstractValidator<ChargeStation>
	{
		public ChargeStationValidator()
		{

			RuleFor(station => station.Name)
				.NotEmpty().WithMessage("Name is required.");

			RuleFor(station => station.Connectors)
				.Must(connectors =>  connectors.Count <= 5)
				.WithMessage("You can not add more than 5 connectors");
		}
	}

}
