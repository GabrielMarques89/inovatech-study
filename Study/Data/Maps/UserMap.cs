using Study.Data.Models;

namespace Study.Data.Maps
{
    public class UserMap : BaseClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.Id);
            Map(x => x.Login);
            Map(x => x.Password);
        }
    }
}