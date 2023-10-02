
using Moneyboard.Core.Entities.UserEntity;
using Moneyboard.Core.Interfaces;
using Nest;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moneyboard.Core.Entities.RefreshTokenEntity
{
    public class RefreshToken : IBaseEntity
    {
        public int Id { get; set; }

        public string Token { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
