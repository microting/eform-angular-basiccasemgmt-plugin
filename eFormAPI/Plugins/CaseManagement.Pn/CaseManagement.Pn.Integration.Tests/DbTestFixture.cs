using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Runtime.InteropServices;
using Microting.eFormBasicCaseManagementBase.Infrastructure.Data;
using Microting.eFormBasicCaseManagementBase.Infrastructure.Data.Factories;

namespace CaseManagement.Pn.Integration.Tests
{
    [TestFixture]
    public abstract class DbTestFixture
    {

        protected eFormCaseManagementPnDbContext DbContext;
        protected string ConnectionString;

        //public RentableItemsPnDbAnySql db;

        public void GetContext(string connectionStr)
        {

            CaseManagementPnDbContextFactory contextFactory = new CaseManagementPnDbContextFactory();
            DbContext = contextFactory.CreateDbContext(new[] {connectionStr});
            
            DbContext.Database.Migrate();
            DbContext.Database.EnsureCreated();                

        }

        [SetUp]
        public void Setup()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ConnectionString = @"data source=(LocalDb)\SharedInstance;Initial catalog=case-mamangement-pn-tests;Integrated Security=True";
            }
            else
            {
                ConnectionString = @"Server = localhost; port = 3306; Database = case-mamangement-pn-tests; user = root; password = secretpassword; Convert Zero Datetime = true;";
            }


            GetContext(ConnectionString);


            DbContext.Database.SetCommandTimeout(300);

            try
            {
                ClearDb();
            }
            catch
            {
                DbContext.Database.Migrate();
            }

            DoSetup();
        }

        [TearDown]
        public void TearDown()
        {

            ClearDb();

            ClearFile();

            DbContext.Dispose();
        }

        public void ClearDb()
        {
            List<string> modelNames = new List<string>();
            modelNames.Add("CalendarUsers");
            modelNames.Add("CaseManagementSettings");


            foreach (var modelName in modelNames)
            {
                try
                {
                    string sqlCmd = string.Empty;
                    if (DbContext.Database.IsMySql())
                    {
                        sqlCmd = string.Format("DELETE FROM `{0}`.`{1}`", "case-mamangement-pn-tests", modelName);
                    }
                    else
                    {
                        sqlCmd = string.Format("DELETE FROM [{0}]", modelName);
                    }
                    DbContext.Database.ExecuteSqlCommand(sqlCmd);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        private string path;

        public void ClearFile()
        {
            path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            path = System.IO.Path.GetDirectoryName(path).Replace(@"file:\", "");

            string picturePath = path + @"\output\dataFolder\picture\Deleted";

            DirectoryInfo diPic = new DirectoryInfo(picturePath);

            try
            {
                foreach (FileInfo file in diPic.GetFiles())
                {
                    file.Delete();
                }
            }
            catch { }


        }
        public virtual void DoSetup() { }

    }
}
