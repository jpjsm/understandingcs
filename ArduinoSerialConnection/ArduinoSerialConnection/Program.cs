using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO.Ports;
using System.Data;

namespace ArduinoSerialConnection
{
    class Program
    {
        static void Main(string[] args)
        {
            SerialPort com3 = new SerialPort("COM4", 57600);
            string text;
            com3.DtrEnable = true;
            com3.ReadBufferSize = 4096;
            com3.Open();
            while (true)
            {
                text = com3.ReadExisting();
                Console.Write(text);
            }
        }
    }
}
