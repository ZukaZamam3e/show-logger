using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowLogger.Models;
public class CodeValueModel
{
    public int CodeValueId { get; set; }

    public int CodeTableId { get; set; }

    public string CodeTable { get; set; }

    public string DecodeTxt { get; set; }
}
