using NSubstitute;
using NUnit.Framework;

namespace RsaSecureToken.Tests
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        private IProfile _profile;
        private IToken _token;
        private AuthenticationService _authenticationService;

        [SetUp]
        public void SetUp()
        {
            _profile = Substitute.For<IProfile>();
            _token = Substitute.For<IToken>();
            _authenticationService = new AuthenticationService(_profile, _token);
        }

        [Test()]
        public void IsValid()
        {
            GivenPassword("joey", "91");
            GivenRandom("000000");

            var actual = WhenValidate("joey", "91000000");
            
            ShouleBeValid(actual);                       
        }

        [Test]
        public void IsInvalid()
        {
            GivenPassword("joey", "91");
            GivenRandom("000000");
            var actual = WhenValidate("joey", "wrong password");
            ShouldBeInvalid(actual);
        }

        private static void ShouldBeInvalid(bool actual)
        {
            Assert.IsFalse(actual);
        }

        private static void ShouleBeValid(bool actual)
        {
            Assert.IsTrue(actual);
        }

        private bool WhenValidate(string account, string password)
        {
            return _authenticationService.IsValid(account, password);
        }

        private void GivenRandom(string random)
        {
            _token.GetRandom("").ReturnsForAnyArgs(random);
        }

        private void GivenPassword(string account, string password)
        {
            _profile.GetPassword(account).Returns(password);
        }
    }
}
