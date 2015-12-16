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

        public int NumberOfPhoneNumbers()
        {
            return _clientContract.PhoneNumbers.Count();
        }

        public int NumberOfEmails()
        {
            return _clientContract.EmailAddresses.Count();
        }

        public EmailAddress EmailAddressByRowNumber(int row)
        {
            if (_clientContract.EmailAddresses.Count() > row)
                return _clientContract.EmailAddresses.ElementAt(row);
            return new EmailAddress();
        }

        public PhoneNumber PhoneNumberByRowNumber(int row)
        {
            if (_clientContract.PhoneNumbers.Count() > row)
                return _clientContract.PhoneNumbers.ElementAt(row);
            return new PhoneNumber();
        }
    }
}