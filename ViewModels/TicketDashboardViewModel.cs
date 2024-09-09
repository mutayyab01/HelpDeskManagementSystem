using HelpDeskSystem.Models;

namespace HelpDeskSystem.ViewModels
{
    public class TicketDashboardViewModel
    {
        public TicketsSummaryView TicketsSummary { get; set; }
        public List<Ticket> Tickets { get; set; }
        public Ticket Ticket { get; set; }
        public TicketsPriorityView TicketsPriority { get; set; }

    }
}
