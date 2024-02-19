using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectFb.Application.Utilities.Exceptions
{
    public class WrongRequestException : Exception
    {
        public WrongRequestException(string message) : base(message)
        {

        }
    }
}
