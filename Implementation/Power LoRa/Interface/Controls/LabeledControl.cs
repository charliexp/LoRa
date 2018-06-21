﻿using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoRa_Controller.Interface.Controls
{
	public abstract class LabeledControl : BaseControl
    {
        #region Types
        public delegate Task ValueChangedDelegate(byte index);
        #endregion
        
        #region Properties
        public ValueChangedDelegate ValueChanged { get; set; }
        public Label Label { get; protected set; }
        public new bool Visible
        {
            get
            {
                return Field.Visible;
            }
            set
            {
                Label.Visible = value;
                Field.Visible = value;
            }
        }
        #endregion

        #region Constructors
        public LabeledControl(string name) : base(name)
		{
            Label = new Label
            {
                Name = name.Replace(" ", "") + "Label",
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleLeft,
			};

			string[] words = Regex.Matches(name.ToString(), "(^[a-z]+|[A-Z]+(?![a-z])|[A-Z][a-z]+)")
											.OfType<Match>()
											.Select(m => m.Value)
											.ToArray();
			Label.Text = string.Join(" ", words);
        }
        #endregion
    }
}