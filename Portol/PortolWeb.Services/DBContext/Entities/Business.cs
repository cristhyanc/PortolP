using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortolWeb.Services.DBContext.Entities
{
    public class Business
    {
        [Key]
        public Guid BusinessID { get; set; }
        public string BusinessName { get; set; }

        [ForeignKey("BusinessFK")]
        public ICollection<User> Users { get; set; }
    }
}
