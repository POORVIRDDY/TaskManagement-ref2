using System;
using System.Collections.Generic;

namespace TaskManagement.Models;

public partial class GnrlSetting
{
    public Guid SettingId { get; set; }

    public int? TaskCounter { get; set; }

    public int? UserStoryCounter { get; set; }

    public string? TaskPrefix { get; set; }

    public string? UserStoryPrefix { get; set; }
}
