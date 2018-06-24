using System;

namespace Power_LoRa.Node
{
    public class Compensator
    {
        #region Types
        public enum CompensatorType
        {
            Unused = 0x00,
            Inductor = 0x01,
            Capacitor = 0x02,
        }
        #endregion

        #region Public constants
        public const string MeasureUnit = "kVAR";
        #endregion

        #region Properties
        public CompensatorType Type { get; set; }
        public byte Position { get; set; }
        public Int32 Value  { get; set; }
        #endregion

        #region Constructors
        public Compensator(CompensatorType type, Int32 value, byte position)
        {
            Type = type;
            Value = value;
            Position = position;
        }
        #endregion
    }
}
