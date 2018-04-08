﻿using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoRa_Controller.Interface.Controls
{
	public abstract class LabeledControl : BaseControl
    {
        #region Types
        protected delegate Task ValueChangedDelegate(int index);
        #endregion

        #region Protected variables
        protected ValueChangedDelegate valueChangedDelegate;
        #endregion

        #region Properties
        public Label label { get; protected set; }
        #endregion

        #region Constructors
        public LabeledControl(string name) : base(name)
		{
			label = new Label
			{
				AutoSize = true,
				Margin = new Padding(InterfaceConstants.ItemPadding, 0, InterfaceConstants.ItemPadding, 0),
				Name = name.Replace(" ", "") + "Label",
				Size = new System.Drawing.Size(InterfaceConstants.LabelWidth, InterfaceConstants.LabelHeight)
			};

			string[] words = Regex.Matches(name.ToString(), "(^[a-z]+|[A-Z]+(?![a-z])|[A-Z][a-z]+)")
											.OfType<Match>()
											.Select(m => m.Value)
											.ToArray();
			label.Text = string.Join(" ", words);
        }
        #endregion

        #region Public methods
        public override void Draw(int index)
		{
			label.Location = new System.Drawing.Point(InterfaceConstants.LabelLocationX,

													InterfaceConstants.GroupBoxFirstItemY +
													index * (InterfaceConstants.InputHeight + InterfaceConstants.ItemPadding) +
													InterfaceConstants.LabelToBoxOffset);

			Field.Location = new System.Drawing.Point(InterfaceConstants.LabelLocationX +
													InterfaceConstants.LabelWidth +
													InterfaceConstants.ItemPadding,

													InterfaceConstants.GroupBoxFirstItemY +
													index * (InterfaceConstants.InputHeight + InterfaceConstants.ItemPadding));

			Field.TabIndex = index;
        }
        #endregion
    }
}
