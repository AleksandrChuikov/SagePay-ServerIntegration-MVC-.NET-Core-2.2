using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SagePayServerIntegration.Entities
{
    public class State
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
