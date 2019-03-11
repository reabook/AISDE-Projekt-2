using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISDE_Projekt_2
{
    public class Chunk
    {
        public int Length { get; set; }
        public float Size { get; set; }

        public Chunk(int length, float size) {
            Length = length;
            Size = size;
        }
    }
}
