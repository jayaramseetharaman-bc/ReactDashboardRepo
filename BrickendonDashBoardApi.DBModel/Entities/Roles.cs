using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BrickendonDashboard.DBModel.Entities
{
  public class Roles : BaseEntity
  {
    [Key ]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string RoleName { get; set; }
    public string RoleDescription { get; set; }

  }
}
