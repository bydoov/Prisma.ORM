using System.Collections.Generic;

namespace Prisma.ORM
{
    public class DbSet<T> 
    {
        public string TableName { get; set; }

        public DbSet()
        {
            TableName = typeof (T).Name;
        }

        public Dictionary<string,string> Values { get; set; }

    }
}
