using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientApp.Services;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using SiSystems.ClientApp.SharedModels;

namespace SiSystems.ClientApp.Tests
{
    [TestFixture]
    public class MatchGuideConstantsTests
    {
        public class FakeObject
        {
            public MatchGuideConstants.ResumeRating Rating;

            public MatchGuideConstants.ResumeRating? NullableRating;
        }

        [Test]
        public void ResumeRating_FromJsonInt_CanBeDeserialized()
        {
            const string jsonString = "{\"rating\":317}";
            var sut = JsonConvert.DeserializeObject<FakeObject>(jsonString);

            Assert.AreEqual(317, sut.Rating);
        }

        [Test]
        [ExpectedException]
        public void ResumeRating_FromJsonStringToNonNullableProperty_ShouldFail()
        {
            const string jsonString = "{\"rating\":\"317\"}";
            var sut = JsonConvert.DeserializeObject<FakeObject>(jsonString);
        }

        [Test]
        [ExpectedException]
        public void ResumeRating_FromJsonStringToNullableProperty_ShouldFail()
        {
            const string jsonString = "{\"nullableRating\":\"317\"}";
            var sut = JsonConvert.DeserializeObject<FakeObject>(jsonString);
        }

        [Test]
        public void ResumeRating_FromJsonNullToNullableProperty_CanBeDeserialized()
        {
            const string jsonString = "{\"nullableRating\":null}";
            var sut = JsonConvert.DeserializeObject<FakeObject>(jsonString);

            Assert.IsFalse(sut.NullableRating.HasValue);
        }

        [Test]
        [ExpectedException]
        public void ResumeRating_FromJsonNullToNonNullableProperty_ShouldFail()
        {
            const string jsonString = "{\"rating\":null}";
            var sut = JsonConvert.DeserializeObject<FakeObject>(jsonString);
        }

        [Test]
        public void ResumeRating_ToString_ShouldDisplayDefaultString()
        {
            const string jsonString = "{\"rating\":0}";
            var sut = JsonConvert.DeserializeObject<FakeObject>(jsonString);

            Assert.AreEqual("Not Checked", sut.Rating.ToString());
        }

        [Test]
        public void ResumeRating_ToString_ShouldDisplayStandardString()
        {
            string jsonString = string.Format("{{\"rating\":{0}}}", MatchGuideConstants.ResumeRating.Standard);
            var sut = JsonConvert.DeserializeObject<FakeObject>(jsonString);

            Assert.AreEqual("Standard", sut.Rating.ToString());
        }

        [Test]
        public void ResumeRating_ToString_ShouldDisplayAboveStandardString()
        {
            string jsonString = string.Format("{{\"rating\":{0}}}", MatchGuideConstants.ResumeRating.AboveStandard);
            var sut = JsonConvert.DeserializeObject<FakeObject>(jsonString);

            Assert.AreEqual("Above Standard", sut.Rating.ToString());
        }

        [Test]
        public void ResumeRating_ToString_ShouldDisplayBelowStandardString()
        {
            string jsonString = string.Format("{{\"rating\":{0}}}", MatchGuideConstants.ResumeRating.BelowStandard);
            var sut = JsonConvert.DeserializeObject<FakeObject>(jsonString);

            Assert.AreEqual("Below Standard", sut.Rating.ToString());
        }

        [Test]
        public void ResumeRating_ToString_ShouldDisplayDefaultStringWhenUnknownValue()
        {
            string jsonString = string.Format("{{\"rating\":{0}}}", 1337);
            var sut = JsonConvert.DeserializeObject<FakeObject>(jsonString);

            Assert.AreEqual("Not Checked", sut.Rating.ToString());
        }

        [Test]
        public void ResumeRating_ShouldHandleNullForNullableProperties()
        {
            string jsonString = string.Format("{{\"nullableRating\":{0}}}", "null");
            var sut = JsonConvert.DeserializeObject<FakeObject>(jsonString);

            Assert.IsFalse(sut.NullableRating.HasValue);
            Assert.IsNull(sut.NullableRating);
        }

        [Test]
        public void ResumeRating_ShouldHandleValueForNullableProperties()
        {
            string jsonString = string.Format("{{\"nullableRating\":{0}}}", MatchGuideConstants.ResumeRating.Standard);
            var sut = JsonConvert.DeserializeObject<FakeObject>(jsonString);

            Assert.IsTrue(sut.NullableRating.HasValue);
            Assert.AreEqual(MatchGuideConstants.ResumeRating.Standard, sut.NullableRating.Value);
        }
    }
}
