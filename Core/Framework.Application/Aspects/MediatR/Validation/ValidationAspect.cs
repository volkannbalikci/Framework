using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Application.Aspects.MediatR.Validation
{
    public class ValidationAspect<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
         where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationAspect(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            ValidationContext<object> validationContext = new(request);

            List<ValidationResult> validationResults = _validators.Select(validator => validator.Validate(validationContext)).ToList();

            List<ValidationFailure> validationFailures = validationResults.SelectMany(validationResult => validationResult.Errors)
                                                                          .Where(failure => failure != null)
                                                                          .ToList();

            this.CheckIfFailuresExists(validationFailures);

            return next();
        }

        private void CheckIfFailuresExists(List<ValidationFailure> validationFailures)
        {
            if (validationFailures.Count != 0)
                throw new ValidationException(validationFailures);
        }
    }
}
