using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowLogger.Models;
public class UpdateUnlinkedShowNameModel
{
    [Display(Name = "Show Name")]
    public string UpdateUnlinkedShowNameShowName { get; set; }

    [Display(Name = "New Show Name")]
    public string UpdateUnlinkedShowNameNewShowName { get; set; }

    [Display(Name = "Show Type Id")]
    public int UpdateUnlinkedShowNameShowTypeId { get; set; }
}
