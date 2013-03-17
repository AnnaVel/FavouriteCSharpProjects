using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwarfGame
{
    public class Rock
    {
        char[] possibleShapes = { '^', '@', '*', '&', '+', '%', '$', '#', '!', '.', ';' };
        int positionX;
        int positionY;
        char shape;
        int size;

        public Rock(int positionX)
        {
            Random rockGenerator = new Random();
            this.positionX = positionX;
            this.positionY = 0;
            int possibleShapeIndex = rockGenerator.Next(0, possibleShapes.Length);
            this.shape = possibleShapes[possibleShapeIndex];
            this.size = rockGenerator.Next(1, 4);
        }

        public int Size
        {
            get { return this.size; }
        }

        public char Shape
        {
            get { return this.shape; }
        }

        public int PositionX
        {
            get { return this.positionX; }
            set { this.positionX = value; }
        }

        public int PositionY
        {
            get { return this.positionY; }
            set { this.positionY = value; }
        }
    }
}
