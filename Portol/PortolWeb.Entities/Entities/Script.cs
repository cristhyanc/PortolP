using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortolWeb.Entities
{
    [Table("tblScript")]
    public class Script
    {
        [Key]
        public Guid ScriptID { get; set; }
        public string ScriptName { get; set; }
        public int Seq { get; set; }
        public DateTime ScriptDate { get; set; }
    }
}
