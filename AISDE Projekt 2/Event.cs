using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISDE_Projekt_2
{
    public class Event
    {
        public string Type { get; set; }

        public int Time { get; set; }
        public int EndTime { get; set; }
        public Chunk Chunk { get; set; }

        public Event(string type, int time)
        {
            Type = type;
            EndTime = time;
        }

        public Event(string type, Chunk chunk, float speed, int time)
        {
            Time = time;
            Chunk = chunk;
            EndTime = (int) (chunk.Size / speed + time);
            Type = type;
        }
    }
}
