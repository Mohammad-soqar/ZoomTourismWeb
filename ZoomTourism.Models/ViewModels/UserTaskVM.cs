using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomTourism.Models.ViewModels
{
    public class UserTaskVM
    {
        public ATask Task { get; set; }
        public IdentityUser User { get; set; }
    }
}
