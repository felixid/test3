using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using PylonC.NET;
//using OverlappedGrab;

namespace WpfApp5
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		public static PylonBuffer<Byte> Pylon_Buffer = new PylonBuffer<byte>(2046*2046*2, true); //user defined
		public static PylonGrabResult_t GrabResult;   //user defined

		public MainWindow()
		{
			InitializeComponent();

			PylonCam pylonCam = new PylonCam();
			pylonCam.PylonC_Open();
			byte[] pylonBuffer = Pylon_Buffer.Array;

			//Console.WriteLine("pylonBuffer pixel values:");
			//foreach (byte bufValue in pylonBuffer) Console.Write("{0, 4}", bufValue);
			Console.WriteLine(" pylonBuffer[] : {0},{1},{2},{3},{4},{5}", pylonBuffer[0], pylonBuffer[1], pylonBuffer[2], pylonBuffer[3], pylonBuffer[4], pylonBuffer[5]);
			Console.WriteLine(" Length of pylonBuffer : {0}", pylonBuffer.Length);
			//Console.ReadLine();


			// To verify the Y in grayscale from YUV422 rawImage/////////////////////////////////////////////////
			int iWidth = 2046;
			int iHeight = 2046;
			PixelFormat pf_test = PixelFormats.Gray8;
			int grayStride = (iWidth * pf_test.BitsPerPixel + 7) / 8;
			byte[] grayBuffer = new byte[grayStride * iHeight];
			for (int i = 0; i < iHeight; i++)
			{
				for (int j = 0; j < iWidth; j++)
				{
					grayBuffer[ j + i * iHeight ] = pylonBuffer[ 1 + (j + i * iHeight) * 2 ];
				}
			}
			Console.WriteLine("Lenght of grayBuffer : {0}.", grayBuffer.Length);
			Console.WriteLine("grayStride : {0}.", grayStride);
			Console.WriteLine("grayBuffer[] : {0},{1},{2},{3},{4},{5}", grayBuffer[0], grayBuffer[1], grayBuffer[2], grayBuffer[3], grayBuffer[4], grayBuffer[5]);

			// Create a BitmapSource.
			BitmapSource bitmap_test = BitmapSource.Create(iWidth, iHeight,
				96, 96, pf_test, null,
				grayBuffer, grayStride);
			// Create an image element;
			//Image myImage = new Image();
			//myImage.Width = 200;
			// Set image source.
			//myImage_test.Source = bitmap_test;
			//////////////////////////////////////////////////////////////////////////////////////////////////////

			
			// Define parameters used to create the BitmapSource.
			PixelFormat pf = PixelFormats.Gray8;
			int width = 2046;
			int height = 2046;
			int rawStride = (width * pf.BitsPerPixel + 7) / 8;
			byte[] rawImage = new byte[rawStride * height];

			rawImage = pylonBuffer;


			// Initialize the image with data.
			//Random value = new Random();
			//value.NextBytes(rawImage);

			ImgUtil imgUtil = new ImgUtil();
//			byte[] yuv422 = rawImage;

			int imgWidth = 2046;
			int imgHeight = 2046;
			//byte[] yuv444 = new byte[] { 100, 50, 10 };
			//byte[] bgr= imgUtil.YUVtoBGR(new byte[] { yuv444[0], yuv444[1], yuv444[2] });
			//Console.WriteLine("bgr: {0},{1},{2} from YUV: {3}, {4}, {5}", bgr[0], bgr[1], bgr[2], yuv444[0], yuv444[1], yuv444[2]);
			int yuv422Stride = imgWidth * 2;
			byte[] yuv422 = new byte[yuv422Stride * imgHeight];
			yuv422 = pylonBuffer;

			PixelFormat pf_bgr = PixelFormats.Bgr24;
			int bgrStride = (imgWidth * pf_bgr.BitsPerPixel + 7) / 8;
			byte[] bgr24 = new byte[bgrStride * imgHeight];

			/*
			for (int row = 0; row < imgHeight; row++)
			{
				for (int col = 0; col < yuv422Stride; col = col + 4)
				{
					int u0 = 0 + col + row * imgHeight;
					int y1 = 1 + col + row * imgHeight;
					int v2 = 2 + col + row * imgHeight;
					int y3 = 3 + col + row * imgHeight;

					byte[] yuv444, bgr;
					yuv444 = new byte[] { yuv422[ y1 ], yuv422[ u0 ], yuv422[ v2 ] };
					bgr = imgUtil.YUVtoBGR(yuv444);
					bgr24[ 0 + (col + row * imgHeight) * 3 / 2 ] = bgr[ 0 ];
					bgr24[ 1 + (col + row * imgHeight) * 3 / 2 ] = bgr[ 1 ];
					bgr24[ 2 + (col + row * imgHeight) * 3 / 2 ] = bgr[ 2 ];
					yuv444 = new byte[] { yuv422[ y3 ], yuv422[ u0 ], yuv422[ v2 ] };
					bgr = imgUtil.YUVtoBGR(yuv444);
					bgr24[ 3 + (col + row * imgHeight) * 3 / 2 ] = bgr[ 0 ];
					bgr24[ 4 + (col + row * imgHeight) * 3 / 2 ] = bgr[ 1 ];
					bgr24[ 5 + (col + row * imgHeight) * 3 / 2 ] = bgr[ 2 ];
				}
			}
			*/
			for (int row = 0; row < imgHeight; row++)
			{
				for (int col = 0; col < imgWidth; col = col + 2)
				{
					int u0 = 0 + (0 + col + row * imgHeight) * 2;
					int y1 = 1 + (0 + col + row * imgHeight) * 2;
					int v2 = 2 + (0 + col + row * imgHeight) * 2;
					int y3 = 3 + (0 + col + row * imgHeight) * 2;

					byte[] yuv444, bgr;
					yuv444 = new byte[] { yuv422[y1], yuv422[u0], yuv422[v2] };
					bgr = imgUtil.YUVtoBGR(yuv444);
					bgr24[0 + (0 + col + row * imgHeight) * 3] = bgr[0];
					bgr24[1 + (0 + col + row * imgHeight) * 3] = bgr[1];
					bgr24[2 + (0 + col + row * imgHeight) * 3] = bgr[2];
					yuv444 = new byte[] { yuv422[y3], yuv422[u0], yuv422[v2] };
					bgr = imgUtil.YUVtoBGR(yuv444);
					bgr24[3 + (0 + col + row * imgHeight) * 3] = bgr[0];
					bgr24[4 + (0 + col + row * imgHeight) * 3] = bgr[1];
					bgr24[5 + (0 + col + row * imgHeight) * 3] = bgr[2];
				}
			}
			Console.WriteLine("yuv422 buffer pixel values: 0th row");
			byte[] firstRowYuv = new byte[yuv422Stride];
			Array.Copy(yuv422, firstRowYuv, yuv422Stride);
			Console.WriteLine("Length of yuv422 buffer row: {0}", yuv422Stride);
			foreach ( byte y422 in firstRowYuv) Console.Write("{0, 4}", y422);
			Console.WriteLine("");

			Console.WriteLine("bgr24 buffer pixel values: 0th row");
			byte[] firstRowBgr24 = new byte[bgrStride];
			Array.Copy(bgr24, firstRowBgr24, bgrStride);
			Console.WriteLine("Length of bgr24 buffer row: {0}", bgrStride);
			foreach ( byte b24 in firstRowBgr24) Console.Write("{0, 4}", b24);
			Console.WriteLine("");

			Console.WriteLine("bgr[{0,3},{1,3},{2,3}] from YUV[{3,3},{4,3},{5,3},{6,3}]", bgr24[0], bgr24[1], bgr24[2], yuv422[0], yuv422[1], yuv422[2], yuv422[3]);
			Console.WriteLine("pylonBuffer[] : {0},{1},{2},{3},{4},{5}", bgr24[0], bgr24[1], bgr24[2], bgr24[3], bgr24[4], bgr24[5]);
			
			//rawStride = 12558348;
			Console.WriteLine("Size of yuv422 : {0}.", yuv422.Length);
			Console.WriteLine("Size of bgr24  : {0}.", bgr24.Length);
			Console.WriteLine("bgrStride[bgr24] : {0}.", bgrStride);
			Console.WriteLine("rawStride[gray8] : {0}.", rawStride);

			

			// Create a BitmapSource.
			BitmapSource bitmap = BitmapSource.Create(imgWidth, imgHeight,
				96, 96, pf_bgr, null,
				bgr24, bgrStride);

			// Create an image element;
			//Image myImage = new Image();
			//myImage.Width = 200;
			// Set image source.
			myImage.Source = bitmap;
		


			pylonCam.PylonC_Close();

			//Pylon.ImageWindowDisplayImage<Byte>(0, pylonCam.Pylon_Buffer, pylonCam.GrabResult);
		}
	}
}