using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace NumMethods3
{
    static class ChartHelper
    {
        public static void Draw(Chart chart, double[] X_values, double[] Y_values)
        {
            Series series = new Series();    //Написал на всякий отдельный класс ChartHelper. Хз, надо ли вообще.
            series.ChartType = SeriesChartType.Line;
            series.Points.DataBindXY(X_values, Y_values);
            chart.Series.Add(series);
        }
    }
}
