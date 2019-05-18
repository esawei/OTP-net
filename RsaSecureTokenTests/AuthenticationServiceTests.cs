using NSubstitute;
using NUnit.Framework;

namespace RsaSecureToken.Tests
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        [SetUp]
        public void SetUp()
        {
            _profile = Substitute.For<IProfile>();
            _token = Substitute.For<IToken>();
            _logger = Substitute.For<ILogger>();
            _authenticationService = new AuthenticationService(_profile, _token, _logger);
        }

        private IProfile _profile;
        private IToken _token;
        private AuthenticationService _authenticationService;
        private ILogger _logger;

        private bool WhenInvalid()
        {
            GivenPassword("joey", "91");
            GivenRandom("000000");
            var actual = WhenValidate("joey", "wrong password");
            return actual;
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

        private bool WhenValid()
        {
            GivenPassword("joey", "91");
            GivenRandom("000000");

            var actual = WhenValidate("joey", "91000000");
            return actual;
        }

        private void ShouldLog(string message)
        {
            _logger.Received(1).Log(Arg.Is<string>(x => x.Contains(message)));
        }

        private void ShouldNotLog()
        {
            _logger.DidNotReceiveWithAnyArgs().Log("");
        }

        [Test]
        public void IsValid()
        {
            var actual = WhenValid();
            ShouleBeValid(actual);
        }

        [Test]
        public void IsInvalid()
        {
            var actual = WhenInvalid();
            ShouldBeInvalid(actual);
        }

        [Test]
        public void Should_log_account_when_invalid()
        {
            WhenInvalid();
            ShouldLog("joey");
        }

        [Test]
        public void Should_not_log_account_when_valid()
        {
            WhenValid();
            ShouldNotLog();
        }
    }
}