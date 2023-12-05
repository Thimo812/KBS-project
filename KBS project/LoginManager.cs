using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBS_project
{
    public class LoginManager
    {
        private readonly IMatchingAppRepository repository;

        public LoginManager(IMatchingAppRepository repository)
        {
            this.repository = repository;
        }

        public bool ValidateLogin(string username)
        {
            if (!repository.IsValidUsername(username) || username == null)
            {
                Console.WriteLine("Invalid username.");
                return false;
            }

            return true;
        }
    }
}
