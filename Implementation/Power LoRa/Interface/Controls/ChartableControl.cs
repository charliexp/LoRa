﻿using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Power_LoRa.Interface.Controls
{
    public class ChartableControl
    {
        #region Private variables
        private Series series;
        //TODO: make private
        public ChartControl chart;
        #endregion

        #region Properties
        public DataPoint LastValue
        {
            set
            {
                Text.Value = value.YValues[0].ToString();
                if (series.Points.Count == chart.MaxPoints)
                    series.Points.RemoveAt(0);
                series.Points.Add(value);
                chart.ResetAutoValues();
            }
        }
        public TextBoxControl Text
        {
            get;
        }
        #endregion

        #region Constructors
        public ChartableControl(Control container, ChartControl chart, Series series, string name, string measureUnit)
        {
            this.chart = chart;
            this.series = series;
            Text = new TextBoxControl(container, name, measureUnit, TextBoxControl.Type.Output);
        }
        #endregion
    }
}
