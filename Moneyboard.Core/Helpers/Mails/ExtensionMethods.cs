using Moneyboard.Core.Entities.UserEntity;
using Moneyboard.Core.Exeptions;
using Moneyboard.Core.Resources;
using System.Net;

namespace Moneyboard.Core.Helpers.Mails
{
    public static class ExtensionMethods
    {
        public static void UserNullChecking(this User user)
        {
            if (user == null)
            {
                throw new HttpException(HttpStatusCode.NotFound,
                    ErrorMessages.UserNotFound);
            }
        }

       /* public static void InviteNullChecking(this InviteUser userInvite)
        {
            if (userInvite == null)
            {
                throw new HttpException(HttpStatusCode.NotFound,
                    ErrorMessages.InviteNotFound);
            }
        }*/

      /*  public static void CommentNullChecking(this Comment comment)
        {
            if (comment == null)
            {
                throw new HttpException(HttpStatusCode.NotFound,
                    ErrorMessages.CommentNotFound);
            }
        }*/
    }
}
