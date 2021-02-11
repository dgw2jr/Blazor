using BlazorApp50.Microservices.Traffic.Data.Dtos;
using BlazorApp50.Microservices.Traffic.Data.Models;
using BlazorApp50.Pages.TrafficReports.Services;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp50.Pages.TrafficReports
{
    public partial class GetTrafficData : ComponentBase
    {
        [Inject]
        public ITrafficService TrafficService { get; set; }

        [Parameter]
        public List<TrafficReportDto> TrafficReports { get; set; }

        [CascadingParameter] 
        public IModalService Modal { get; set; }

        protected override async Task OnInitializedAsync()
        {
            TrafficReports = await TrafficService.GetTrafficReports();
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
                TrafficReports.Insert(0, result.Data as TrafficReportDto);
            }
        }
    }
}