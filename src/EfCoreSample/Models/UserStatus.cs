using System;

namespace EfCoreSample.Models;

public class UserStatus
{
    public UserStatusState State { get; set; }
    public DateTime LastUpdated { get; set; }
    public string Description { get; set; }
}