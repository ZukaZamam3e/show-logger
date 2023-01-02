using System.ComponentModel.DataAnnotations.Schema;

namespace ShowLogger.Data.Entities;
public class SL_USER_PREF
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int USER_PREF_ID { get; set; }

    public int USER_ID { get; set; }

    public string DEFAULT_AREA { get; set; }
}
