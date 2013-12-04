using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using Raven.Client.Indexes;
using Raven.Client.Linq;
using RavenMvcApp.Models;

namespace RavenMvcApp.Indexes
{
    public class People_CountByState : AbstractIndexCreationTask<Person, People_CountByState.ReduceResults>
    {
        public class ReduceResults
        {
            public string State { get; set; }
            public int Count { get; set; }
        }

        public People_CountByState()
        {
            Map = people => from person in people
                select new
                {
                    State = person.Address.State,
                    Count = 1
                };

            Reduce = results => from result in results
                group result by result.State
                into g
                select new
                {
                    State = g.Key,
                    Count = g.Sum(x => x.Count)
                };
        }
    }
}