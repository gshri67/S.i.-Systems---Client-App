using System;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using DbUp;

namespace SiSystems.ClientApp.Database.MatchGuide
{
    class Program
    {
        static int Main(string[] args)
        {
            var options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                //If no operation chosen, default to Upgrade
                if (!options.Destroy && !options.Create && !options.Upgrade && !options.Seed && !options.Wipe)
                {
                    options.Upgrade = true;
                }

                options.ConnectionString = CoalesceConnectionString(options);

                try
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(options.ConnectionString))
                            throw new ArgumentException();

                        var csb = new System.Data.Common.DbConnectionStringBuilder();
                        csb.ConnectionString = options.ConnectionString; // throws
                    }
                    catch
                    {
                        throw new AbortProcessException(String.Format("'{0}' is an invalid connection string.", options.ConnectionString));
                    }

                    if (options.Destroy)
                    {
                        DestroyDatabase(options);
                    }

                    if (options.Create)
                    {
                        CreateDatabase(options);
                    }

                    if (options.Upgrade)
                    {
                        UpgradeDatabase(options);
                    }
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.ResetColor();

                    return -1;
                }

                return 0;
            }

            return -1;
        }

        private static string CoalesceConnectionString(Options options)
        {
            if (!String.IsNullOrWhiteSpace(options.ConnectionString))
            {
                return options.ConnectionString;
            }

            return System.Configuration.ConfigurationManager.AppSettings["connectionString"];
        }

        //We can't convert database names to parameters... not supported.
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        private static void DestroyDatabase(Options options)
        {
            var dbName = (new SqlConnectionStringBuilder(options.ConnectionString)).InitialCatalog;
            var server = (new SqlConnectionStringBuilder(options.ConnectionString)).DataSource;

            if (options.Check)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(@"Would drop database [{0}\{1}].", server, dbName);
                Console.WriteLine();
                Console.ResetColor();
                return;
            }

            ConsoleKeyInfo option = new ConsoleKeyInfo();

            if (!options.Force)
            {
                do
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(@"Are you absolutely sure you want to drop your database [{0}\{1}] [y/N]? ", server, dbName);
                    Console.ResetColor();
                    option = Console.ReadKey();
                } while (option.Key != ConsoleKey.Y && option.Key != ConsoleKey.N && option.Key != ConsoleKey.Enter);
            }

            Console.WriteLine();

            if (option.Key == ConsoleKey.N || option.Key == ConsoleKey.Enter)
            {
                throw new AbortProcessException("Aborting database destroy change");
            }

            var connectionString = options.ConnectionString.Replace(dbName, "master");

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var exists = new SqlCommand(string.Format(@"select coalesce((select 1 from sys.databases where name='{0}'), 0)", dbName), conn);
                if ((int)exists.ExecuteScalar() == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine(@"Database [{0}\{1}] does not exist.", server, dbName);
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(@"Dropping existing database [{0}\{1}]", server, dbName);
                    Console.ResetColor();
                    var sqlReset = string.Format(@"
                        ALTER DATABASE {0} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                        DROP DATABASE [{0}];", dbName);
                    var sqlCommand = new SqlCommand(sqlReset, conn);
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        //We can't convert database names to parameters... not supported.
        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        private static void CreateDatabase(Options options)
        {
            var dbName = (new SqlConnectionStringBuilder(options.ConnectionString)).InitialCatalog;
            var server = (new SqlConnectionStringBuilder(options.ConnectionString)).DataSource;

            if (options.Check)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(@"Would create database [{0}\{1}] if it doesn't exist.", server, dbName);
                Console.WriteLine();
                Console.ResetColor();
                return;
            }

            var connectionString = options.ConnectionString.Replace(dbName, "master");

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var exists = new SqlCommand(string.Format(@"select coalesce((select 1 from sys.databases where name='{0}'), 0)", dbName), conn);
                if ((int)exists.ExecuteScalar() != 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Error.WriteLine(@"Database [{0}\{1}] already exists.", server, dbName);
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine(@"Creating database [{0}\{1}]", server, dbName);
                    var sqlReset = string.Format(@"
                        CREATE DATABASE [{0}]", dbName);
                    var sqlCommand = new SqlCommand(sqlReset, conn);
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        private static void UpgradeDatabase(Options options)
        {
            try
            {
                var upgrader = DeployChanges.To
                                            .SqlDatabase(options.ConnectionString)
                                            //.WithTransaction()
                                            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                                            .LogToConsole()
                                            .Build();

                var pendingScripts = upgrader.GetScriptsToExecute();

                if (pendingScripts.Any())
                {
                    if (options.Check)
                    {
                        Console.WriteLine("Upgrade is required.");
                        Console.WriteLine("Scripts to run:");
                        pendingScripts.ForEach(s =>
                        {
                            Console.WriteLine("\t{0}", s.Name);
                        });
                    }
                    else
                    {

                        var result = upgrader.PerformUpgrade();

                        if (!result.Successful)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Error.WriteLine(result.Error);
                            Console.ResetColor();
                        }

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Success!");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.WriteLine("No upgrade is required");
                }
            }
            catch (Exception e)
            {
                throw new AbortProcessException(e.Message);
            }
        }


        
    }
}
