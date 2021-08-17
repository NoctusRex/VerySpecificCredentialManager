namespace VerySpecificCredentialManager.Models
{
    public class Credential
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ProcessName { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is not Credential)
            {
                return false;
            }

            Credential credential = (Credential)obj;
            return UserName == credential.UserName && Password == credential.Password && ProcessName == credential.ProcessName;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
