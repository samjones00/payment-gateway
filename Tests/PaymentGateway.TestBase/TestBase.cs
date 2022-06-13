using Moq;

namespace PaymentGateway.Tests.Shared;

public class TestBase
{
    public readonly Fixture _fixture;
    public readonly MockRepository _mockRepository;

    public TestBase()
    {
        _fixture = new Fixture();
        _mockRepository = new MockRepository(MockBehavior.Strict);
    }

    public static IEnumerable<string> NullOrWhiteSpaceStrings() => new List<string> { " ", "", null };

    public static string CreateStringOfLength(int length) => new('0', length);

}