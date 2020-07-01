using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntSimulation
{
    class Food : GameObject
    {
        public static void SpawnOn(World world, PointF center, float amount = 100)//Se crea la comida
        {
            float radius = (float)Math.Round(Math.Sqrt(amount) / 2);
            for (float x = center.X - radius; x <= center.X + radius; x++)
            {
                for (float y = center.Y - radius; y <= center.Y + radius; y++)
                {
                    Food f = new Food();
                    int pointX = (int)Math.Round(x);//redondeo
                    int pointY = (int)Math.Round(y);
                    f.Position = new PointF(pointX, pointY);
                    world.AddFood(f,pointX,pointY);
                }
            }
        }

        public Food()
        {
            Color = Color.Green;
        }
    }
}
