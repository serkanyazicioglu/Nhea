using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nhea.Helper
{
    public static class BufferHelper
    {
        public static byte[] Concat(params byte[][] buffers)
        {
            int totalLength = 0;

            for (int i = 0; i < buffers.Length; i++)
            {
                totalLength += buffers[i].Length;
            }

            byte[] concatData = new byte[totalLength];

            int index = 0;

            for (int i = 0; i < buffers.Length; i++)
            {
                System.Buffer.BlockCopy(buffers[i], index, concatData, 0, buffers[i].Length);

                index += buffers[i].Length;
            }

            return concatData;
        }
    }
}
