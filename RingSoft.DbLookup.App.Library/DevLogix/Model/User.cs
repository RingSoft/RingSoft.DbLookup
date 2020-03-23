using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace RingSoft.DbLookup.App.Library.DevLogix.Model
{
    public enum DeveloperTypes
    {
        [Description("Non Developer")]
        NonDeveloper = 0,

        [Description("Quality Assurance")]
        QualityAssurance = 1,

        [Description("Developer")]
        Developer = 2,
    }

    [Table(("TB_Users"))]
    public class User
    {
        [Column("intUserID")]
        public int Id { get; set; }

        [Column("strUserName")]
        public string UserName { get; set; }

        [Column("txtNotes")]
        public string Notes { get; set; }

        [Column("strPassword")]
        public string Password { get; set; }

        [Column("bolSUP")]
        public bool IsSupervisor { get; set; }

        [Column("bytDeveloperType")]
        public DeveloperTypes DeveloperType { get; set; }

        [Column("txtRights")]
        public string Rights { get; set; }
    }
}
