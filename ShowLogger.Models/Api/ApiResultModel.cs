using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowLogger.Models.Api;
public class ApiResultModel<T>
{
    public T ApiResultContents { get; set; }

    public string Result { get; set; }
}
