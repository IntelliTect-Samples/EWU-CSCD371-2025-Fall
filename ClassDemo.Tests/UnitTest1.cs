namespace ClassDemo.Tests
{
    public class UnitTest1
    {
        [Theory]
        [InlineData("admin", "goodpassword")]
        [InlineData("InigoMontoya", "goodpassword")]
        [InlineData("PrincessButtercup", "goodpassword")]
        public void TryLogin_WithGoodPassword_SuccessfulLogin(string username, String password)
        {
            Program program = new();
            Assert.True(program.TryLogin(username, password));

            Program.Multiply(2, 3);
        }

        [Fact]
        public void SwapTest()
        {

            // Arrange
            int left = 1;
            int right = 2;
            // Act
            try
            {
                Program.Swap(ref left, ref right);
            }
            catch (InvalidCastException)
            {
            }
            Assert.Equal(2, left);
            //Assert.Equal(1, right);
            // Assert
        }
        [Fact]
        public void SwapStringTest()
        {

            // Arrange
            string left = "1";
            string right = "2";

            // Act
            Program.Swap(left, right);
            Assert.Equal("2", left);
            Assert.Equal("1", right);
        }

        [Fact]
        public void ToUpperStringTest()
        {

            string first = "Inigo " + "Montoya";
            string second = "Inigo Montoya";
            second = second.ToUpper();
            Assert.Equal("INIGO MONTOYA", second);
        }

        [Fact]
        public void TupleSwapTest()
        {
            // Arrange
            string left = "1";
            string right = "2";

            // Act
            (string wasRight, string wasLeft) =
                Program.SwapTuple(left, right);
            Assert.Equal("2", wasRight);
            Assert.Equal("1", wasLeft);

            (right, left) = Program.SwapTuple(left, right);
            Assert.Equal(("1", "3"), (right, left));
        }

        [Fact]
        // MethodUnderTest_ConditionUnderTest_ExpectedResult
        public void TryLogin_ValidCredentials_Success()
        {
            Program program = new();
            Assert.True(program.TryLogin(username: "admin", password: "goodpassword"));
        }

        [Fact]
        public void TryLogin_InigoMontoyaValidCredentials_Success()
        {
            Program program = new();
            Assert.True(program.TryLogin(username: "InigoMontoya", password: "goodpassword"));
        }

        [Fact]
        public void TryLogin_InvalidCredentials_Failure()
        {
            Program program = new();
            Assert.False(program.TryLogin(username: "admin", password: "wrongpassword"));
        }

        [Fact]
        public void Login_PrincessButtercupValidCredentials_Success()
        {
            Program program = new();
            program.Login(username: "PrincessButtercup", password: "goodpassword");
        }

        [Fact]
        public void Login_InvalidCredentials_ThrowsInvalidOperationException()
        {
            Program program = new();
            Assert.Throws<InvalidOperationException>(
                () => program.Login(username: "admin", password: "wrongpassword")
                );
        }
    }
}
