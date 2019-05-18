using NUnit.Framework;

namespace RsaSecureToken.Tests
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        [Test()]
        public void IsValid()
        {
            var target = new AuthenticationService(new StubProfileDao(), new StubRsaTokenDao());

            var actual = target.IsValid("joey", "91000000");

            //always failed
            Assert.IsTrue(actual);                       
        }
    }

    internal class StubProfileDao : IProfile
    {
        public string GetPassword(string account)
        {
            if (account=="joey")
            {
                return "91";
            }

            return "";
        }
    }

    internal class StubRsaTokenDao : IToken
    {
        public string GetRandom(string account)
        {
            return "000000";
        }
    }
}
