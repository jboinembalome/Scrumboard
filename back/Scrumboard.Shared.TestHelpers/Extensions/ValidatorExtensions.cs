using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Scrumboard.Shared.TestHelpers.Extensions;

public static class ValidatorExtensions
{
    public static void ShouldCallValidateAndThrowAsync<T>(
        this IValidator<T> validator,
        Times times) where T : class
    {
        Mock.Get(validator)
            .Verify(x => x.ValidateAsync(
                    It.IsAny<ValidationContext<T>>(), 
                    It.IsAny<CancellationToken>()), 
                times);
    }
    
    public static void SetupValidationFailed<T>(
        this IValidator<T> validator, 
        string propertyName, 
        string errorMessage) where T : class
    {
        var mockValidator = Mock.Get(validator);

        var error = new ValidationFailure(propertyName, errorMessage);
        var failureResult = new ValidationResult([error]);
        
        SetupInstanceValidationFailed(mockValidator, failureResult);
        SetupContextValidationFailed(mockValidator, failureResult);
    }
    
    private static void SetupInstanceValidationFailed<T>(
        Mock<IValidator<T>> mockValidator, 
        ValidationResult failureResult) where T : class
    {
        mockValidator.Setup(p => p.Validate(It.IsAny<T>()))
            .Returns(failureResult)
            .Verifiable();

        mockValidator.Setup(p => p.ValidateAsync(It.IsAny<T>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(failureResult)
            .Verifiable();
    }
    
    private static void SetupContextValidationFailed<T>(
        Mock<IValidator<T>> mockValidator, 
        ValidationResult failureResult) where T : class
    {
        mockValidator.Setup(p => p.Validate(
                It.Is<ValidationContext<T>>(context => context.ThrowOnFailures)))
            .Throws(new ValidationException(failureResult.Errors))
            .Verifiable();

        mockValidator.Setup(p => p.ValidateAsync(
                It.Is<ValidationContext<T>>(context => context.ThrowOnFailures), 
                It.IsAny<CancellationToken>()))
            .Throws(new ValidationException(failureResult.Errors))
            .Verifiable();

        mockValidator.Setup(p => p.Validate(
                It.Is<ValidationContext<T>>(context => !context.ThrowOnFailures)))
            .Returns(failureResult)
            .Verifiable();

        mockValidator.Setup(p => p.ValidateAsync(
                It.Is<ValidationContext<T>>(context => !context.ThrowOnFailures), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(failureResult)
            .Verifiable();
    }
}
