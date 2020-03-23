using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RSDbLookupApp.Library.DevLogix.Model
{
    [Table(("TB_Tasks"))]
    public class Task
    {
        [Column("intTaskID")]
        public int Id { get; set; }

        [Column("strTaskDesc")]
        public string Name { get; set; }

        [Column("dteDueDate")]
        public DateTime DueDate { get; set; }

        [Column("dteCompletedDate")]
        public DateTime CompletedDate { get; set; }

        [Column("decEstHrs")]
        public double EstimatedHours { get; set; }

        [Column("decHrsSpent")]
        public double HoursSpent { get; set; }

        [Column("decOrigEst")]
        public double OriginalEstimate { get; set; }

        [Column("txtNotes")]
        public string Notes { get; set; }

        [Column("intProjectID")]
        public int? ProjectId { get; set; }

        public virtual Project Project { get; set; }

        [Column("intAssignedToID")]
        public int? AssignedToId { get; set; }

        public virtual User AssignedTo { get; set; }


        [Column("decPercentComplete")]
        public double PercentComplete { get; set; }
    }
}
