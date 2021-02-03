using BlazorApp50.Microservices.TrafficReport.Messages;
using Blazored.Modal;
using Blazored.Modal.Services;
using Core;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace BlazorApp50.Pages.TrafficReport
{
    public partial class EditTrafficReport : ComponentBase
    {
        [CascadingParameter] 
        BlazoredModalInstance ModalInstance { get; set; }

        private readonly Core.TrafficReport Report = new Core.TrafficReport();

        [Inject]
        private TrafficContext Context { get; set; }

        private async Task Save()
        {
            Report.CreatedDate = DateTime.UtcNow;

            await bus.Publish<ITrafficReportCreatedMessage>(new
            {
                Summary = Report.Summary,
                CreatedDate = Report.CreatedDate
            });

            toastService.ShowSuccess("Saved!");

            await ModalInstance.CloseAsync(ModalResult.Ok(Report));
        }
    }
}