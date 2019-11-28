using Infrastructure.Data;
using Infrastructure.Data.Core;
using Microsoft.EntityFrameworkCore;

namespace Test.NUnit.Repository
{
    public class UserRepository : EfRepository<Model.User>, IRepository<Model.User>
    {
        public UserRepository(DbContext dbContext) : base(dbContext)
        {

        }

        public string Call(string userId)
        {

            return $"正在链接：{userId}";
        }

    }
}
