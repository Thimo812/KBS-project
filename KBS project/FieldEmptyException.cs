using KBS_project.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KBS_project
{
    public class FieldEmptyException : Exception
    {
        public RegistrationFields Field {  get; set; }
        public FieldEmptyException(RegistrationFields field)
        {
            Field = field;
        }
    }
}
