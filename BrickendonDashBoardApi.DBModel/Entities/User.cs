using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BrickendonDashboard.DBModel.Entities
{
  public class User : BaseEntity
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MobileNumber { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public UserType UserType { get; set; }

    [ForeignKey("UserType")]
    public int UserTypeId { get; set; }
    public List<UserRole> UserRole { get; set; }

    public bool IsActive { get; set; }

    }
}
