using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class CubicSpline
    {
        SplineTuple[] splines; // Сплайн
        // Структура, описывающая сплайн на каждом сегменте сетки
        private struct SplineTuple
        {
            public double a, b, c, d, x;
        }

        // Построение сплайна
        // x - узлы сетки, должны быть упорядочены по возрастанию, кратные узлы запрещены
        // y - значения функции в узлах сетки
        // n - количество узлов сетки
        public void BuildSpline(double[] x, double[] y, int n)
        {
            // Инициализация массива сплайнов
            splines = new SplineTuple[n];
            for (int i = 0; i < n; ++i)
            {
                splines[i].x = x[i];
                splines[i].a = y[i];
            }
            splines[0].c = splines[n - 1].c = 0.0;

            // Решение СЛАУ относительно коэффициентов сплайнов c[i] методом прогонки для трехдиагональных матриц
            // Вычисление прогоночных коэффициентов - прямой ход метода прогонки
            double[] alpha = new double[n - 1];
            double[] beta = new double[n - 1];
            alpha[0] = beta[0] = 0.0;
            for (int i = 1; i < n - 1; ++i)
            {
                double hi = x[i] - x[i - 1];
                double hi1 = x[i + 1] - x[i];
                double A = hi;
                double C = 2.0 * (hi + hi1);
                double B = hi1;
                double F = 6.0 * ((y[i + 1] - y[i]) / hi1 - (y[i] - y[i - 1]) / hi);
                double z = (A * alpha[i - 1] + C);
                alpha[i] = -B / z;
                beta[i] = (F - A * beta[i - 1]) / z;
            }

            // Нахождение решения - обратный ход метода прогонки
            for (int i = n - 2; i > 0; --i)
            {
                splines[i].c = alpha[i] * splines[i + 1].c + beta[i];
            }

            // По известным коэффициентам c[i] находим значения b[i] и d[i]
            for (int i = n - 1; i > 0; --i)
            {
                double hi = x[i] - x[i - 1];
                splines[i].d = (splines[i].c - splines[i - 1].c) / hi;
                splines[i].b = hi * (2.0 * splines[i].c + splines[i - 1].c) / 6.0 + (y[i] - y[i - 1]) / hi;
            }
        }

        // Вычисление значения интерполированной функции в произвольной точке
        public double Interpolate(double x)
        {
            if (splines == null)
            {
                return double.NaN; // Если сплайны ещё не построены - возвращаем NaN
            }

            int n = splines.Length;
            SplineTuple s;

            if (x <= splines[0].x) // Если x меньше точки сетки x[0] - пользуемся первым эл-тов массива
            {
                s = splines[0];
            }
            else if (x >= splines[n - 1].x) // Если x больше точки сетки x[n - 1] - пользуемся последним эл-том массива
            {
                s = splines[n - 1];
            }
            else // Иначе x лежит между граничными точками сетки - производим бинарный поиск нужного эл-та массива
            {
                int i = 0;
                int j = n - 1;
                while (i + 1 < j)
                {
                    int k = i + (j - i) / 2;
                    if (x <= splines[k].x)
                    {
                        j = k;
                    }
                    else
                    {
                        i = k;
                    }
                }
                s = splines[j];
            }

            double dx = x - s.x;
            // Вычисляем значение сплайна в заданной точке по схеме Горнера (в принципе, "умный" компилятор применил бы схему Горнера сам, но ведь не все так умны, как кажутся)
            return s.a + (s.b + (s.c / 2.0 + s.d * dx / 6.0) * dx) * dx;
        }
    }
}

public partial class Form1 : Form
    {
        private string[] data;
        private double[] pointsX;
        private double[] pointsY;

        private void parseData()
        {
            data = Input.Lines;
            if (data.Length != 0)
            {
                pointsX = new double[data.Length];
                pointsY = new double[data.Length];
                plot.Series[0].Points.Clear();
                groupBox2.Enabled = false;

                for (int i = 0; i < data.Length; i++)
                {
                    string[] values = data[i].Split(' ');
                    try
                    {
                        pointsX[i] = Convert.ToDouble(values[0]);
                        pointsY[i] = Convert.ToDouble(values[1]);
                    }
                    catch (FormatException fe)
                    {
                        MessageBox.Show("Ошибка при считывании данных. Неверный формат ввода в строке " + (i + 1), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    plot.Series[0].Points.AddXY(pointsX[i], pointsY[i]);
                }

                groupBox2.Enabled = true;
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void run_Click(object sender, EventArgs e)
        {
            //TODO: N - input
            plot.Series[1].Points.Clear();
            plot.Series[1].Color = Color.Green;
            int n = pointsX.Length;
            double[] Lx = new double[n];
            for (int i = 0; i < n; i++)
            {
                double K = pointsY[i];
                for (int xj = 0; xj < n; xj++)
                    if (xj != i)
                        K /= (pointsX[i] - pointsX[xj]);

                int cLength = 2;
                int jStart = 1;
                double[] nV = new double[n + 1];
                nV[0] = 1;
                if (i != 0)
                    nV[1] = -pointsX[0];
                else
                {
                    nV[1] = -pointsX[1];
                    jStart = 2;
                }

                double[] aV = new double[n];
                double[] bV = new double[n];

                for (int j = jStart; j < pointsX.Length; j++)
                {
                    if (i != j)
                    {
                        aV = (double[])nV.Clone();
                        nV = new double[pointsX.Length + 1];
                        bV[0] = 1;
                        bV[1] = -pointsX[j];

                        for (int a = 0; a < cLength; a++)
                            for (int b = 0; b < 2; b++)
                                nV[a + b] += aV[a] * bV[b];

                        cLength++;
                    }
                }

                for (int a = 0; a < n; a++)
                    Lx[a] += K * nV[a];
            }

            for (double dx = pointsX[0] - 1; dx < pointsX[pointsX.Length - 1] + 1; dx += 0.1)
            {
                double curY = 0;
                for (int p = 0; p < Lx.Length; p++)
                    curY += Math.Pow(dx, Lx.Length - 1 - p) * Lx[p];
                plot.Series[1].Points.AddXY(dx, curY);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            parseData();
        }

        private void Input_TextChanged(object sender, EventArgs e)
        {

        }

        private void TypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void run2_Click(object sender, EventArgs e)
        {
            plot.Series[1].Points.Clear();
            plot.Series[1].Color = Color.Red;
            CubicSpline cs = new CubicSpline();
            cs.BuildSpline(pointsX, pointsY, pointsY.Length);

            for (double dx = pointsX[0]; dx < pointsX[pointsX.Length - 1]; dx += 0.1)
            {
                plot.Series[1].Points.AddXY(dx, cs.Interpolate(dx));
            }
        }
        public double derivative(double i)
        {
            return (cs.Interpolate(i + delt) - cs.Interpolate(i)) / delt;
        }

        CubicSpline cs = new CubicSpline();
        double delt = 0.001;


        public void run3_Click(object sender, EventArgs e)
        {
            plot.Series[1].Points.Clear();
            plot.Series[1].Color = Color.Blue;

          //  CubicSpline cs = new CubicSpline();
            cs.BuildSpline(pointsX, pointsY, pointsY.Length);
           

            for (double i = pointsX[0]; i < pointsX[pointsX.Length - 1]; i += delt)
            {
                plot.Series[1].Points.AddXY(i, derivative(i));
               //  plot.Series[1].Points.AddXY(i, ((cs.Interpolate(i) - cs.Interpolate(i-delt))/delt));
               // plot.Series[1].Points.AddXY(i, (cs.Interpolate(i + delt) - cs.Interpolate(i-delt)) /(2 * delt));

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            plot.Series[1].Points.Clear();
            plot.Series[1].Color = Color.Purple;

            cs.BuildSpline(pointsX, pointsY, pointsY.Length);
            double _delt = 0.002;

            for (double i = pointsX[0]+0.002; i < pointsX[pointsX.Length -1]; i += delt)
            {
                plot.Series[1].Points.AddXY(i, (cs.Interpolate(i+_delt)-2*cs.Interpolate(i)+cs.Interpolate(i-_delt))/(_delt*_delt));
               // plot.Series[1].Points.AddXY(i, derivative(derivative(i)));
            }
        }

    
        public double del(int i)
        {
            return pointsY[i - 2] - 4 * pointsY[i - 1] + 6 * pointsY[i] - 4 * pointsY[i + 1] + pointsY[i + 2]; 
        }


        private void button3_Click(object sender, EventArgs e)
        {
            double[] z = new double[pointsY.Length];
            
            for (int i = 0; i < pointsY.Length; i++)
            {
                if (i == 0)
                    z[i] = pointsY[0] - del(2)/70;
                if (i == 1)
                    z[i] = pointsY[1] + 2 * del(2) / 35;
                if (i == pointsY.Length - 2)
                    z[i] = pointsY[pointsY.Length-2] + 2 * del(pointsY.Length - 3) / 35;
                if (i == pointsY.Length - 1)
                    z[i] = pointsY[pointsY.Length - 1] - del(pointsY.Length - 3) / 70;
                if ((i > 1) && (i<pointsY.Length - 2))
                    z[i] = pointsY[i] - 3 * del(i) / 35;
            }

            for (int i = 0; i < z.Length; i++)
            {
                plot.Series[1].Points.AddXY(pointsX[i], z[i]);
                Console.WriteLine(pointsX[i] + "   " + z[i] + Environment.NewLine);
            }
            }
    }






