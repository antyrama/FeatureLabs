using System.ComponentModel.DataAnnotations;
using FeatureLabs.AppServices.Configuration.Exceptions;

namespace FeatureLabs.AppServices.Configuration.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureOptionsWithValidation<TOptions>(this IServiceCollection services,
        IConfiguration configuration)
        where TOptions : class
    {
        string position = typeof(TOptions).TryGetPosition()!;

        services.AddOptions<TOptions>()
            .Bind(configuration.GetSection(position))
            //.ValidateDataAnnotations()
            .Validate(config =>
            {
                var validationContext = new ValidationContext(config);

                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(config, validationContext, validationResults, true);
                if (!isValid)
                {
                    var errors = validationResults.SelectMany(x => x.MemberNames, (x, y) => new { x.ErrorMessage, y });
                    var errorMessages = errors.Select(x => $"{x.ErrorMessage} ({x.y})").ToArray();
                    throw new CustomValidationException($"Application setting at '{position}' is invalid.", errorMessages);
                }
                return isValid;
            }, "Configuration is invalid")
            .ValidateOnStart();

        return services;
    }
}
