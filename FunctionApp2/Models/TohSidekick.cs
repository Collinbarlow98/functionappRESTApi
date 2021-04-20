using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToHApi.Models
{
    public class TohSidekick
    {
        public string Id { get; set; } = Guid.NewGuid().ToString(format: "n");
        public string Name { get; set; }
        public int Likes { get; set; }
        public string Hero { get; set; }
    }
}
