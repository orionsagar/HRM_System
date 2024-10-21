using Domains.Models;
using MediatR;
using System.Collections.Generic;

namespace Application.Tasks.Queries.QMainModule
{
    public class GetAllMenuPageNameQuery : IRequest<List<UserAccessTools>>
    {
    }
}
