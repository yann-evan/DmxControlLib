using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace DmxControlLib.Hardware
{
    public static class DmxController
    {
        const string defaultComPort = "COM6";

        /// <summary>
        /// Serial port Object
        /// </summary>
        private static SerialPort _SerialPort;

        /// <summary>
        /// Is Controlleur Connected ?
        /// </summary>
        static bool connected = false;

        /// <summary>
        /// connect DmxController
        /// </summary>
        public static void Connect(string ComPort)
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
        public static void Connect()
        {
            Connect(defaultComPort);
        }

        /// <summary>
        /// Return ID of the controlleur
        /// </summary>
        /// <returns>Controlleurs' ID</returns>
        public static string GetID()
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
        public static void WriteValue(int channel, int value)
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
        public static void Close()
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
        public static bool IsOpen()
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
