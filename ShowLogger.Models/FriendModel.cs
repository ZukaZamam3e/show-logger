using System.ComponentModel.DataAnnotations;

namespace ShowLogger.Models;
public class FriendModel
{
    public int Id { get; set; }

    public int FriendUserId { get; set; }

    [Display(Name = "Email")]
    public string FriendEmail { get; set; }

    [Display(Name = "Created Date")]
    public DateTime CreatedDate { get; set; }

    public bool IsPending { get; set; }

    [Display(Name = "Status")]
    public string IsPendingZ => IsPending ? "Pending" : "Added";
}

public class AddFriendModel 
{
    [Display(Name = "Add by Email")]
    public string Email { get; set; }
}