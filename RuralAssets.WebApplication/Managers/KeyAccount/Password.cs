using Org.BouncyCastle.OpenSsl;

namespace RuralAssets.WebApplication
{
    public class Password : IPasswordFinder
    {
        private readonly char[] _password;

        public Password(char[] word)
        {
            _password = (char[]) word.Clone();
        }

        public char[] GetPassword()
        {
            return (char[]) _password.Clone();
        }
    }
}