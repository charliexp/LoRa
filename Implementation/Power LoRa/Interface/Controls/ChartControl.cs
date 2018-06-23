using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Power_LoRa.Interface.Controls
{
    public class ChartControl : Chart
    {
        #region Private variables
        private ChartArea chartArea;
        private Title title;
        #endregion

        #region Properties
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

            chartArea = new ChartArea();
            chartArea.AxisX.Enabled = AxisEnabled.True;
            chartArea.AxisY.Enabled = AxisEnabled.True;
            chartArea.AxisY2.Enabled = AxisEnabled.True;
            title = new Title(Name);

            chartArea.AxisX.LabelStyle.Format = "HH:mm:ss";
            //chartArea.AxisY2.Title = "kVAR";
            //chartArea.AxisY.Title = "W";
            //chartArea.AxisY.LabelStyle.IsEndLabelVisible = true;

            ChartAreas.Add(chartArea);
            Titles.Add(title);
            if (seriesList != null)
                foreach (Series series in seriesList)
                    Series.Add(series);
        }
        #endregion
    }
}
