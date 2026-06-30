using Microsoft.AspNetCore.Mvc.Testing;

namespace FeatureLabs.DatabaseTesting.FunctionalTests;

[CollectionDefinition("Tests")]
public class SharedTestCollection : ICollectionFixture<ContainerizedApplicationFactory>
{
    
}
