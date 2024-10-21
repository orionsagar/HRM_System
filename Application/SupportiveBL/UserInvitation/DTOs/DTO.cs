using Domains.Models;
using System;
using System.Collections.Generic;

namespace Application.SupportiveBL.UserInvitation
{
    public class InviteModel
    {
        public string Input { get; set; }
        public string Hash { get; set; }
        public bool HasExpiry { get; set; }
        public DateTime? ExpiredTime { get; set; }
        public bool? IsExpired { get; set; }
        public InvitationType InvitationType { get; set; }
    }

    public class InviteResult
    {
        public InviteResult(bool success, InviteModel model)
        {
            Success = success;
            Errors = new List<string>();
            Invite = model;
        }

        public List<string> Errors { get; set; }

        public bool Success { get; }
        public InviteModel Invite { get; }
    }
}
