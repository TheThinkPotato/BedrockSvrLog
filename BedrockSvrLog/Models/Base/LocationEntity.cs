using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedrockSvrLog.Models.Base;
public abstract class LocationEntity
{
    public int? PositionX { get; set; }
    public int? PositionY { get; set; }
    public int? PositionZ { get; set; }
    public string? Dimension { get; set; } = string.Empty;
}