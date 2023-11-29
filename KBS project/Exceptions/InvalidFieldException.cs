using KBS_project.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBS_project.Exceptions
{
    public class InvalidFieldException : Exception
    {
        public RegistrationFields Field { get; set; }
        public InvalidFieldException(RegistrationFields field)
        {
            Field = field;
        }
    }
}
