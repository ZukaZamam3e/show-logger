using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowLogger.Data.Entities;

public class SL_SHOW
{
    public int SHOW_ID { get; set; }

    public int USER_ID { get; set; }

    public string SHOW_NAME { get; set; }

    public int SHOW_TYPE_ID { get; set; }

    public int? SEASON_NUMBER { get; set; }

    public int? EPISODE_NUMBER { get; set; }

    public DateTime DATE_WATCHED { get; set; }

}
