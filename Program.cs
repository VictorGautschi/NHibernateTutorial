using HibernatingRhinos.Profiler.Appender.NHibernate;
using NHibernate.Cache;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;

using System;
using System.Linq;
using System.Reflection;

namespace NHibernateTutorial
{
    class Program
    {
        static void Main(string[] args)
        {
            NHibernateProfiler.Initialize();
            var cfg = new Configuration();

            
            cfg.DataBaseIntegration(x => {
                x.ConnectionString = "Data Source=DESKTOP-B5KCRVC;Initial Catalog=NHibernateTutorialDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                //x.ConnectionString = "default"; // when using XML-configuration
                x.Driver<SqlClientDriver>();
                x.Dialect<MsSql2008Dialect>();
                x.LogSqlInConsole = true;
                x.BatchSize = 10;
            });


            // here it find the mapping files to tell NHibernate how to go from C# classes into the database tables
            //cfg.Configure(); // not working to configure through XML
               
            /* Caching
            cfg.Cache(c => {
                c.UseMinimalPuts = true;
                c.UseQueryCache = true;
            });
            cfg.SessionFactory().Caching.Through<HashtableCacheProvider>()
               .WithDefaultExpiration(1440);
            */

            cfg.AddAssembly(Assembly.GetExecutingAssembly());
            
            var sefact = cfg.BuildSessionFactory();
            
            using (var session = sefact.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {

                    /* Cache example - showing that only 1 of the below will run
                    var studentUsingTheFirstQuery = session.Get<Student>(1);
                    var studentUsingTheSecondQuery = session.Get<Student>(1);
                    */

                    /* for (int i = 0; i < 25; i++)
                    {

                        var student = new Student
                        {
                            // ID = 100 + i, // guid is creating this automatically now
                            FirstName = "FirstName" + i.ToString(),
                            LastName = "LastName" + i.ToString(),
                            AcademicStanding = StudentAcademicStanding.Good
                        };

                        session.Save(student);
                    }
                    */

                    var students = session.CreateCriteria<Student>().List<Student>();

                    /* Create Records 
                    var student1 = new Student
                    {
                        ID = 1,
                        FirstName = "Allan",
                        LastName = "Bommer",
                        AcademicStanding = StudentAcademicStanding.Excellent,
                        Address = new Location
                        {
                            Street = "123 Street",
                            City = "Lahore",
                            Province = "Punjab",
                            Country = "Pakistan"
                        }
                    };
                    
                    var student2 = new Student
                    {
                        ID = 2,
                        FirstName = "Jerry",
                        LastName = "Lewis",
                        AcademicStanding = StudentAcademicStanding.Good,
                        Address = new Location
                        {
                            Street = "124 Street",
                            City = "Lahore",
                            Province = "Punjab",
                            Country = "Pakistan"
                        }
                    };

                    session.Save(student1);
                    session.Save(student2); 
                    */

                    /* Fetch Record
                    var stdnt = session.Get<Student>(2);
                    Console.WriteLine("Retrieved by ID");
                    Console.WriteLine("{0} \t{1} \t{2}", stdnt.ID, stdnt.FirstName, stdnt.LastName);
                    */

                    /* Update Record
                    Console.WriteLine("Update the last name of ID = {0}", stdnt.ID);
                    stdnt.LastName = "Donald";
                    session.Update(stdnt); 
                    */

                    /* Delete Record
                    Console.WriteLine("Delete the record which has ID = {0}", stdnt.ID);
                    session.Delete(stdnt);
                    Console.WriteLine("\nFetch the complete list again\n");
                    */

                    Console.WriteLine("\nFetch the complete list again\n");

                    /* Display list of Records */
                    foreach (var student in students)
                    {
                        Console.WriteLine("{0} \t{1} \t{2}", student.ID, student.FirstName, student.LastName);
                    }

                    tx.Commit();
                }

                Console.ReadLine();
            } 
        }
    }
}

