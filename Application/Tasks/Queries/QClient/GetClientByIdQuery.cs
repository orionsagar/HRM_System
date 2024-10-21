using Domains.ViewModels;
using MediatR;

namespace Application.Tasks.Queries.QClient
{
    public class GetClientByIdQuery : IRequest<ClientViewModel>
    {
        public int ClientID { get; set; }
        public int OrgId { get; set; }
    }
}
