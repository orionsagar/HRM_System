using Domains.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.Queries.QPaymentType
{
    public class GetAllPaymentTypeQuery : IRequest<List<PaymentType>>
    {
        public int OrgId { get; set; }
    }
}
