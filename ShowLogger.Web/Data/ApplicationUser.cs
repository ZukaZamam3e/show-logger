using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ShowLogger.Web.Data;

public class ApplicationUser : IdentityUser
{
    public int UserId { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }
}
