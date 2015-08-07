namespace SiSystems.SharedModels
{
    public class Contact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return FirstName + " " + LastName; } }

        public string EmailAddress { get; set; }
    }
}
