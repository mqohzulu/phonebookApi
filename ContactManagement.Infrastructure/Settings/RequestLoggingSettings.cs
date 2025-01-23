using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Infrastructure.Settings
{
    public class RequestLoggingSettings
    {
        public bool LogRequestBody { get; set; } = true;
        public bool LogQueryParameters { get; set; } = true;
        public List<string> ExcludePaths { get; set; } = new();
        public List<string> ExcludeContentTypes { get; set; } = new();
        public int MaxRequestBodyLength { get; set; } = 32768; // 32KB
    }
}
