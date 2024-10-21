using Domains.Models;
using MediatR;
using System.Collections.Generic;

namespace Application.Tasks.Queries.QCompany
{
    public class GetAllCompanyQuery : IRequest<List<Company>>
    {
    }
}
