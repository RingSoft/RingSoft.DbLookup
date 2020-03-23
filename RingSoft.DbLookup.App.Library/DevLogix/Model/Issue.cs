using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RingSoft.DbLookup.App.Library.DevLogix.Model
{
    [Table("TB_Issues")]
    public class Issue
    {
        [Column("intIssueID")]
        public int Id { get; set; }

        [Column("intTaskID")]
        public int TaskId { get; set; }

        public virtual Task Task { get; set; }

        [Column("strIssueDesc")]
        public string Description { get; set; }

        [Column("bolResolved")]
        public bool IsResolved { get; set; }

        [Column("dteResolved")]
        public DateTime ResolvedDate { get; set; }

        [Column("txtNotes")]
        public string Notes { get; set; }
    }
}
