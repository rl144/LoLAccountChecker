namespace LoL_Account_Checker
{
    internal class LoginData
    {
        public string Password;
        public string Username;

        public LoginData(string usr, string pass)
        {
            Username = usr;
            Password = pass;
        }
    }
}