using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BrickendonDashboard.DBModel.Entities
{
  public class Role : BaseEntity
  {
    [Key ]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Key { get; set; }

  }
}
