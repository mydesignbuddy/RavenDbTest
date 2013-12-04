using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Extensions;
using Raven.Client.Indexes;

namespace RavenDb2Test
{
    [TestFixture]
    public class RavenDb2Test_databases
    {
        private IDocumentStore documentStore;

        [TestFixtureSetUp]
        public void Initialize()
        {
            documentStore = new DocumentStore { Url = "http://localhost:8081" }.Initialize();
        }

        [TestFixtureTearDown]
        public void CleanUp()
        {
            documentStore.Dispose();
        }

        [Test]
        public void get_databases()
        {
            var databases = documentStore.DatabaseCommands.GetDatabaseNames(10, 0);
            foreach (var database in databases)
            {
                Console.WriteLine(database);
            }
        }

        [Test]
        public void create_database()
        {
            // create DB
            documentStore.DatabaseCommands.CreateDatabase(new DatabaseDocument
            {
                Disabled = false,
                Id = "MyData",
                Settings = new Dictionary<string, string>{
                    { 
                          "Raven/DataDir", "~/Tenants/MyData"
                    }
                }
            });
        }

        [Test]
        public void Test_Saving_Character_in_database()
        {
            using (var session = documentStore.OpenSession("MyData"))
            {
                var random = new Random();
                var character = new Character
                {
                    Name = "Some Dude" + random.Next(1, 10000) + " in MyData",
                    Class = new CharacterClass { Name = "Warrior" },
                    Race = new Race { Name = "Human" },
                    Inventory = new List<Item> { new Item { Name = "Sword", Attack = 20, Defence = 0 } }
                };

                session.Store(character);

                session.SaveChanges();
            }
        }
    }
}
