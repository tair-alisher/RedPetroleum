using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RedPetroleum.Models.Entities
{
    [Table("ACL")]
    public class ACL
    {   
        public Guid AclId { get; set; }
        public string UserId { get; set; }
        public int RoleId { get; set; }
        public string GrandUrl { get; set; }
        public string DenyUrl { get; set; }
    }  
}