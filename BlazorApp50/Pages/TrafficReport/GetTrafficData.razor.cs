using Blazored.Modal;
using Blazored.Modal.Services;
using Core;
using MassTransit;
using Messages;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp50.Pages.TrafficReport
{
    public partial class GetTrafficData : ComponentBase
    {
        [Parameter]
        public List<WeatherReport> forecasts { get; set; }

        [Inject]
        IRequestClient<GetWeatherReports> WeatherReportsClient { get; set; }

        [CascadingParameter] 
        public IModalService Modal { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var response = await WeatherReportsClient.GetResponse<GetWeatherReportsResult>(new { });
            forecasts = response.Message.WeatherReports;
        }

        public async Task ShowModal()
        {
            var formModal = Modal.Show<CreateWeatherReport>("Create Weather Report", new ModalOptions { Animation = ModalAnimation.FadeInOut(1) });
            var result = await formModal.Result;

            if (result.Cancelled)
            {
                Console.WriteLine("Modal was cancelled");
            }
            else
            {
                forecasts.Insert(0, result.Data as WeatherReport);
            }
        }
    }
}
