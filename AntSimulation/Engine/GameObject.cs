using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace AntSimulation
{
    abstract class GameObject
    {
        private PointF position = new PointF(0,0);//Tengo una position
        private double rotation = 0;//Rotacion
        private Color color = Color.Black;//Color
        private long lastUpdate = 0;//Intervalo de Update = 10
        public string TipoDeObjeto;
        //Propiedades
        public virtual long UpdateInterval { get { return 10; } }

        public PointF Position
        {
            get { return position; }
            set { position = value; }
        }
        
        public double Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        
        public RectangleF Bounds//Devuelve el limite de mi objeto
        {
            get { return new RectangleF(Position, new Size(1, 1)); }
        }

        public virtual void InternalUpdateOn(World world)//Reviso si me puedo actualizar o no
        {
            long now = Environment.TickCount;
            if (now - lastUpdate > UpdateInterval)
            {
                lastUpdate = now;
                UpdateOn(world);
            }
        }

        public virtual void UpdateOn(World world) //Update GameObject Virtual
        {
            // Do nothing
        }

        public void Turn(float angle)//Gira mi objeto en 180 grados
        {
            Rotation += Math.PI * angle / 180.0;
        }

        public void Forward(float dist)//Camina al destino
        {
            //(direction degreeCos @ direction degreeSin) *distance + location
            Position = new PointF((int)Math.Round(Math.Cos(rotation) * dist + Position.X),
                                  (int)Math.Round(Math.Sin(rotation) * dist + Position.Y));
        }

        public void LookTo(PointF p)//Roto para mirar un objeto
        {
            Rotation = Math.Atan2(p.Y - Position.Y, p.X - Position.X);
        }
    }
}
