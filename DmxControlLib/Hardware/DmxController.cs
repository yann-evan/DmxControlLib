using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace DmxControlLib.Hardware
{
    public class DmxController
    {
        /// <summary>
        /// Port COM par default
        /// </summary>
        const string defaultComPort = "COM6";

        /// <summary>
        /// Serial port Object
        /// </summary>
        private  SerialPort _SerialPort;

        /// <summary>
        /// Is Controlleur Connected ?
        /// </summary>
        bool connected = false;

        /// <summary>
        /// Connecte le Controlleur DMX
        /// </summary>
        /// <param name="ComPort"></param>
        /// <exception cref="ErrorDmxConnectionException"
        /// <exception cref="AlreadyConnectedDmxDeviceException"
        public void Connect(string ComPort)
        {
            if (!connected)
            {
                try
                {
                    _SerialPort = new SerialPort(ComPort);
                    _SerialPort.BaudRate = 250000;
                    _SerialPort.Open();

                    connected = true;
                }
                catch (Exception)
                {
                    throw new ErrorDmxConnectionException();
                }
            }
            else
            {
                throw new AlreadyConnectedDmxDeviceException();
            }

        }

        /// <summary>
        /// connect DmxController with defaultComPort
        /// </summary>
        public void Connect()
        {
            Connect(defaultComPort);
        }

        /// <summary>
        /// Return ID of the controlleur
        /// </summary>
        /// <returns>Controlleurs' ID</returns>
        public string GetID()
        {
            string id = "";
            _SerialPort.Write("i");
            Task.Delay(10);
            id = _SerialPort.ReadExisting();
            return id;
        }

        /// <summary>
        /// Change DMX Value
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="value"></param>
        /// <exception cref="ErrorDmxSendDataException"
        /// <exception cref="NoDmxDeviceConnectedException"
        public void WriteValue(int channel, int value)
        {
            if (connected)
            {
                if (channel < 1 || channel > 512)
                    throw new ChannelOutOfRangeException();
                if (value < 0 || value > 255)
                    throw new ValueOutOfRangeException();

                try
                {
                    _SerialPort.Write(Convert.ToString(channel));
                    _SerialPort.Write("c");
                    _SerialPort.Write(Convert.ToString(value));
                    _SerialPort.Write("w");
                }
                catch (Exception)
                {
                    throw new ErrorDmxSendDataException();
                }
            }
            else
            {
                throw new NoDmxDeviceConnectedException();
            }
        }

        /// <summary>
        /// Close Controlleur
        /// </summary>
        public void Close()
        {
            if (_SerialPort != null)
            {
                if (_SerialPort.IsOpen)
                {
                    _SerialPort.Close();
                }
            }

            connected = false;

        }

        /// <summary>
        /// Is open ?
        /// </summary>
        /// <returns></returns>
        public bool IsOpen()
        {
            return connected;
        }
    }

    #region Dmx Exception
    public class NoDmxDeviceConnectedException : Exception
    {
        public NoDmxDeviceConnectedException() : base("Aucun Controlleur Dmx de connecté")
        {

        }
    }

    public class ChannelOutOfRangeException : Exception
    {
        public ChannelOutOfRangeException() : base("Canal Dmx invalide")
        {
        }
    }

    public class ValueOutOfRangeException : Exception
    {
        public ValueOutOfRangeException() : base("Valeur Dmx invalide")
        {
        }
    }

    public class AlreadyConnectedDmxDeviceException : Exception
    {
        public AlreadyConnectedDmxDeviceException() : base("Controlleur Dmx déja connecté")
        {

        }
    }

    public class ErrorDmxConnectionException : Exception
    {
        public ErrorDmxConnectionException() : base("Impossible de connecté le controleur Dmx")
        {

        }
    }

    public class ErrorDmxSendDataException : Exception
    {
        public ErrorDmxSendDataException() : base("Erreur de transition des données")
        {

        }
    }
    #endregion
}
