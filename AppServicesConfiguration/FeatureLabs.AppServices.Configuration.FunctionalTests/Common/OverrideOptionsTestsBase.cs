using AutoMapper;
using FeatureLabs.AppServices.Configuration.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace FeatureLabs.AppServices.Configuration.FunctionalTests.Common;

public class OverrideOptionsTestsBase<TStartup, TOptions> : IClassFixture<WebApplicationFactory<TStartup>>
    where TStartup : class
    where TOptions : class, new()
{
    private readonly WebApplicationFactory<TStartup> _factory;
    private readonly ITestOutputHelper _outputHelper;
    private readonly IMapper _mapper;

    public OverrideOptionsTestsBase(WebApplicationFactory<TStartup> factory,
        ITestOutputHelper outputHelper)
    {
        _factory = factory;
        _outputHelper = outputHelper;
        _mapper = new MapperConfiguration(cfg => cfg.CreateMap<TOptions, TOptions>())
            .CreateMapper();
    }

    protected void Test(int id, TOptions options, string[] expectedErrorMessages)
    {
        _outputHelper.WriteLine($"Parameter set with id = {0}", id);

        var ex = Assert.Throws<CustomValidationException>(() =>
        {
            var factory = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddOptions<TOptions>().Configure(opt =>
                    {
                        _mapper.Map(options, opt);
                    });
                });
            });

            factory.CreateClient();
        });

        ex.ErrorMessages.Should().BeEquivalentTo(expectedErrorMessages);
    }
}
