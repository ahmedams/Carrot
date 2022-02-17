using Carrot.Entities.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carrot.Entities
{
    [Table("Gateway")]
    public class Account : EntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OktaUserId { get; set; }
    }
}
