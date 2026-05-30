using System.Collections.Generic;

namespace RestfulApiTests.Reporting
{
    public static class TestResultStore
    {
        public static List<TestResult> Results { get; } = new();
    }
}