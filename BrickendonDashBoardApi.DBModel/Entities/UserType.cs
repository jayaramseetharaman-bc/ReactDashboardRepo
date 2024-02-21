using BrickendonDashboard.DBModel.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrickendonDashboard.DBModel.Entities
{
  public class UserType : BaseEntity
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
   public int  Id { get; set; }
   public string Name { get; set; }
   public string? Description { get; set; } 
   public string Key { get; set; }

  }
}
