using RestfulApiTests.Fixtures;
using Xunit;

namespace RestfulApiTests.TestCollections;

[CollectionDefinition("Api Test Collection")]
public class TestCollection : ICollectionFixture<ReportFixture>;
