using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AntSimulation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private const int scale = 4;
        private World world = new World();

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeWorld();
            SetStyle(ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.UserPaint
                | ControlStyles.AllPaintingInWmPaint,
                true);
        }

        //UpdateTimer cada 1 mls
        //Food Spawner cada 30,000 mls

        private void InitializeWorld()
        {
            Nest nest = new Nest();
            nest.Position = world.Center;
            world.Add(nest);

            for (int i = 0; i < 75; i++)//Creo las 75 Hormigas con su position y rotation random 
            {
                Ant ant = new Ant(nest);
                ant.Rotation = world.Random() * Math.PI * 2;
                ant.Position = nest.Position;
                world.Add(ant);
            }

   
            SpawnSomeFood();
        }

        private void SpawnSomeFood() //Spawnea Comida 3 veces o en 3 puntos random del mundo
        {
            for (int j = 0; j < 3; j++)
            {
                PointF random;
                do
                {
                    random = world.RandomPoint();
                }
                while (world.Dist(world.Center, random) < 50);
                Food.SpawnOn(world, random, world.Random(10, 50));
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e) //se dibuja el mundo
        {
            e.Graphics.ScaleTransform(scale, scale);
            world.DrawOn(e.Graphics);
        }

        private void updateTimer_Tick(object sender, EventArgs e)//Se actualiza el Jugeo cada 1 mls y manda a refrescar
        {
            Text = world.GameObjects.Count().ToString();
            ClientSize = new Size(world.Width * scale, world.Height * scale);
            world.Update();
            Refresh();
        }

        private void foodSpawner_Tick(object sender, EventArgs e) //Se crea la comida cada 30,000 mls
        {
            SpawnSomeFood();
        }
    }
}
