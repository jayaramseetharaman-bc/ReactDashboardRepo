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
   public UserTypes UserTypes { get; set; }
   public string Description { get; set; } 

  // public User User { get; set; }

  }
}
