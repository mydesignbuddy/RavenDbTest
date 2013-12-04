using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace RavenDb2Test
{
    [TestFixture]
    public class RavenDb2Test
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
        public void Test_Saving_Character()
        {
            using (var session = documentStore.OpenSession())
            {
                var random = new Random();
                var character = new Character
                {
                    Id = "characters/32111111111111",
                    Name = "TEST Some Dude" + random.Next(1,10000),
                    Class = new CharacterClass { Name = "Warrior" },
                    Race = new Race { Name = "Human" },
                    Inventory = new List<Item> { new Item { Name = "Sword", Attack = 20, Defence = 0 } }
                };

                session.Store(character);

                session.SaveChanges();
            }
        }

        [Test]
        public void Test_Loading_Character()
        {
            using (var session = documentStore.OpenSession())
            {
                var character = session.Load<Character>("Characters/33");
                Console.WriteLine(character.Name);
                Console.WriteLine(character.Class.Name);
            }
        }

        [Test]
        public void Test_Updating_Character()
        {
            using (var session = documentStore.OpenSession())
            {
                var character = session.Load<Character>("Characters/33");
                character.Name = "Buddy2";
                character.Class.Name = "Cajun2";


                var character2 = session.Load<Character>("Characters/353");
                character2.Name = "Mike21";
                character2.Class.Name = "Dude2";

                session.SaveChanges();

                Console.WriteLine(character.Name);
                Console.WriteLine(character.Class.Name);
            }
        }

        [Test]
        public void Test_Delete_Character()
        {
            using (var session = documentStore.OpenSession())
            {
                var character = session.Load<Character>("Characters/33");
                session.Delete(character);

                session.SaveChanges();
            }
        }

        [Test]
        public void Test_Query_Character()
        {
            using (var session = documentStore.OpenSession())
            {
                var characters = session.Query<Character>().Where(c => c.Class.Name == "Mage");

                foreach (var character in characters)
                {
                    Console.WriteLine(character.Id);
                    Console.WriteLine(character.Name);
                    Console.WriteLine(character.Class.Name);
                }
            }
        }
    }
}
