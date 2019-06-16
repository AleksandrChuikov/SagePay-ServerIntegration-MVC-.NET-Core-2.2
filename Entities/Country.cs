using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SagePayServerIntegration.Entities
{
    public class Country
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
