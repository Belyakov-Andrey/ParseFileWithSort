using ParseFileWithSort;

namespace ParseFileWithSortTests;

public class UserParserTests
{
    [Theory]
    [InlineData("1,John,Doe,john@test.com,Male,192.168.1.1")]
    [InlineData("2;Jane;Doe;jane@test.com;Female;192.168.1.2")]
    [InlineData("3, Bob , Smith , bob@test.com , Male , 192.168.1.3 ")]
    public void ParseUserLine_ValidInput_ReturnsUser(string input)
    {
        var user = User.ParseUserLine(input);
        
        Assert.NotNull(user);
        Assert.False(string.IsNullOrWhiteSpace(user.Id));
        Assert.False(string.IsNullOrWhiteSpace(user.Email));
    }

    [Theory]
    [InlineData("")]
    [InlineData("1,John,Doe")]
    public void ParseUserLine_InvalidInput_ThrowsException(string input)
    {
        Assert.Throws<ArgumentException>(() => User.ParseUserLine(input));
    }

    [Theory]
    [InlineData(" test ", "test")]
    [InlineData("test\t", "test")]
    [InlineData("test\r\n", "test")]
    public void CleanField_RemovesWhitespaceAndControlChars(string input, string expected)
    {
        var result = User.CleanField(input);
        
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ParseUserLine_WithEmptyEmail_ThrowsException()
    {
        var input = "1,John,Doe,,Male,192.168.1.1";
        
        var ex = Assert.Throws<ArgumentException>(() => User.ParseUserLine(input));
        Assert.Contains("Email не может быть пустым", ex.Message);
    }
}