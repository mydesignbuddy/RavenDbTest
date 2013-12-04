using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Indexes;

namespace RavenDb2Test.Indexes
{
    public class Characters_CharacterCountByClass : AbstractIndexCreationTask<Character, Characters_CharacterCountByClass.ReduceResult>
    {
        public class ReduceResult
        {
            public string ClassName { get; set; }
            public int Count { get; set; }
        }

        public Characters_CharacterCountByClass()
        {
            Map = characters => from character in characters
                                select new { ClassName = character.Class.Name, Count = 1 };
            Reduce = results => from result in results
                                group result by result.ClassName
                                    into g
                                    select new
                                    {
                                        ClassName = g.Key,
                                        Count = g.Sum(x => x.Count)
                                    };
        }
    }
}
