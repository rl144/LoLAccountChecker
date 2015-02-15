namespace LoLAccountChecker.Data
{
    internal class LoginData
    {
        public bool Checked;
        public string Password;
        public string Username;

        public LoginData(string usr, string pass)
        {
            Username = usr;
            Password = pass;
            Checked = false;
        }
    }
}