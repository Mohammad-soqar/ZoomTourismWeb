using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomTourism.Models.ViewModels
{
    public class UserVM
    {
     
            public string UserId { get; set; }
            public string UserName { get; set; }
            public string Name { get; set; } // Replace with the actual property in your ApplicationUser model
            public IList<string> Roles { get; set; }
        
    }
}
