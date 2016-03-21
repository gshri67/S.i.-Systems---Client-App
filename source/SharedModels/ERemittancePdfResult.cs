namespace SiSystems.SharedModels
{
    public class ERemittancePdfResult
    {
        public bool IsSuccess { get { return ResponseCode > 0; } }

        public string Description { get; set; }

        public int ResponseCode { get; set; }        
    }
}