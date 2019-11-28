using System.Collections.Generic;
using System.Linq;
using Infrastructure.Data.Core;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Test.NUnit.Model;
using Test.NUnit.Repository;

namespace Test.NUnit
{
    public class TestData
    {
        private InMemoryContext _dbContext;

        [SetUp]
        public void Setup()
        {
            _dbContext = new InMemoryContext();
            if (!_dbContext.Users.Any())
            {
                _dbContext.AddRange(TestUsers);
                _dbContext.SaveChanges();
            }
        }

        [Test]
        public void GetUser()
        {
            var userRepository = new EfRepository<User>(_dbContext);
            var user = userRepository.FindById(1);
            Assert.NotNull(user);            
            user.Name = "AA";
            userRepository.Update(user);
            Assert.That(_dbContext.SaveChanges() > 0);
            Assert.NotNull(userRepository.Get(w => w.Name.Equals("AA")));

            var unitOfWork = new UnitOfWork<DbContext>(_dbContext);

            var userService = unitOfWork.GetRepository<User>();
            Assert.NotNull(userService.Get(w => w.Id == 1));
            Assert.NotNull(userService.GetList());
            Assert.NotNull(userService.GetPage());

            var userCustomRepository = unitOfWork.GetRepository<User, UserRepository>();
            Assert.NotNull(userCustomRepository);
        }

        private IEnumerable<User> TestUsers => new List<User>
        {
            new User {Id = 1, Name = "A"},
            new User {Id = 2, Name = "B"}
        };
    }
}