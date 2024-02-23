using Microsoft.AspNetCore.Components;
using SunScape.Services;
using System.Globalization;

namespace SunScape.Components.Layout
{
    public partial class MultiCultureSelector : ComponentBase
    {
        private List<CultureInfo> _cultureList;

        [Inject]
        protected CultureService CultureService { get; set; }

        protected override void OnInitialized()
        {
            Culture = CultureInfo.CurrentCulture;
            
            _cultureList = CultureService?
                .GetSupportedCultures().Select(CultureInfo.GetCultureInfo)
                .ToList() ?? new();
        }

        private CultureInfo Culture
        {
            get
            {
                return CultureInfo.CurrentCulture;
            }
            set
            {
                if (CultureInfo.CurrentCulture != value)
                {
                    var uri = new Uri(Navigation.Uri).GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped);
                    var cultureEscaped = Uri.EscapeDataString(value.Name);
                    var uriEscaped = Uri.EscapeDataString(uri);

                    Navigation.NavigateTo($"Culture/Set?culture={cultureEscaped}&redirectUri={uriEscaped}", forceLoad: true);
                }
            }
        }

    }
}
