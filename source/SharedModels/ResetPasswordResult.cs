namespace SiSystems.SharedModels
{
    public class ResetPasswordResult
    {
        public bool IsSuccess { get { return ResponseCode > 0; } }

        public string Description { get; set; }

        public int ResponseCode { get; set; }
    }
}
