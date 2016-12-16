using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
namespace nanoelegui
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        Program prog = new Program();
        const int sqsize = 5;
        int[,] array;
        DispatcherTimer dispatcherTimer;
        public MainWindow()
        {

            InitializeComponent();
            array = prog.Init();
            for (int i = 0; i < sqsize; i++)
            {
                for (int j = 0; j < sqsize; j++)
                {
                    SpinLine[i, j] = new Line();
                    my_canvas.Children.Add(SpinLine[i, j]);
                }
            }
            dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);

            Draw(array);
            //for (int i = 0; i < 1; i++)
            //{
            //    array=prog.run();
            //    Draw(array);
            //}
        }
        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            array = prog.run();
            Draw(array);
        }

        private void my_canvas_Initialized(object sender, EventArgs e)
        {
            my_canvas.Background = new SolidColorBrush(Colors.White);
        }

        void Draw(int[,] array)
        {

            for (int i = 0; i < sqsize; i++)
            {
                for (int j = 0; j < sqsize; j++)
                {

                    spin(i, j, array[i, j]);
                }
            }

        }
        const int length = 60;

        double lotx1, lotx2, loty1, loty2;

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Stop();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.Start();
            array = prog.run();
            Draw(array);
        }

        Line[,] SpinLine = new Line[sqsize, sqsize];
        void spin(int x, int y, int deg)
        {
            lotx1 = -length / 2 * Math.Sin(Math.PI / 180 * deg) + length * x + 30;
            lotx2 = length / 2 * Math.Sin(Math.PI / 180 * deg) + length * x + 30;
            loty1 = length / 2 * Math.Cos(Math.PI / 180 * deg) + length * y + 30;
            loty2 = -length / 2 * Math.Cos(Math.PI / 180 * deg) + length * y + 30;
            int deg_ = deg / 40;
            switch (deg_)
            {
                case 0:
                    SpinLine[x, y].Stroke = Brushes.AliceBlue;
                    break;

                case 1:
                    SpinLine[x, y].Stroke = Brushes.Red;
                    break;

                case 2:
                    SpinLine[x, y].Stroke = Brushes.BlanchedAlmond;
                    break;
                case 3:
                    SpinLine[x, y].Stroke = Brushes.CornflowerBlue;
                    break;
                case 4:
                    SpinLine[x, y].Stroke = Brushes.DarkSlateBlue;
                    break;
                case 5:
                    SpinLine[x, y].Stroke = Brushes.Fuchsia;
                    break;
                case 6:
                    SpinLine[x, y].Stroke = Brushes.Gainsboro;
                    break;
                case 7:
                    SpinLine[x, y].Stroke = Brushes.IndianRed;
                    break;
                case 8:
                    SpinLine[x, y].Stroke = Brushes.Khaki;
                    break;
                case 9:
                    SpinLine[x, y].Stroke = Brushes.LawnGreen;
                    break;


            }
            //if (deg < 180)
            //{
            //    SpinLine[x, y].Stroke = Brushes.Red;
            //}
            //else
            //{
            //    SpinLine[x, y].Stroke = Brushes.Black;
            //}
            SpinLine[x, y].X1 = lotx1;
            SpinLine[x, y].X2 = lotx2;
            SpinLine[x, y].Y1 = loty1;
            SpinLine[x, y].Y2 = loty2;
            SpinLine[x, y].HorizontalAlignment = HorizontalAlignment.Left;
            SpinLine[x, y].VerticalAlignment = VerticalAlignment.Center;
            SpinLine[x, y].StrokeThickness = 4;
        }



    }
    class Program
    {

        const int sqsize = 5;
        const int dev = 360;

        const int loop = 1000;
        const int view = loop / 10;
        int[,] array;
        int[,] initarray;
        public int[,] Init()
        {
            int ram = 0;
            int seed = Environment.TickCount;
            Random rand = new Random(seed);
            array = new int[sqsize, sqsize];
            initarray = new int[sqsize, sqsize];

            for (int i = 0; i < sqsize; i++)
            {
                for (int j = 0; j < sqsize; j++)
                {
                    ram = (360 / dev) * rand.Next(dev);
                    initarray[i, j] = ram;
                    array[i, j] = ram;
                }
            }
            return array;
        }
        public int[,] run()
        {
            int seed = Environment.TickCount;
            Random rand = new Random(seed);
            int x;
            int y;
            int old = 0;
            int[] dx = { 1, 0, -1, 0 };//四方位
            int[] dy = { 0, 1, 0, -1 };
            //int[] dx = { 1, 1, 1, 0, -1, -1, -1, 0 };//八方位
            //int[] dy = { 1, 0, -1, -1, -1, 0, 1, 1 };
            //八方位でやっても収束する気がしない

            int tempx, tempy;

            double en_old = 0.0;
            double en_new = 0.0;
            int counter = 0;
            Console.WriteLine("Init state");
            printout(array);
            for (int i = 0; i < loop; i++)
            {
                //System.Threading.Thread.Sleep(1);
                seed = Environment.TickCount + 1;
                //Random rand2 = new Random(seed++);
                x = rand.Next(sqsize);
                y = rand.Next(sqsize);
                old = array[x, y];
                array[x, y] = (360 / dev) * rand.Next(dev);

                en_old = 0.0;
                en_new = 0.0;


                //if (0 <= x && x < sqsize - 1)
                //{
                //    en_old = en_old + Math.Cos(Math.PI * ((double)(array[x, y] - array[x + 1, y])) / 180.0);
                //    en_new = en_new + Math.Cos(Math.PI * ((double)(old - array[x + 1, y])) / 180.0);

                //}
                //if (0 < x && x <= sqsize - 1)
                //{
                //    en_old = en_old + Math.Cos(Math.PI * ((double)(array[x, y] - array[x - 1, y])) / 180.0);
                //    en_new = en_new + Math.Cos(Math.PI * ((double)(old - array[x - 1, y])) / 180.0);
                //}
                //if (0 < y && y <= sqsize - 1)
                //{
                //    en_old = en_old + Math.Cos(Math.PI * ((double)(array[x, y] - array[x, y - 1])) / 180.0);
                //    en_new = en_new + Math.Cos(Math.PI * ((double)(old - array[x, y - 1])) / 180.0);
                //}
                //if (0 <= y && y < sqsize - 1)
                //{
                //    en_old = en_old + Math.Cos(Math.PI * ((double)(array[x, y] - array[x, y + 1])) / 180.0);
                //    en_new = en_new + Math.Cos(Math.PI * ((double)(old - array[x, y + 1])) / 180.0);
                //}
                for (int j = 0; j < dx.Length; j++)
                {
                    tempx = x + dx[j];
                    tempy = y + dy[j];
                    if (0 <= tempx && tempx <= sqsize - 1 && 0 <= tempy && tempy <= sqsize - 1)
                    {
                        en_old = en_old + Math.Cos(Math.PI * ((double)(array[x, y] - array[tempx, tempy])) / 180.0);
                        en_new = en_new + Math.Cos(Math.PI * ((double)(old - array[tempx, tempy])) / 180.0);

                    }
                }

                if (en_old <= en_new)
                {
                    array[x, y] = old;
                    counter++;//for debug
                }
                else
                {
                    break;
                }

                ////////////////
                //ノイズを混入させる
                //
                if (true && rand.Next(1000000) == 0)
                {
                    x = rand.Next(sqsize);
                    y = rand.Next(sqsize);
                    array[x, y] = (360 / dev) * rand.Next(dev);
                }
                //else
                //{
                //    Console.Clear();
                //    Console.WriteLine("This loop is {0} : ave {1},\t{2}%", i, average(array), i / (double)loop * 100);
                //    printout(array);
                //}
                if (i % view == 0)
                {
                    // Draw(array);
                    //Console.Clear();
                    Console.WriteLine("This loop is {0} : ave {1},\t{2}%", i, average(array), i / (double)loop * 100);
                    printout(array);
                }
            }
            return array;
            ////Console.Clear();
            //Console.WriteLine("Finish");
            //printout(array);
            //Console.WriteLine("Init array is");
            //printout(initarray);
            //Console.WriteLine("init ave \t{0}:\t{1}\nfinish ave\t{2}:\t{3}", average(initarray), Dispersion(initarray), average(array), Dispersion(array));
            //Console.WriteLine("Change num is {0}", counter);
            //Console.ReadLine();
        }
        private void printout(int[,] array)
        {
            for (int i = 0; i < sqsize; i++)
            {
                for (int j = 0; j < sqsize; j++)
                {
                    Console.Write("{0}\t", array[i, j]);
                }
                Console.WriteLine();
            }

        }
        private int average(int[,] array)
        {
            int ave = 0;
            for (int i = 0; i < sqsize; i++)
            {
                for (int j = 0; j < sqsize; j++)
                {
                    ave = ave + array[i, j];
                }
            }
            return ave / sqsize / sqsize;
        }
        static double Dispersion(int[,] array)
        {
            int ave = 0;
            int ave2 = 0;
            for (int i = 0; i < sqsize; i++)
            {
                for (int j = 0; j < sqsize; j++)
                {
                    ave = ave + array[i, j];
                    ave2 = ave2 + array[i, j] * array[i, j];
                }
            }
            ave = ave / sqsize / sqsize;
            double gosa = 0;
            for (int i = 0; i < sqsize; i++)
            {
                for (int j = 0; j < sqsize; j++)
                {
                    gosa = gosa + Math.Pow(ave - array[i, j], 2);
                }
            }
            return Math.Sqrt(gosa);
        }
    }
}
