namespace FeatureLabs.AppServices.Configuration.FunctionalTests.Common
{
    internal class MappedTestDataBuilderBase<TOptions>
        where TOptions : class, new()
    {
        private readonly Func<TOptions> _factory;

        protected MappedTestDataBuilderBase(Func<TOptions> factory)
        {
            _factory = factory;
        }

        protected IEnumerable<object[]> Build(params Func<TOptions, List<string>, int>[] configure)
        {
            foreach (var func in configure)
            {
                var options = _factory();
                var errors = new List<string>();
                
                var count = func(options, errors);

                yield return new object[] { count, options, errors };
            }
        }
    }
}
