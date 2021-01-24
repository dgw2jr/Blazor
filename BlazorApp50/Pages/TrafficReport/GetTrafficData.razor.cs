using Blazored.Modal;
using Blazored.Modal.Services;
using Core;
using MassTransit;
using Messages;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp50.Pages.TrafficReport
{
    public partial class GetTrafficData : ComponentBase
    {
        [Inject]
        private TrafficContext Context { get; set; }

        [Parameter]
        public List<Core.TrafficReport> TrafficReports { get; set; }

        [CascadingParameter] 
        public IModalService Modal { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var result = Context.TrafficReports
                .AsNoTracking()
                .AsQueryable();

            TrafficReports = result.ToList();
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
                TrafficReports.Insert(0, result.Data as Core.TrafficReport);
            }
        }
    }
}
