using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace SiSystems.ClientApp.Database
{
    class Options
    {
        [Option('c', "check", DefaultValue = false, HelpText = "Simply check if the database needs to be updated. Do not perform database changes.")]
        public bool Check { get; set; }

        [Option("force", DefaultValue = false, HelpText = "Force destructive actions, non-interactive")]
        public bool Force { get; set; }

        [Option("destroy", DefaultValue = false, MutuallyExclusiveSet = "Destroy", HelpText = "Drops entire database! Cannot be combined with other DB flags.")]
        public bool Destroy { get; set; }

        [Option("create", DefaultValue = false, MutuallyExclusiveSet = "Create", HelpText = "Create the database from scratch. Cannot be combined with other DB flags.")]
        public bool Create { get; set; }

        [Option("upgrade", DefaultValue = false, MutuallyExclusiveSet = "Upgrade", HelpText = "Upgrade database. Cannot be combined with other DB flags.")]
        public bool Upgrade { get; set; }

        [Option("wipe", DefaultValue = false, MutuallyExclusiveSet = "Wipe", HelpText = "Keep database schema but wipe table contents. Cannot be combined with other DB flags.")]
        public bool Wipe { get; set; }

        [Option("seed", DefaultValue = false, MutuallyExclusiveSet = "Seed", HelpText = "Seed data into database. Cannot be combined with other DB flags.")]
        public bool Seed { get; set; }

        [Option("adduser", DefaultValue = false, MutuallyExclusiveSet = "AddUser", HelpText = "Add the current user into the Persons table. Cannot be combined with other DB flags.")]
        public bool AddUser { get; set; }

        [ValueOption(0)]
        public string ConnectionString { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var help = new HelpText
            {
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            help.AddPreOptionsLine("Usage: Dotplan.Database [options] <connectionString>");
            help.AddPreOptionsLine("");
            help.AddPreOptionsLine("Connection String Example: \"Server=.;Database=dotplan;Trusted_Connection=True;\"");
            help.AddPreOptionsLine("");
            help.AddPreOptionsLine("Note that Database operation flags are mutually exclusive, if you want multiple operations (ie. upgrade and seed) then you'll need to call this application multiple times.");
            help.AddOptions(this);
            return help;
        }
    }
}
