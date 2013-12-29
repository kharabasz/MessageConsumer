using Data.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Data.Commands
{
    public class ExtractionCommand : ICommand
    {
        public long Id { get; set; }

        public string RepositoryLocation { get; set; }

        public void Execute(string message)
        {
            var file = string.Format("C:\\Temp\\{0}.txt", Id);
            Thread.Sleep(10000);
            Tuple<System.Data.DbType, int?, object> item1 = new Tuple<System.Data.DbType, int?, object>(System.Data.DbType.Int64, null, Id);

            IRepository dataRepo = new DataRepository(DatabaseConfiguration.ConnectionString);
            var isExtracted = dataRepo.GetAll<ExtractionResult>("IsExtractionCreated", new Dictionary<string, Tuple<System.Data.DbType, int?, object>>() { { "DocumentHandle", item1 } })[0];
            if (isExtracted.IsExtractionDone == 0)
            {
                Console.WriteLine(" [x] Writing file ...");
                dataRepo.Insert("InsertExtractionStatus", new Dictionary<string, Tuple<System.Data.DbType, int?, object>>() { { "DocumentHandle", item1 } });
                File.Create(file).Dispose();
                File.WriteAllText(file, string.Format("{0}_{1}_ IM A FILE", Id, RepositoryLocation));
            }
            else
            {
                Console.WriteLine(" [x] another process is working on this");
            }
            
        }
    }
}
