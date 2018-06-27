using System;
using MountainAddicted.Database;
using System.Data.Entity;
using NUnit.Framework;
using System.Threading.Tasks;

namespace MountainAddicted.Tests
{
    [TestFixture]
    public class DatabaseConnectionTest
    {
        [Test]
        public async Task CreateDatabaseAndAddMountain()
        {
            using (var db = new MountainDbContext("MountainDBTestConnectionString"))
            {
                var mountain = new MountainDbData
                {
                    NeLat = 3,
                    Title = "Everest",
                    Description = "Highest mountain in the world!"
                };
                db.Mountains.Add(mountain);
                db.SaveChanges();

                Assert.AreEqual(1, await db.Mountains.CountAsync());

                db.Mountains.Remove(mountain);
                db.SaveChanges();

                Assert.AreEqual(0, await db.Mountains.CountAsync());
            }
        }
    }
}
