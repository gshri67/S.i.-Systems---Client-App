using SiSystems.ClientApp.Web.Domain.MatchGuideService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiSystems.ClientApp.Web.Domain.Fakes
{
    /// <summary>
    /// Use this as a proxy for the actual match guide account service client
    /// to avoid making requests to the actual endpoint.
    /// This is used because we don't have a local version of match guide and
    /// we don't want to accidentally reset people's passwords.
    /// </summary>
    public class FakeMatchGuideAccountServiceClient : IAccountService
    {
        public LoginResponse ForgotPassword(string emailAddress, string portal)
        {
            throw new NotImplementedException();
        }

        public Task<LoginResponse> ForgotPasswordAsync(string emailAddress, string portal)
        {
            return Task.FromResult(new LoginResponse());
        }

        public LoginResponse CandidateLogin(string emailAddress, string Password)
        {
            throw new NotImplementedException();
        }

        public Task<LoginResponse> CandidateLoginAsync(string emailAddress, string Password)
        {
            throw new NotImplementedException();
        }

        public LoginResponse ClientAutoLogin(string userIDTSID)
        {
            throw new NotImplementedException();
        }

        public Task<LoginResponse> ClientAutoLoginAsync(string userIDTSID)
        {
            throw new NotImplementedException();
        }

        public LoginResponse ClientLogin(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public Task<LoginResponse> ClientLoginAsync(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public EContractLoginResponse EContractCandidateLogin(string emailAddress, string Password, string EContractId)
        {
            throw new NotImplementedException();
        }

        public Task<EContractLoginResponse> EContractCandidateLoginAsync(string emailAddress, string Password, string EContractId)
        {
            throw new NotImplementedException();
        }  

        public LinkValidationRes IsValidForgotPasswordLink(string forgotPasswordId)
        {
            throw new NotImplementedException();
        }

        public Task<LinkValidationRes> IsValidForgotPasswordLinkAsync(string forgotPasswordId)
        {
            throw new NotImplementedException();
        }

        public LoginResponse Login(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public Task<LoginResponse> LoginAsync(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public ServiceResponse Logout()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse> LogoutAsync()
        {
            throw new NotImplementedException();
        }

        public LoginResponse SendOTP(string portal, string emailAddress, string userAgent, string platform)
        {
            throw new NotImplementedException();
        }

        public Task<LoginResponse> SendOTPAsync(string portal, string emailAddress, string userAgent, string platform)
        {
            throw new NotImplementedException();
        }

        public LoginResponse UpdatePassword(string forgotPasswordId, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<LoginResponse> UpdatePasswordAsync(string forgotPasswordId, string newPassword)
        {
            throw new NotImplementedException();
        }

        public UserDetailsRes VerifyOTP(string portal, string emailAddress, string userAgent, string platform, string otpCode)
        {
            throw new NotImplementedException();
        }

        public Task<UserDetailsRes> VerifyOTPAsync(string portal, string emailAddress, string userAgent, string platform, string otpCode)
        {
            throw new NotImplementedException();
        }

        public UserDetailsRes VerifyOTPValidity(string portal, string emailAddress, string userAgent, string platform)
        {
            throw new NotImplementedException();
        }

        public Task<UserDetailsRes> VerifyOTPValidityAsync(string portal, string emailAddress, string userAgent, string platform)
        {
            throw new NotImplementedException();
        }
    }
}
