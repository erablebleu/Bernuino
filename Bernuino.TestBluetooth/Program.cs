using Bernuino.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Bernuino.TestBluetooth
{
    class Program
    {
        static void Main(string[] args)
        {
            var sh = new SerialHelper
            {
                StartCharacter = 0x02,
                EndCharacter = 0x03,
            };

            /*
            foreach (int speed in new int[] {
                // 1200,
                2400,
                4800,
                9600,
                19200,
                38400,
                57600,
                115200, // max vitesse pc
                // 230400,
                // 460800,
                // 921600,
                // 1382400 
            })
            {

                Console.WriteLine($"Test vitesse {speed}");
                using (var port = new SerialPort("COM5", speed))
                {
                    port.Open();
                    Thread.Sleep(1000);
                    port.WriteLine($"vitesse : {speed}");
                }
                Thread.Sleep(1000);
            }*/
            using (var port = new SerialPort("COM6", 9600, Parity.None, 8, StopBits.One))
            {
                port.Encoding = Encoding.ASCII;
                port.Open();
                port.DataReceived += (s, e) => Console.WriteLine(port.ReadExisting());
                sh.Stream = port.BaseStream;
                while (true)
                {/*
                    sh.Write(Encoding.ASCII.GetBytes(Console.ReadLine()));
                    var msg = sh.Read();

                    if (msg != null)
                    {
                        Console.WriteLine(Encoding.ASCII.GetString(msg));
                        
                    }
                    */

                    var str = new MSG { MsgType = MsgType.SettingRequest, DataType = DataType.XOffset, Value = 42 };

                    WriteData(port.BaseStream, str);

                    Console.WriteLine();

                    Thread.Sleep(10000);
                }
            }


            //codeResetSpeed("AT+BAUD8", 921600);


            Console.ReadLine();
        }

        static void WriteResetError(Stream stream)
        {

        }
        static void WriteData(Stream stream, object obj)
        {
            /* <STX><Size><CHK><Data><ETX>
             *   1  |  2  | 1  |  x  | 1
             *  
             *  Avec :
             *      <Data> = <MsgType><...>
             *      
             * En cas d'erreur, répondre un MsgType=Error
             * En cas de réception de l'erreur, répondre <ETX><ETX><ETX><ETX><ETX>
             *      puis renvoyer le message ?
             * 
             */

            int size = Marshal.SizeOf(obj);
            byte[] arr = new byte[size + 5];

            // STX
            arr[0] = 0x02;

            // Size
            arr[1] = (byte)size;
            arr[2] = (byte)(size / 256);

            // Data
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(obj, ptr, true); // pas possible de tout faire en byte : pas possible de diggérencier un STX d'un int = 2
            Marshal.Copy(ptr, arr, 4, size);
            Marshal.FreeHGlobal(ptr);

            // CHK
            arr[3] = 0;
            for (int i = 4; i < size + 4; i++)
                arr[3] += arr[i];

            // ETX
            arr[arr.Length - 1] = 0x03;

            // Write data
            stream.Write(arr, 0, size + 5);
        }

        static void codeResetSpeed(string speedCommand, int baud)
        {
            byte[] msg = Encoding.ASCII.GetBytes(speedCommand);

            double cycleTime = 1000000.0 / 16000000.0; // en ns
            double bitTime = 1000000.0 / (double)baud; // en ns

            bool[] bin = msg.SelectMany(m => ToBin(m)).ToArray();

            int cnt = 0;
            double time = 0;

            while (time < bin.Length * bitTime)
            {
                if (time >= cnt * bitTime)
                {
                    bool val = bin[cnt];

                    Console.WriteLine();
                    if (cnt > 0 && bin[cnt - 1] == val)
                        Console.WriteLine("    \"nop\" \"\\n\\t\"");
                    else
                    {
                        if (val)
                            Console.WriteLine("    \"out 0x0B, r17\" \"\\n\\t\"");
                        else
                            Console.WriteLine("    \"out 0x0B, r16\" \"\\n\\t\"");
                    }

                    cnt++;
                }
                else
                {
                    Console.WriteLine("    \"nop\" \"\\n\\t\"");
                }
                time += cycleTime;
            }

        }

        static IEnumerable<bool> ToBin(byte b)
        {
            yield return false;

            for (int i = 0; i < 8; i++)
                yield return (b & 1 << i) != 0;

            yield return true;
            yield break;
        }
    }
}
