using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Prisma.ORM
{
    public class PrismaClient
    {
        private readonly string _connectionString;
        public SqlConnection Connection { get; set; }

        public PrismaClient(string connectionString)
        {
            _connectionString = connectionString;
            CreateDbConnection();
        }

        private void CreateDbConnection()
        {
            Connection = new SqlConnection(_connectionString);
            Connection.Open();
            GetMet();
        }

        private void GetMet()
        {
            var dbContext = GetTheDbSetsFromDbContext();
            SetValueToDbSets(dbContext);
        }

        private Type GetTheDbSetsFromDbContext()
        {
            return (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                    from type in assembly.GetTypes()
                    where type.BaseType == typeof(PrismaClient)
                    select type).First();

        }

        private void SetValueToDbSets(Type dbContext)
        {
            var oji = Activator.CreateInstance(dbContext);
            try
            {
                var dbSetsAsProperties = dbContext
                    .GetProperties()
                    .Where(x => x.PropertyType.Name == "DbSet`1").First();

                var initilze = CreateDbSetToAssing(dbContext);


                dbSetsAsProperties.SetValue(oji, initilze, null);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public object CreateDbSetToAssing(Type dbContext)
        {
            var createInst = dbContext
                   .GetProperties()
                   .Where(x => x.PropertyType.Name == "DbSet`1").Select(x => x.PropertyType).First();

            return Activator.CreateInstance(createInst);
        }

        public void OpenDbConnection() => Connection.Open();

        public void CloseDbConnection() => Connection.Close();

        public List<string> CheckTables()
        {
            var res = new List<string>();
            var sql = $"Select table_name from information_schema.tables";
            var sqlCommand = new SqlCommand(sql, Connection);

            var red = sqlCommand.ExecuteReader();


            for (int i = 0; i < red.FieldCount; i++)
            {
                while (red.Read())
                {
                    var b = red[i];
                    res.Add(b.ToString());
                }
            }

            return res;
        }
    }
}
