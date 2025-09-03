using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedrockSvrLog.Models.Base
{
    public abstract class PlayerDayTimeLocationSpawnEntity : LocationEntity
    {
        public int? GameTime { get; set; }
        public int GameDay { get; set; }
        public int? SpawnPositionX { get; set; }
        public int? SpawnPositionY { get; set; }
        public int? SpawnPositionZ { get; set; }
    }
}
