using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Linq;
using RavenDbTest.Indexes;

namespace RavenDbTest
{
    [TestClass]
    public class UnitTest1
    {
        private IDocumentStore documentStore;

        [TestInitialize]
        public void Initialize()
        {
            documentStore = new DocumentStore {Url = "http://localhost:8080"}.Initialize();
            /*documentStore.DatabaseCommands.PutIndex("Characters/byName",
                new IndexDefinitionBuilder<Character>
                {
                    Map = characters => from character in characters 
                                        select new { character.Name }
                });*/
            IndexCreation.CreateIndexes(typeof(Characters_ByName).Assembly, documentStore);
        }

        [TestCleanup]
        public void CleanUp()
        {
            documentStore.Dispose();
        }

        [TestMethod]
        public void Test_Saving_Character()
        {
            using (var session = documentStore.OpenSession())
            {
                var character = new Character
                {
                    Name = "Some Dude",
                    Class = new CharacterClass {Name = "Warrior"},
                    Race = new Race {Name = "Human"},
                    Inventory = new List<Item> {new Item {Name = "Sword", Attack = 20, Defence = 0}}
                };

                session.Store(character);

                session.SaveChanges();
            }
        }

        [TestMethod]
        public void Test_Loading_Character()
        {
            using (var session = documentStore.OpenSession())
            {
                var character = session.Load<Character>("Characters/129");
                Console.WriteLine(character.Name);
                Console.WriteLine(character.Class.Name);
            }
        }

        [TestMethod]
        public void Test_Updating_Character()
        {
            using (var session = documentStore.OpenSession())
            {
                var character = session.Load<Character>("Characters/129");
                character.Name = "Archmage";
                character.Class.Name = "Mage";

                session.SaveChanges();

                Console.WriteLine(character.Name);
                Console.WriteLine(character.Class.Name);
            }
        }

        [TestMethod]
        public void Test_Delete_Character()
        {
            using (var session = documentStore.OpenSession())
            {
                var character = session.Load<Character>("Characters/129");
                session.Delete(character);

                session.SaveChanges();
            }
        }

        [TestMethod]
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

        [TestMethod]
        public void Test_Query_Character_w_Index()
        {
            using (var session = documentStore.OpenSession())
            {
                var characters = session.Query<Character, Characters_ByName>().Where(c => c.Name.StartsWith("Some"));

                foreach (var character in characters)
                {
                    Console.WriteLine(character.Id);
                    Console.WriteLine(character.Name);
                    Console.WriteLine(character.Class.Name);
                }
            }
        }

        [TestMethod]
        public void Test_Query_Character_w_Index_Map_Reduce()
        {
            using (var session = documentStore.OpenSession())
            {
                var result =
                    session.Query<Characters_CharacterCountByClass.ReduceResult, Characters_CharacterCountByClass>()
                        .FirstOrDefault(x => x.ClassName == "Warrior") ?? new Characters_CharacterCountByClass.ReduceResult();


                Console.WriteLine(result.ClassName);
                Console.WriteLine(result.Count);
            }
        }

    }
}