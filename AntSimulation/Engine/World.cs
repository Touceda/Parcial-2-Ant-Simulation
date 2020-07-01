using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace AntSimulation
{
    class World
    {
        private Random rnd = new Random();//Simple random
        private const int width = 125;//Tamaño del mundo
        private const int height = 125;//Tamañp del mundo
        private Size size = new Size(width, height);//Tamaño del mundo

        private List<GameObject> objects = new List<GameObject>();//La lista que contiene todos los objetos del mudno
        private List<Ant> Hormigas = new List<Ant>();
        //private List<Food> Comida = new List<Food>();
        private Food[,] Comida = new Food[360, 360];

        public IEnumerable<GameObject> GameObjects
        { get {
                List<GameObject> AllObject = new List<GameObject>();
                foreach (var obj in objects)
                {
                    AllObject.Add(obj);
                }
                foreach (var obj in Hormigas)
                {
                    AllObject.Add(obj);
                }
                foreach (var obj in Comida)
                {
                    if (obj != null)
                    {
                        AllObject.Add(obj);
                    }
                }
                return AllObject.ToArray(); } }

        public int Width { get { return width; } }//Propiedades que devuelven alto y ancho
        public int Height { get { return height; } }

        public PointF Center { get { return new PointF(width / 2, height / 2); } }//El centro de mi mundo

        public bool IsInside(PointF p)
        {
            return p.X >= 0 && p.X < width
                && p.Y >= 0 && p.Y < height;
        }
        
        public PointF RandomPoint()//Genera y devuelve un PointF Random
        {
            return new PointF(rnd.Next(width), rnd.Next(height));
        }

        public float Random()//Devuelve un Float Random
        {
            return (float)rnd.NextDouble();
        }

        public float Random(float min, float max) //Devuelve un float Random con un minimo y un maximo posible
        {
            return (float)rnd.NextDouble() * (max - min) + min;
        }

        public void Add(Ant obj)//Añada un objeto al mundo
        {
            Hormigas.Add(obj);
        }

        public void AddFood(Food obj,int x, int y)//Añada un objeto al mundo
        {
            Comida[x, y] = obj;
        }

        public void Add(GameObject obj)//Añado el resto de objetos
        {
            objects.Add(obj);
        }


        public void Remove(GameObject obj)//Elimino un objeto del mundo
        {
            objects.Remove(obj);
        }

        public void RemoveFood(int x, int y)//Elimino un objeto del mundo
        {
            Comida[x, y] = null;
        }

        public void Update()//Actualizo todos los objetos del mundo
        {
            foreach (GameObject obj in GameObjects)
            {
                obj.InternalUpdateOn(this);
                obj.Position = Mod(obj.Position, size);
            }
        }

        public void DrawOn(Graphics graphics) //Dibuja el mundo
        {
            graphics.FillRectangle(Brushes.White, 0, 0, width, height);
            //var all = GameObjects.Where(t => t != null);
            foreach (GameObject obj in GameObjects)
            {
                graphics.FillRectangle(new Pen(obj.Color).Brush, obj.Bounds);//(Puedo guardar los pens en un array, posible optimizacion futura)
            }
        }

        public double Dist(PointF a, PointF b)//me busca el punto del objeto
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        public double Dist(float x1, float y1, float x2, float y2)//me pusca el punto del objeto
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }

        // http://stackoverflow.com/a/10065670/4357302
        private static float Mod(float a, float n)
        {
            float result = a % n;
            if ((a < 0 && n > 0) || (a > 0 && n < 0))
                result += n;
            return result;
        }
        private static PointF Mod(PointF p, SizeF s)
        {
            return new PointF(Mod(p.X, s.Width), Mod(p.Y, s.Height));
        }

        public IEnumerable<GameObject> GameObjectsNear(int xpos, int ypos)
        {

            List<GameObject> ComidaARango = new List<GameObject>();
            int pointx = xpos;
            int pointy = ypos;

            //esto es por un error, "para no mirar el otro lado de la pared"
            if (xpos < 0)
            {
                pointx = 0;
            }

            if (ypos < 0)
            {
                pointy = 0;
            }

            if (Comida[pointx, pointy] != null)
            {
                ComidaARango.Add(Comida[pointx, pointy]);
            }

            var x = ComidaARango.Where(t => true);
            return x;

        }

        public IEnumerable<GameObject> GameObjectsNear(PointF pos, float dist = 1)
        {
            var Pheromonas = objects.Where(t => t is Pheromone);
            return Pheromonas.Where(t => Dist(t.Position, pos) < dist);

            //return objects.Where(t => Dist(t.Position, pos) < dist);
        }

    }

 


}




