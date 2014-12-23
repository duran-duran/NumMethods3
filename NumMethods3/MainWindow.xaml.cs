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
using System.Windows.Forms.DataVisualization.Charting;

namespace NumMethods3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ResetChartBtn_Click(object sender, RoutedEventArgs e)
        {
            ResultChart.Series.Clear();
        }

        private void WFHost_Loaded(object sender, RoutedEventArgs e)
        {
            ResultChart.ChartAreas.Add(new ChartArea("Result"));
            ResultChart.ChartAreas["Result"].AxisX.Title = "X";
            ResultChart.ChartAreas["Result"].AxisY.Title = "Y";
        }

        private void DrawOriginBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Берем входную стоку, содержащую запись исходной функции..
                string origin_input = OriginBox.Text;
                //.. и переводим ее в постфиксную нотацию
                PostfixNotation origin = new PostfixNotation(origin_input);

                //Получаем границы интервала и требуемый размер сетки
                double left_bound = Convert.ToDouble(LeftBoundBox.Text);
                double right_bound = Convert.ToDouble(RightBoundBox.Text);
                int points_count = Convert.ToInt32(PointsCountBox.Text);

                //Получаем разбиение точек и сетку значений функции
                double[] x = FunctionsHelper.GetPartition(left_bound, right_bound, points_count);
                double[] y = FunctionsHelper.CalculateValues(origin, x);

                //Рисуем функцию
                ChartHelper.Draw(ResultChart, x, y);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void SolveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Берем входную стоку, содержащую запись правой части уравнения...
                string func_input = FunctionBox.Text;
                //.. и переводим ее в постфиксную нотацию
                PostfixNotation func = new PostfixNotation(func_input);

                //Получаем границы интервала и требуемый размер сетки
                double left_bound = Convert.ToDouble(LeftBoundBox.Text);
                double right_bound = Convert.ToDouble(RightBoundBox.Text);
                int points_count = Convert.ToInt32(PointsCountBox.Text);

                //Получаем сетку иксов и значение шага разбиения
                double[] x = FunctionsHelper.GetPartition(left_bound, right_bound, points_count);
                double norm = (right_bound - left_bound) / (points_count - 1);

                //Получаем начальное значение
                double y0 = Convert.ToDouble(InitValBox.Text);

                //Получаем требуемый порядок метода
                int rank = (int)MethodRankSlider.Value;

                //Вычисляем сетку значений искомой функции
                double[] y = PredCorScheme.Solve(func, x, y0, norm, rank);

                //Отображаем график в интерфейсе, если он был скрыт
                if (ChartContainer.Visibility == System.Windows.Visibility.Collapsed)
                    ChartContainer.Visibility = System.Windows.Visibility.Visible;

                //Рисуем кривую на графике
                ChartHelper.Draw(ResultChart, x, y);
            
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
