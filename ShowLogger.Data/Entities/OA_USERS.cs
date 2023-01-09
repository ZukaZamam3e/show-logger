using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowLogger.Data.Entities;

public class OA_USERS
{
    public int USER_ID { get; set; }

    public string FIRST_NAME { get; set; }

    public string LAST_NAME { get; set; }

    public string USER_NAME { get; set; }
}
