using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Bernuino.Core
{
    public enum MsgType : short
    {
        SettingAffectation = 1,
        SettingRequest,
        StitchAffectation,
        Error = 99,
    }
    public enum DataType : short
    {
        XOffset = 1,
        XScale,
        YOffset,
        YScale,
    }
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct MSG
    {
        public MsgType MsgType;
        public DataType DataType;
        public float Value;
    }

    public class SerialHelper
    {
        #region Constructors

        #endregion

        #region Events

        #endregion

        #region Fields

        private byte[] _data = new byte[0];

        #endregion

        #region Methods

        /// <summary>
        ///     Réception de données
        /// </summary>
        /// <returns></returns>
        public byte[] Read(int? bufferSize = null)
        {
            var buffer = new byte[bufferSize ?? 1024];
            int len;
                        
            while ((len = Stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                LastReadDate = DateTime.Now;
                _data = _data.Append(buffer, len);
            }

            _data = _data.GetAndRemove(StartCharacter, EndCharacter, out var message);
            return message;
        }

        /// <summary>
        ///     Envoi des données sur le stream
        /// </summary>
        /// <param name="data"></param>
        public void Write(byte[] data)
        {
            if (data is null)
                return;

            byte[] packet = new byte[data.Length + 2];

            packet[0] = StartCharacter;
            Buffer.BlockCopy(data, 0, packet, 1, data.Length);
            packet[packet.Length - 1] = EndCharacter;

            Stream.Write(packet, 0, packet.Length);

            LastWriteDate = DateTime.Now;
        }

        #endregion

        #region Properties

        public Stream Stream { get; set; }
        public DateTime LastReadDate { get; set; }
        public DateTime LastWriteDate { get; set; }
        public byte StartCharacter { get; set; }
        public byte EndCharacter { get; set; }

        #endregion
    }
}
