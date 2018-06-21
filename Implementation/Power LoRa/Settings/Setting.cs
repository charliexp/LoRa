namespace Power_LoRa.Settings
{
	public class Setting
    {
        #region Private variables
        private object value;
        #endregion

        #region Properties
        public string Name { get; private set; }
        public object Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
                SettingHandler.Save(this);
            }
        }
        #endregion

        #region Constructors
        public Setting(string name)
		{
			Name = name;
        }
        #endregion
    }
}
