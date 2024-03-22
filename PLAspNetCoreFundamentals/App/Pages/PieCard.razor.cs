using PLAspNetCoreFundamentals.Models;
using Microsoft.AspNetCore.Components;

namespace PLAspNetCoreFundamentals.App.Pages
{
    public partial class PieCard
    {
        [Parameter]
        public Pie? Pie { get; set; }
    }
}
