using BlazorApp50.Pages.TrafficReports.Dtos;
using BlazorApp50.Pages.TrafficReports.Services;
using Blazored.Modal;
using Blazored.Modal.Services;
using MassTransit;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BlazorApp50.Pages.TrafficReports
{
    public partial class EditTrafficReport : ComponentBase
    {
        [Inject]
        public ITrafficService TrafficService { get; set; }

        [CascadingParameter] 
        BlazoredModalInstance ModalInstance { get; set; }

        private readonly TrafficReportDto Report = new TrafficReportDto();

        private async Task Save()
        {
            await TrafficService.CreateTrafficReport(Report);

            //TODO error checking
            toastService.ShowSuccess("Saved!");

            await ModalInstance.CloseAsync(ModalResult.Ok(Report));
        }
    }
}