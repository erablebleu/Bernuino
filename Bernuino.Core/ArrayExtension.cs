using System;

namespace Bernuino.Core
{
    public static class ArrayExtension
    {
        #region Methods

        /// <summary>
        ///     Concatène les tableaux
        /// </summary>
        /// <param name="arr1"></param>
        /// <param name="arr2"></param>
        /// <param name="arr2Size"></param>
        /// <returns></returns>
        public static byte[] Append(this byte[] arr1, byte[] arr2, int? arr2Size = null)
        {
            int cnt = arr2Size ?? arr2.Length;
            var result = new byte[arr1.Length + cnt];
            Buffer.BlockCopy(arr1, 0, result, 0, arr1.Length);
            Buffer.BlockCopy(arr2, 0, result, arr1.Length, cnt);
            return result;
        }

        /// <summary>
        ///     Recherche un caractère et retourne ce qu'il y a après
        /// </summary>
        /// <param name="data"></param>
        /// <param name="search"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static byte[] GetAndRemove(this byte[] data, byte start, byte end, out byte[] message)
        {
            int startIndex = Array.IndexOf(data, start);
            int endIndex = Array.IndexOf(data, end);

            if (startIndex < 0
                || startIndex < 0)
            {
                message = null;
                return data;
            }

            if (startIndex >= endIndex)
                message = null;
            else
            {
                message = new byte[endIndex - startIndex - 1];
                Buffer.BlockCopy(data, startIndex + 1, message, 0, message.Length);
            }

            var result = new byte[data.Length - endIndex - 1];

            Buffer.BlockCopy(data, endIndex + 1, result, 0, result.Length);

            return result;
        }


        #endregion
    }
}