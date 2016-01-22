using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccountExecutiveApp.Core.ViewModel;
using Moq;
using NUnit.Framework;
using Shared.Core;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.Tests.ViewModels
{
    [TestFixture]
    public class UserContactTests
    {
        private Mock<IMatchGuideApi> _mockApi;
        private ClientContactDetailsViewModel _viewModel;

        private const string Zero = "0";

        [SetUp]
        public void Setup()
        {
            _mockApi = new Mock<IMatchGuideApi>();
            _viewModel = new ClientContactDetailsViewModel(_mockApi.Object);
        }

        [Test]
        public void UserContactFullName_WithEmptyName_ReturnLinkedInUrl()
        {
            UserContact contactWithEmptyName = new UserContact();
            contactWithEmptyName.FirstName = string.Empty;
            contactWithEmptyName.LastName = string.Empty;

            SetupDetailsReturnWithUserContact( contactWithEmptyName );

            _viewModel.LoadContact(0);

            Assert.AreNotEqual(string.Empty, _viewModel.LinkedInString);
        }

        [Test]
        public void UserContactFullName_WithMultipleSpaces_ReturnLinkedInUrlWithoutSpaces()
        {
            UserContact contactWithSpacesInName = new UserContact();
            contactWithSpacesInName.FirstName = " Bob Smith ";
            contactWithSpacesInName.LastName = " Smitherson Smithy ";

            SetupDetailsReturnWithUserContact(contactWithSpacesInName);

            _viewModel.LoadContact(0);
            
            Assert.AreEqual(contactWithSpacesInName.LinkedInUrl.Contains(" "), false);
        }

        private void SetupDetailsReturnWithUserContact( UserContact contact )
        {
            _mockApi.Setup(api => api.GetUserContactById(0)).Returns(Task.FromResult(contact));
            _viewModel = new ClientContactDetailsViewModel(_mockApi.Object);
        }
    }
}
