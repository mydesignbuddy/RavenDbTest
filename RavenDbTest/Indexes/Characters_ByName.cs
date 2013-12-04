using System.Linq;
using Raven.Client.Indexes;

namespace RavenDbTest.Indexes
{
    public class Characters_ByName : AbstractIndexCreationTask<Character>
    {
        public Characters_ByName()
        {
            Map = characters => from character in characters
                select new {character.Name};
        }
    }
}
