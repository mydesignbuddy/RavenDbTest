using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Indexes;

namespace RavenDb2Test.Indexes
{
    public class Characters_ByClassName : AbstractIndexCreationTask<Character>
    {
        public Characters_ByClassName()
        {
            Map = characters => from character in characters
                select new {character.Class.Name};
        }
    }
}
