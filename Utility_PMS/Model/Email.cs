using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utility_PMS.Model;

namespace Utility_PMS.Model
{
    public class Email
    {

        public string To { get; set; }
        public string Subject { get; set; }
        public string MEssage { get; set; }

    }
}
