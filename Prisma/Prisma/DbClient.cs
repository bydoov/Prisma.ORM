using Prisma.ORM;

namespace Prisma
{
    public class DbClient : PrismaClient
    {
        public DbClient() : base("Server=DESKTOP-TA3U8B5;Database=А;Trusted_Connection=true;MultipleActiveResultSets=true;")
        {

        }

        public DbSet<Blog> Blog { get; set; }
    }
}
