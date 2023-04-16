using FeatureLabs.AppServices.Configuration.FunctionalTests.Common;
using FeatureLabs.AppServices.Configuration.Options;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace FeatureLabs.AppServices.Configuration.FunctionalTests.Options;

public class AzureB2COptionsTests : OverrideOptionsTestsBase<Program, AzureB2COptions>
{
    public AzureB2COptionsTests(WebApplicationFactory<Program> factory, ITestOutputHelper outputHelper)
        : base(factory, outputHelper)
    {
    }

    [Theory]
    [MemberData(nameof(AzureB2COptionsIncorrectDataBuilder.Build),
        MemberType = typeof(AzureB2COptionsIncorrectDataBuilder))]
    public void ShouldThrowExceptionWhenAppSettingIncorrect(int id, AzureB2COptions options, string[] expectedErrorMessage)
    {
        Test(id, options, expectedErrorMessage);
    }
}
