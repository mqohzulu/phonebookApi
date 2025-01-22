using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.Authentication.Common.Interfaces
{
    public interface IAuditableEntity
    {
        DateTime CreatedAt { get; set; } 
        string CreatedBy { get; set; }  
        DateTime? ModifiedAt { get; set; } 
        string? ModifiedBy { get; set; }   
        DateTime? DeletedAt { get; set; }  
        string? DeletedBy { get; set; }  
    }
}
