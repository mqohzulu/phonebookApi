using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.Common.Models
{
    public class ContactUpdatedEmailMessage : EmailMessage
    {
        public string ContactName { get; set; }
    }

}
