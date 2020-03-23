using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RingSoft.DbLookup.App.Library.DevLogix.Model
{
    [Table(("TB_Projects"))]
    public class Project
    {
        [Column("intProjectID")]
        public int Id { get; set; }

        [Column("strProject")]
        public string Name { get; set; }

        [Column("txtNotes")]
        public string Notes { get; set; }

        [Column("dteDeadline")]
        public DateTime Deadline { get; set; }

        [Column("dteOriginal")]
        public DateTime OriginalDeadline { get; set; }
    }
}
