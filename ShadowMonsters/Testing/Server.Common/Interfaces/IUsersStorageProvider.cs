using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Common.Interfaces
{
    public interface IUsersStorageProvider
    {
        Task<IList<Character>> GetCharacters(int accountId);
    }
}