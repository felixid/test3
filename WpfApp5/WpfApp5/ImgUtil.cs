using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp5
{
	class ImgUtil
	{
		public byte[] YUV422toYUV444(byte[] YUV422)
		{
			return new byte[] { 0, 0, 0 };
		}

		public byte[] YUVtoBGR(byte[] YUV444)
		{		
			byte Y = YUV444[0];
			byte U = YUV444[1];
			byte V = YUV444[2];

			int C = Y - 16;
			int D = U - 128;
			int E = V - 128;

			byte R = clip((298 * C + 409 * E + 128) >> 8);
			byte G = clip((298 * C - 100 * D - 208 * E + 128) >> 8);
			byte B = clip((298 * C + 516 * D + 128) >> 8);

			return new byte[] { B, G, R };
		}

		private byte clip(int value)
		{
			if (value > 255) value = 255;
			if (value < 0) value = 0;
			return (byte)value;
		}
	}
}
