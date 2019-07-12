using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WellService.Models
{
    public class ClientQuota
    {
        public string Email { get; set; }
        public int Quota { get; set; }
        [Key]
        public int ClientID { get; set; }
        [Required]
        public string ClientName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
   
}
