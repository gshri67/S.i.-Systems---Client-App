using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin;

namespace ClientApp.Core.Tests
{
    [SetUpFixture]
    public class SetUp
    {
        [SetUp]
        public void RunBeforeAnyTest()
        {
            Insights.Initialize(Insights.DebugModeKey, "1.0", "ClientApp.Core.Test");
        }
    }
}
