﻿using Power_LoRa.Interface.Nodes;
using System;
using System.Windows.Forms;
using static Power_LoRa.Connection.Messages.Message;

namespace Power_LoRa.Node
{
    public class BaseNode
    {
        #region Types
        //TODO remove
        public enum NodeType
        {
            Gateway = 0,
            EndDevice,
            Unknown,
        }
        public enum AddressType
        {
            General = 0,
            Master = 1,
            Beacon = 2,
            PC = 0xff,
        }
        #endregion

        #region Public constants
        public const int MinTransmissionRate = 5;
        public const int MaxTransmissionRate = 3600;
        #endregion

        #region Private variables
        private byte address;
        private Int16 transmissionRate;
        private DateTime timestamp;
        private Int32 activeEnergy;
        private Int32 reactiveEnergy;
        private Int32 activePower;
        private Int32 reactivePower;
        private Int32 apparentPower;
        private double powerFactor;
        #endregion

        #region Properties
        public BaseNodeGroupBox GroupBox { get; private set; }
        public byte Address
        {
            get { return address; }
            set
            {
                address = value;
                GroupBox.Address = value;
                if (address == (int)AddressType.Master)
                    Type = NodeType.Gateway;
                else if (address == (int)AddressType.General)
                    Type = NodeType.Unknown;
                else
                    Type = NodeType.EndDevice;
            }
        }
        public Int16 TransmissionRate
        {
            get
            {
                return transmissionRate;
            }
            set
            {
                transmissionRate = value;
                GroupBox.UpdateInterface(GroupBox.TransmissionRate, transmissionRate);
            }
        }
        public DateTime Timestamp
        {
            get
            {
                return timestamp;
            }
            set
            {
                timestamp = value;
                GroupBox.UpdateInterface(GroupBox.LastReadingTime, timestamp.ToString("HH:mm:ss"));
            }
        }
        public Int32 ActiveEnergy
        {
            get
            {
                return activeEnergy;
            }
            set
            {
                activeEnergy = value;

                GroupBox.UpdateInterface(GroupBox.ActiveEnergy, activeEnergy);
            }
        }
        public Int32 ReactiveEnergy
        {
            get
            {
                return reactiveEnergy;
            }
            set
            {
                reactiveEnergy = value;
                GroupBox.UpdateInterface(GroupBox.ReactiveEnergy, reactiveEnergy);
            }
        }
        public Int32 ActivePower
        {
            get
            {
                return activePower;
            }
            set
            {
                activePower = value;
                ApparentPower = (Int32) Math.Sqrt(Math.Pow(activePower, 2) * Math.Pow(reactivePower, 2));
                GroupBox.UpdateInterface(GroupBox.ActivePower, activePower);
            }
        }
        public Int32 ReactivePower
        {
            get
            {
                return reactivePower;
            }
            set
            {
                reactivePower = value;
                ApparentPower = Convert.ToInt32(Math.Sqrt(Math.Pow(activePower, 2) + Math.Pow(reactivePower, 2)));
                GroupBox.UpdateInterface(GroupBox.ReactivePower, reactivePower);
            }
        }
        public Int32 ApparentPower
        {
            get
            {
                return apparentPower;
            }
            set
            {
                apparentPower = value;
                if (ApparentPower != 0)
                    PowerFactor = (double) ActivePower / ApparentPower;
                GroupBox.UpdateInterface(GroupBox.ApparentPower, apparentPower);
            }
        }
        public Double PowerFactor
        {
            get
            {
                return powerFactor;
            }
            set
            {
                powerFactor = value;
                GroupBox.UpdateInterface(GroupBox.PowerFactor, powerFactor);
            }
        }

        public NodeType Type { get; private set; }
        #endregion

        #region Constructors
        public BaseNode()
        {
            Type = NodeType.Unknown;
            GroupBox = new BaseNodeGroupBox(SetNewAddress, "Directly Connected Node");
        }
        #endregion

        #region Public methods
        public void SetNewAddress(object sender, EventArgs e)
        {
            BaseNodeGroupBox parentGroupBox = (BaseNodeGroupBox)((Button)sender).Parent.Parent.Parent.Parent;
            parentGroupBox.Address = parentGroupBox.NewAddress;
            Address = parentGroupBox.Address;
            Program.Write(new Connection.Messages.Message(CommandType.SetAddress, parentGroupBox.Address));
        }
        #endregion
    }
}