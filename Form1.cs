using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{

    public partial class Form1 : Form
    {
        public string[] data;      // точки x y
        public double[] pointsX;   // значения X
        public double[] pointsY;   // значения Y

        public Form1()
        {
            InitializeComponent();
        }

        // проверка данных
        private void buttonCheckData_Click(object sender, EventArgs e)
        {
            parseData();
        }

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

        // сплайн
        private void splineButton_Click(object sender, EventArgs e)
        {
            plot.Series[1].Name = "Сплайн";
            plot.Series[1].Points.Clear();
            plot.Series[1].Color = Color.Green;
            CubicSpline cs = new CubicSpline();

            cs.BuildSpline(pointsX, pointsY, pointsY.Length);
            int h = (int)(pointsX[1] - pointsX[0]);

            for (double dx = pointsX[0]; dx < pointsX[pointsX.Length - 1]; dx += 0.1)
            {
                plot.Series[1].Points.AddXY(dx, cs.Interpolate(dx));
            }
        }

        // провизводные
        CubicSpline cs = new CubicSpline();
        double dx = 0.001;                    // приращение аргумента
        double d2x = 0.002;                   



        // 1 производная
        public void buttondx1_Click(object sender, EventArgs e)
        {
            plot.Series[1].Name = "1 производная";
            plot.Series[1].Points.Clear();
            plot.Series[1].Color = Color.Red;
           
            cs.BuildSpline(pointsX, pointsY, pointsY.Length);

            for (double i = pointsX[0]; i < pointsX[pointsX.Length - 1]; i += dx)
            {
                plot.Series[1].Points.AddXY(i, dy(i));
            }

            cs.resetParams();
        }

        public double dy(double i)
        {
            return (cs.Interpolate(i + dx) - cs.Interpolate(i)) / dx;
        }

        // 2 производная
        private void buttondx2_Click(object sender, EventArgs e)
        {
            plot.Series[1].Name = "2 производная";
            plot.Series[1].Points.Clear();
            plot.Series[1].Color = Color.Purple;

            cs.BuildSpline(pointsX, pointsY, pointsY.Length);

            for (double i = pointsX[0]; i < pointsX[pointsX.Length - 1]; i += dx)
            {
                plot.Series[1].Points.AddXY(i, d2y(i));
            }

            cs.resetParams();
        }

        public double d2y(double i)
        {
            return (cs.Interpolate(i + 2 *d2x) - 2 * cs.Interpolate(i) + cs.Interpolate(i - 2 * d2x)) / (d2x*d2x);
            //return (cs.Interpolate(i + 2*_delt) - 2 * cs.Interpolate(i) + cs.Interpolate(i - 2*_delt)) / (_delt * _delt);
        }

    }


    class CubicSpline
    {
        SplineTuple[] splines; // Сплайн

        int indexOffset = 0;
        double h;

        private struct SplineTuple
        {
            public double a, b, c, d, x;    // коэф.
        }

        public void resetParams()
        {
            indexOffset = 0;
        }

        unsafe static bool _Solve_square_system(double* result, double* mtx, int n)
        {
            double* mtx_u_ii, mtx_ii_j;
            double a;
            double* mtx_end = mtx + n * n, result_i, mtx_u_ii_j = null;
            double mx;
            int d = 0;
            // mtx_ii - указатель на 'ведущий' диагональный элемент матрицы
            for (double* mtx_ii = mtx, mtx_ii_end = mtx + n; mtx_ii < mtx_end; result++, mtx_ii += n + 1, mtx_ii_end += n, d++)
            {
                // Смотрим под ведущим элеменом, значение максимальное по модулю
                {
                    mx = System.Math.Abs(*(mtx_ii_j = mtx_ii));
                    for (mtx_u_ii = mtx_ii + n, result_i = result + 1; mtx_u_ii < mtx_end; mtx_u_ii += n, result_i++)
                    {
                        if (mx < System.Math.Abs(*mtx_u_ii))
                        {
                            mx = System.Math.Abs(*(mtx_ii_j = mtx_u_ii));
                            mtx_u_ii_j = result_i;
                        }
                    }
                    // если максимальный по модулю элемент равен 0 - система вырождена и не имеет решения
                    if (mx == 0) return false;
                    // если максимальный элемент не является ведущим, делаем перестановку строк, чтобы он стал ведущим
                    else if (mtx_ii_j != mtx_ii)
                    {
                        a = *result;
                        *result = *mtx_u_ii_j;
                        *mtx_u_ii_j = a;
                        for (mtx_u_ii = mtx_ii; mtx_u_ii < mtx_ii_end; mtx_ii_j++, mtx_u_ii++)
                        {
                            a = *mtx_u_ii; *mtx_u_ii = *mtx_ii_j; *mtx_ii_j = a;
                        }
                    }
                }
                //'обнуляем' элементы над ведущим
                for (mtx_u_ii = mtx_ii - n, result_i = result - 1; mtx_u_ii > mtx; mtx_u_ii -= n)
                {
                    a = *(mtx_u_ii) / *mtx_ii;
                    for (mtx_ii_j = mtx_ii + 1, mtx_u_ii_j = mtx_u_ii + 1; mtx_ii_j < mtx_ii_end; *(mtx_u_ii_j++) -= *(mtx_ii_j++) * a) ;
                    *(result_i--) -= *(result) * a;
                }
                //'обнуляем' элементы под ведущим
                for (mtx_u_ii = mtx_ii + n, result_i = result + 1; mtx_u_ii < mtx_end; mtx_u_ii += d)
                {
                    a = *(mtx_u_ii++) / *mtx_ii;
                    for (mtx_ii_j = mtx_ii + 1; mtx_ii_j < mtx_ii_end; *(mtx_u_ii++) -= *(mtx_ii_j++) * a) ;
                    *(result_i++) -= *(result) * a;
                }
            }
            //матрица приведена к дигональному виду
            //приводим ее к единичному, получая права решение
            for (; mtx_end > mtx; *(--result) /= *(--mtx_end), mtx_end -= n) ;
            return true;
        }
        unsafe public static bool SolveSquare(double[] x, double[,] A, double[] b)
        {
            int n = x.Length;
            if (n == b.Length && n == A.GetLength(0) && n == A.GetLength(1))
            {
                if (n > 0)
                {
                    Array.Copy(b, x, n);
                    fixed (double* presult = &x[0])
                    fixed (double* pmtx = &A[0, 0])
                        return _Solve_square_system(presult, pmtx, n);
                }
                else return true;
            }
            throw new IndexOutOfRangeException();
        }


        public void BuildSpline(double[] x, double[] y, int n)
        {
            splines = new SplineTuple[n];
            for (int i = 0; i < n; ++i)
            {
                splines[i].x = x[i];
                splines[i].a = y[i];
            }
            splines[0].c = splines[n - 1].c = 0.0;

            h = x[1] - x[0];

            double[,] A = new double[3, 3]
            {
                {4*h , h, 0 },
                {h, 4*h, h },
                {0, h, 4*h }
            };

            double[] b = new double[3]
            {
                3 / h * (y[2] - 2 * y[1] + y[0]),
                3 / h * (y[3] - 2 * y[2] + y[1]),
                3 / h * (y[4] - 2 * y[3] + y[2])
            };

            double[] koeffC = new double[3];


            SolveSquare(koeffC, A, b);

            for (int i = n - 2; i > 0; --i)
            {
                splines[i].c = koeffC[i - 1];
            }

            for (int i = 0; i < n - 1; i++)
            {
                splines[i].d = (splines[i + 1].c - splines[i].c) / (3 * h);
                splines[i].b = (y[i + 1] - y[i]) / h - h * (splines[i + 1].c + 2 * splines[i].c) / 3.0;
            }
        }

        // Вычисление значения интерполированной функции в произвольной точке
        public double Interpolate(double x)
        {
            if (x > splines[0].x + (indexOffset + 1) * h)
            {
                indexOffset++;
            }

            double dx = x - splines[indexOffset].x;
            return splines[indexOffset].a + splines[indexOffset].b * dx + splines[indexOffset].c * dx * dx + splines[indexOffset].d * dx * dx * dx;
        }
    }

}
