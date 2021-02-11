using BlazorApp50.Microservices.Traffic.Data.Dtos;
using BlazorApp50.Microservices.Traffic.Data.Models;
using BlazorApp50.Microservices.Traffic.Messages;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace BlazorApp50.Pages.TrafficReports
{
    public partial class EditTrafficReport : ComponentBase
    {
        [CascadingParameter] 
        BlazoredModalInstance ModalInstance { get; set; }

        private readonly TrafficReportDto Report = new TrafficReportDto();

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