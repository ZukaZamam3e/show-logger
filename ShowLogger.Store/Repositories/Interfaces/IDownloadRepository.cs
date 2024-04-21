using ShowLogger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowLogger.Store.Repositories.Interfaces;
public interface IDownloadRepository
{
    ExportModel ExportData();
}
