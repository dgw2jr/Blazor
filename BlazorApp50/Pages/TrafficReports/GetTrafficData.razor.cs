using BlazorApp50.Microservices.Traffic.Data.Models;
using BlazorApp50.Microservices.Traffic.Messages;
using Blazored.Modal;
using Blazored.Modal.Services;
using MassTransit;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp50.Pages.TrafficReports
{
    public partial class GetTrafficData : ComponentBase
    {
        [Parameter]
        public List<TrafficReport> TrafficReports { get; set; }

        [Inject]
        public IRequestClient<IGetTrafficReportsMessage> RequestClient { get; set; }

        [CascadingParameter] 
        public IModalService Modal { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var response = await RequestClient.GetResponse<IGetTrafficReportsResult>(new { });
            var result = response.Message.TrafficReports;

            TrafficReports = result;
        }

        public async Task ShowModal()
        {
            var formModal = Modal.Show<EditTrafficReport>("Create Traffic Report", new ModalOptions { Animation = ModalAnimation.FadeInOut(1) });
            var result = await formModal.Result;

            if (result.Cancelled)
            {
                Console.WriteLine("Modal was cancelled");
            }
            else
            {
                TrafficReports.Insert(0, result.Data as TrafficReport);
            }
        }
    }
}
