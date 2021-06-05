using System.Collections.Generic;

namespace PlatformScience.Service.Specs.Models
{
    public class RequestModel
    {
        public int[] RoomSize { get; set; }
        public int[] Coords { get; set; }
        public List<int[]> Patches { get; set; }
        public string Instructions { get; set; }

        public RequestModel()
        {
            RoomSize = new int[2];
            Coords = new int[2];
            Patches = new List<int[]>();
        }
    }
}
