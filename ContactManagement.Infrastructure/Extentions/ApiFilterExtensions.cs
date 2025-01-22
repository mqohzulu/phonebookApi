using ContactManagement.Infrastructure.ActionFilters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Infrastructure.Extentions
{
    public static class ApiFilterExtensions
    {
        public static IMvcBuilder AddApiResultFilter(this IMvcBuilder builder)
        {
            builder.AddMvcOptions(options =>
            {
                options.Filters.Add<ApiResultFilter>();
            });

            return builder;
        }
    }
}
