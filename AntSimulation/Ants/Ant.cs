using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace AntSimulation
{
    class Ant : GameObject
    {
        //Hormiga 
        private Nest nest;
        private bool hasFood = false;

        public Ant(Nest nest)
        {
            this.nest = nest;
        }

        public override void UpdateOn(World world)
        {
            if (hasFood)
            {
                Color = Color.Red;
                ReleasePheromone(world);
                MoveToNest(world);
            }
            else
            {
                Color = Color.Blue;
                Wander(world);
                if (!CheckFood(world))
                {
                    CheckPheromone(world);
                }
            }
        }

        private void Wander(World world)
        {
            Forward(world.Random(1, 5));
            Turn(world.Random(-25, 25));

        }


        private bool CheckFood(World world)
        {

            IEnumerable<GameObject> food = FindNear<Food>(world, 15);//Devuelve la comida que esta cerca //15 es el radio de busqueda


            if (food.Any())
            {
                GameObject f = food.Where(each => each.Position.Equals(Position)).FirstOrDefault();
                if (f != null)
                {
                    //Probar algunas cosas de la escala del mundo
                    hasFood = true;
                    int pointX = (int)Math.Round(f.Position.X);//redondeo
                    int pointY = (int)Math.Round(f.Position.Y);
                    world.RemoveFood(pointX, pointY);
                }
                else
                {
                    LookTo(food.First().Position);
                    Wander(world);
                }
                return true;
            }
            return false;
            //IEnumerable<GameObject> food = FindNear<Food>(world, 15);
            //if (food.Any())
            //{
            //    GameObject f = food.Where(each => each.Position.Equals(Position)).FirstOrDefault();
            //    if (f != null)
            //    {
            //        hasFood = true;
            //        world.Remove(f);
            //    }
            //    else
            //    {
            //        LookTo(food.First().Position);
            //        Wander(world);
            //    }
            //    return true;
            //}
            //return false;
        }

        private void CheckPheromone(World world)
        {
            PointF? strongestPoint = null;
            double strongestIntensity = 0;
            IEnumerable<Pheromone> nearPheromones = FindNear<Pheromone>(world, 10);
            foreach (Pheromone p in nearPheromones.Where(p => p.Intensity > 5))
            {
                if (p.Intensity > strongestIntensity)
                {
                    strongestIntensity = p.Intensity;
                    strongestPoint = p.Position;
                }
            }

            if (strongestPoint.HasValue)
            {
                LookTo(strongestPoint.Value);
                Wander(world);
            }
        }

        private void ReleasePheromone(World world)
        {
            Pheromone.SpawnOn(world, Position, world.Dist(Position, world.Center) * 1.5);
        }

        private void MoveToNest(World world)
        {
            LookTo(nest.Position);
            Wander(world);
            if (Position.Equals(nest.Position))
            {
                hasFood = false;
            }
        }


        private IEnumerable<T> FindNear<T>(World world, float radius) where T : GameObject
        {
            List<T> result = new List<T>();
            //List<Point> RangoDeHormiga = new List<Point>();
            for (float x = Position.X - radius; x <= Position.X + radius; x++)
            {
                for (float y = Position.Y - radius; y <= Position.Y + radius; y++)
                {
                    int pointX = (int)Math.Round(x);//redondeo
                    int pointY = (int)Math.Round(y);
                    result.AddRange(world
                      .GameObjectsNear(pointX, pointY)
                      .Select(t => t as T)
                      .Where(t => t != null));
                }
            }
            return result;
        }
    }
}

        //    private IEnumerable<Food> FindNear(World world, float radius) //Este metodo encuentra la comida que esta cerca
        //    {
        //        IEnumerable<Food> result;
        //        List<Point> RangoDeHormiga = new List<Point>();
        //        for (float x = Position.X - radius; x <= Position.X + radius; x++)
        //        {
        //            for (float y = Position.Y - radius; y <= Position.Y + radius; y++)
        //            {
        //                int pointX = (int)Math.Round(x);//redondeo
        //                int pointY = (int)Math.Round(y);
        //                RangoDeHormiga.Add(new Point(pointX, pointY));
        //            }
        //        }

        //        result = world.GameObjectsNear(RangoDeHormiga);
        //        //pudeo pasar un array de pointF con todos los puntos o el rango de mi hormiga, y a partir de ahi que me devuelva una lista con toda la comida posible que toque
        //        //La comida la puedo almacenar en un array segun su pos x e y, de esta manera puedo eliminar dist que causa el problema
        //        //Luego tendria que suplicar el metodo pero para las pheromonas y activarlas
        //        return result;
        //    }
        //}
    
