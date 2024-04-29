using FluentValidation;
using green.flux.Domain;

namespace green.flux.Validation
{
	public class ChargeStationValidator : AbstractValidator<ChargeStation>
	{
		public ChargeStationValidator()
		{
			RuleFor(station => station.ID)
				.NotEmpty().WithMessage("ID is required.");

			RuleFor(station => station.Name)
				.NotEmpty().WithMessage("Name is required.");

			RuleFor(station => station.Connectors)
				.Must(connectors => connectors.Count >= 1 && connectors.Count <= 5)
				.WithMessage("A Charge Station must have between 1 and 5 connectors.");
		}
	}

}
