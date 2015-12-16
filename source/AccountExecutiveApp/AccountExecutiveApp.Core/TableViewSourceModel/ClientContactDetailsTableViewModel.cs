using System.Collections.Generic;
using System.Linq;
using SiSystems.SharedModels;

namespace AccountExecutiveApp.Core.TableViewSourceModel
{
    public class ClientContactDetailsTableViewModel
    {
        private UserContact _clientContract;

        public ClientContactDetailsTableViewModel(UserContact clientContract)
        {
            _clientContract = clientContract;
        }

        public string FormattedPhoneNumberByRowNumber(int row)
        {
            if (_clientContract.PhoneNumbers.Count() > row)
                return _clientContract.PhoneNumbers.ElementAt(row).FormattedNumber;
            return string.Empty;
        }
        public string FormattedEmailByRowNumber(int row)
        {
            if (_clientContract.EmailAddresses.Count() > row)
                return _clientContract.EmailAddresses.ElementAt(row).Email;
            return string.Empty;
        }

        public int NumberOfPhoneNumbers()
        {
            return _clientContract.PhoneNumbers.Count();
        }

        public int NumberOfEmails()
        {
            return _clientContract.EmailAddresses.Count();
        }

        public string PhoneNumberContactTypeByRowNumber(int row)
        {
            if (_clientContract.PhoneNumbers.Count() > row)
                return _clientContract.PhoneNumbers.ElementAt(row).Title;
            return string.Empty;
        }

        public string EmailAddressContactTypeByRowNumber(int row)
        {
            if (_clientContract.EmailAddresses.Count() > row)
                return _clientContract.EmailAddresses.ElementAt(row).Title;
            return string.Empty;
        }
    }
}