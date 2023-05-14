using JWT.Algorithms;
using JWT.Builder;

namespace BotyObchodASP.Models
{
    public class LoginService
    {
    public readonly string _SECRET = "TajneTajneJoJo";
        private MyContext myContext = new MyContext();
        public bool Authenticate(TbAdmin credentials)
        {
            TbAdmin admin = myContext.TbAdmins.FirstOrDefault(x => x.Username == credentials.Username);

            if (admin == null)
                return false;

            return BCrypt.Net.BCrypt.Verify(credentials.Password, admin.Password);
        }
        public bool VerifyToken(string tok)
        {
            try
            {
                _ = JwtBuilder.Create().WithAlgorithm(new HMACSHA256Algorithm()).WithSecret(_SECRET).MustVerifySignature().Decode(tok);
                return true;
            }
            catch
            {

                return false;
            }
        }
    }
}
