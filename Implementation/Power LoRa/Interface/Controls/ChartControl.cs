using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Power_LoRa.Interface.Controls
{
    public class ChartControl : Chart
    {
        #region Private constants
        private const int minimizedMaxPoints = 3;
        private const int maxizedMaxPoints = 100;
        #endregion

        #region Private variables
        private ChartArea chartArea;
        private Legend legend;
        private Title title;
        #endregion

        #region Properties
        public Series ActiveEnergy { get; private set; }
        public Series ReactiveEnergy { get; private set; }
        #endregion

        #region Constructors
        public ChartControl(string name) : base()
        {
            Name = name;

            chartArea = new ChartArea();
            legend = new Legend();
            title = new Title(Name);
            ActiveEnergy = new Series
            {
                Color = Color.Red,
                IsVisibleInLegend = false,
                IsValueShownAsLabel = false,
                ChartType = SeriesChartType.Line,
                XValueType = ChartValueType.DateTime,
            };
            ReactiveEnergy = new Series
            {
                Color = Color.Yellow,
                IsVisibleInLegend = false,
                IsValueShownAsLabel = false,
                ChartType = SeriesChartType.Line,
                XValueType = ChartValueType.DateTime,
            };

            chartArea.AxisX.LabelStyle.Format = "HH:mm:ss";
            //chartArea.AxisY.Title = "W";
            //chartArea.AxisY.LabelStyle.IsEndLabelVisible = true;

            ChartAreas.Add(chartArea);
            Titles.Add(title);
            //Legends.Add(legend);
            Series.Add(ActiveEnergy);
            Series.Add(ReactiveEnergy);
        }
        #endregion

        #region Public methods
        public void AddPoint (Series series, DateTime timestamp, int y)
        {
            if (series.Points.Count == minimizedMaxPoints)
                series.Points.RemoveAt(0);
            Refresh();
            series.Points.AddXY(timestamp, y);
        }
        #endregion
    }
}
