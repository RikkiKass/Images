using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Images.Data;
namespace Images.Web.Models
{
    public class ViewImageViewModel
    {
         public Image Image { get; set; }
         public bool CanProceed { get; set; }
        public string Message { get; set; }
        
    }
}
