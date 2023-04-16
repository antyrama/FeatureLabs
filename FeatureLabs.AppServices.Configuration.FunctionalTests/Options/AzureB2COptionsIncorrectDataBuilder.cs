using FeatureLabs.AppServices.Configuration.FunctionalTests.Common;
using FeatureLabs.AppServices.Configuration.Options;

namespace FeatureLabs.AppServices.Configuration.FunctionalTests.Options
{
    internal class AzureB2COptionsIncorrectDataBuilder : MappedTestDataBuilderBase<AzureB2COptions>
    {
        public AzureB2COptionsIncorrectDataBuilder() 
            : base(() => new AzureB2COptions
        {
            ClientId = "ClientId",
            ClientSecret = "564wev7i5vb68or",
            Domain = "acme.onmicrosoft.com",
            EmailAddress = "valid@email.address",
            Instance = "acme.b2clogin.com"
        })
        {
        }

        public static IEnumerable<object[]> Build()
        {
            var builder = new AzureB2COptionsIncorrectDataBuilder();

            return builder.Build(
                (options, errors) =>
                {
                    options.EmailAddress = "blah blah";

                    errors.Add("The EmailAddress field is not a valid e-mail address. (EmailAddress)");

                    return 1;
                },
                (options, errors) =>
                {
                    options.EmailAddress = "  ";

                    errors.Add("The EmailAddress field is required. (EmailAddress)");

                    return 2;
                },
                (options, errors) =>
                {
                    options.EmailAddress = null!;

                    errors.Add("The EmailAddress field is required. (EmailAddress)");

                    return 3;
                });
        }
    }
}
