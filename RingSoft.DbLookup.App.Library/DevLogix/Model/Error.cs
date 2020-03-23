using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RingSoft.DbLookup.App.Library.DevLogix.Model
{
    [Table(("TB_Errors"))]
    public class Error
    {
        [Column("intErrorID")]
        public int Id { get; set; }

        [Column("strErrorNo")]
        public string Number { get; set; }

        [Column("dteDate")]
        public DateTime Date { get; set; }

        [Column("intStatusID")]
        public int StatusId { get; set; }

        [Column("intProductID")]
        public int ProductId { get; set; }

        [Column("intPriorityID")]
        public int PriorityId { get; set; }

        [Column("dteFixedDate")]
        public DateTime FixedDate { get; set; }

        [Column("dteCompletedDate")]
        public DateTime CompletedDate { get; set; }

        [Column("intAssignedToID")]
        public int? AssignedToId { get; set; }

        public virtual User AssignedToUser { get; set; }

        [Column("intTesterID")]
        public int? TesterId { get; set; }

        public virtual User TestUser { get; set; }

        [Column("txtDescription")]
        public string Description { get; set; }

        [Column("txtResolution")]
        public string Resolution { get; set; }

        [Column("intFoundVersionID")]
        public int FoundVersionId { get; set; }

        [Column("intFixedVersionID")]
        public int FixedVersionId { get; set; }

        [Column("intOutlineID")]
        public int OutlineId { get; set; }

        [Column("decHrsSpent")]
        public double HoursSpent { get; set; }
    }
}
