using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace RavenMvcApp.Dal
{
    public class RavenStore
    {
        public static IDocumentStore Initialize()
        {
            var parser = ConnectionStringParser<RavenConnectionStringOptions>.FromConnectionStringName("RavenDB");
            parser.Parse();

            var Store = new DocumentStore
            {
                Url = parser.ConnectionStringOptions.Url,
                DefaultDatabase = parser.ConnectionStringOptions.DefaultDatabase
            };

            Store.Initialize();

            //Auto Create Indexes from classes
            IndexCreation.CreateIndexes(Assembly.GetExecutingAssembly(), Store);

            return Store;
        }
    }
}