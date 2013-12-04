using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using RavenDb2Test.Indexes;

namespace RavenDb2Test
{
    [TestFixture]
    public class RavenDb2TestWIndexes
    {
        private IDocumentStore documentStore;

        [TestFixtureSetUp]
        public void Initialize()
        {
            documentStore = new DocumentStore { Url = "http://localhost:8081" }.Initialize();

            //create index with DatabaseCommands
            if(documentStore.DatabaseCommands.GetIndex("Characters/byName")==null)
            {
                documentStore.DatabaseCommands.PutIndex("Characters/byName",
                    new IndexDefinitionBuilder<Character>
                    {
                        Map = characters => from character in characters
                                            select new { character.Name }
                    });
            }
            

            //create indexes with a classes from assembly
            IndexCreation.CreateIndexes(typeof(Characters_ByClassName).Assembly, documentStore);
        }

        [TestFixtureTearDown]
        public void CleanUp()
        {
            documentStore.Dispose();
        }

        [Test]
        public void Test_Query_Character_w_Index()
        {
            using (var session = documentStore.OpenSession())
            {
                var characters = session.Query<Character, Characters_ByClassName>().Where(c => c.Name.StartsWith("War"));

                foreach (var character in characters)
                {
                    Console.WriteLine("ID: {0}\tName: {1}\tClass:{2}", character.Id, character.Name, character.Class.Name);
                }
            }
        }

        [Test]
        public void Test_Query_Character_w_Index_Map_Reduce_First_Record()
        {
            using (var session = documentStore.OpenSession())
            {
                var result = session.Query<Characters_CharacterCountByClass.ReduceResult, Characters_CharacterCountByClass>()
                    .FirstOrDefault(x => x.ClassName == "Warrior") ?? new Characters_CharacterCountByClass.ReduceResult();
                Console.WriteLine("Class: {0}\t\tCount: {1}", result.ClassName, result.Count);
            }
        }

        [Test]
        public void Test_Query_Character_w_Index_Map_Reduce()
        {
            using (var session = documentStore.OpenSession())
            {
                var results = session.Query<Characters_CharacterCountByClass.ReduceResult, Characters_CharacterCountByClass>();

                foreach (var result in results)
                {
                    Console.WriteLine("Class: {0}\t\tCount: {1}",result.ClassName, result.Count);
                }
                
            }
        }

        [Test]
        public void Test_Mass_Delete_Characters()
        {
            // mass delete all warriors
            documentStore.DatabaseCommands.DeleteByIndex(
                "Characters/ByClassName", 
                new IndexQuery{ Query = "Name:Warrior" }, 
                allowStale: false
            );
        }
        
        [Test]
        public void Test_Mass_Update_Characters()
        {
            // mass delete all warriors
            documentStore.DatabaseCommands.UpdateByIndex(
                "Characters/ByClassName", 
                new IndexQuery { Query = "Name:Warrior" },
                new []
                {
                    new PatchRequest
                    {
                        Type = PatchCommandType.Set,
                        Name = "Name",
                        Value = "Updated from RavenDB's Patch API"
                    }
                }
            );
        }
    }
}
