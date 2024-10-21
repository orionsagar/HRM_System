using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domains.Models
{
    public class Invitation
    {
        [Key]
        public int InvitationID { get; set; }
        public string Email { get; set; }
        public bool HasExpiry { get; set; }
        public DateTime? ExpireAt { get; set; }
        public bool? IsExpired { get; set; }
        public InvitationType InvitationType { get; set; }
        public string Hash { get; set; }
        public string InviteLink { get; set; }
        public string InviteFrom { get; set; }
        public UserSpace UserSpace { get; set; }
        public int? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual UserInfo UserInfo { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum InvitationType : short
    {
        Account = 1, Password
    }

    public enum UserSpace : short
    {
        Platform = 1, Client, Organisation
    }
}
