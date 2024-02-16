using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace BrickendonDashboard.DBModel.Entities
{
  public class UserRoles : BaseEntity
  {

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [ForeignKey("User")]
    public int UserId { get; set; }  
    public User User { get; set; }    

    [ForeignKey("Roles")]
    public int RoleId { get; set; }   
    public Roles Roles { get; set; }

  }
}
