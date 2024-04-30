using FluentValidation;
using green.flux.Domain;
namespace green.flux.Validation
{

	public class ConnectorValidator : AbstractValidator<Connector>
	{
		public ConnectorValidator()
		{

			RuleFor(connector => connector.MaxCurrent)
				.GreaterThan(0).WithMessage("Max current must be greater than zero.");
		}
	}
}
