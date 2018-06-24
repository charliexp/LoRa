using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace Power_LoRa.Interface.Controls
{
    public class ChartControl : Chart
    {
        #region Private variables
        private Title title;
        #endregion

        #region Properties
        public ChartArea ChartArea { get; }
        public int MaxPoints
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public ChartControl(List<Series> seriesList, string name) : base()
        {
            Name = name;
            MaxPoints = 10;

            title = new Title(Name);
            ChartArea = new ChartArea("ChartArea");
            ChartArea.AxisX.Enabled = AxisEnabled.False;
            ChartArea.AxisY.Enabled = AxisEnabled.False;
            ChartArea.AxisY2.Enabled = AxisEnabled.False;
            ChartArea.AxisX.LabelStyle.Enabled = false;
            ChartArea.AxisY.LabelStyle.Enabled = false;
            ChartArea.AxisY2.LabelStyle.Enabled = false;
            ChartArea.Position.X = 0;
            ChartArea.Position.Y = 40;
            ChartArea.Position.Height = 60;
            ChartArea.Position.Width = 100;
            //chartArea.AxisY2.Title = "kVAR";
            //chartArea.AxisY.Title = "W";
            //chartArea.AxisY.LabelStyle.IsEndLabelVisible = true;

            Legends.Add(new Legend("LeftLegend")
            {
                Enabled = false,
                LegendStyle = LegendStyle.Column,
            });
            Legends.Add(new Legend("RightLegend")
            {
                Enabled = false,
                LegendStyle = LegendStyle.Column,
            });
            ChartAreas.Add(ChartArea);
            Titles.Add(title);
            if (seriesList != null)
                foreach (Series series in seriesList)
                {
                    Series.Add(series);
                }
        }
        #endregion
    }
}
