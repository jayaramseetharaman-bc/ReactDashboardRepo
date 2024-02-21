using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace BrickendonDashboard.DBModel.Entities
{
  public class UserRole : BaseEntity
  {

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [ForeignKey("User")]
    public int UserId { get; set; }  
    public User User { get; set; }    

    [ForeignKey("Role")]
    public int RoleId { get; set; }   
    public Role Role { get; set; }

  }
}
