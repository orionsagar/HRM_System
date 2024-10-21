﻿using Domains.ViewModels;
using MediatR;

namespace Application.Tasks.Commands.CUserInfo
{
    public class CreateUserInfoCommand : IRequest<int>
    {
        public UserInfoViewModel userInfoViewModel { get; set; }
    }
}