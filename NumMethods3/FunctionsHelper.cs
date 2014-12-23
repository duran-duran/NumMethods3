using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumMethods3
{
    static class FunctionsHelper
    {
        //Получение сетки значений функции по ее постфиксной нотации и сетке иксов
        public static double[] CalculateValues(PostfixNotation f, double[] x)
        {
            //Очевидно, размер сетки значений функции такой же, как у сетки иксов
            double[] y = new double[x.Length];

            //Для каждого x из сетки
            for (var i = 0; i < x.Length; i++)
                //Находим значение функции
                y[i] = f.Calculate(x[i]);

            return y;
        }

        //Получение разбиения (мн-ва точек) заданного интервала на заданное число точек
        public static double[] GetPartition(double left_bound, double right_bound, int points_count)
        {
            double[] result = new double[points_count];
            //Вычисляем шаг разбиения
            double norm = (right_bound - left_bound) / (points_count - 1);

            for (int i = 0; i < points_count; i++)
            {
                //Вычисляем значения точек
                result[i] = left_bound + i * norm;
            }

            return result;
        }
    }
}
