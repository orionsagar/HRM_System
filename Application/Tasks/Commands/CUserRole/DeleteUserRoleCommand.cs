﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tasks.Commands.CUserRole
{
    public class DeleteUserRoleCommand : IRequest<int>
    {
        public int Id { get; set; }
    }
}