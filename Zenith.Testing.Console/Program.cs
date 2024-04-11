// See https://aka.ms/new-console-template for more information
using System.Drawing;
using Zenith.Drawing;
using Zenith.System.Drawing;

Console.WriteLine("Hello, World!");

var basePath = @"C:\Users\bigba\OneDrive\Pictures";
var filename = "crown-royal";
var extension = ".png";
var img = new Bitmap(Path.Combine(basePath, filename + extension));
var imgBuf = ImageBufferHelpers.FromBitmap(img);

//imgBuf.Convolve(Kernels.Identity, EdgeHandling.Crop);

var newBuf = imgBuf.Convolve(Kernels.UnsharpMask, EdgeHandling.Skip);

var output = newBuf.GetImage();
output.Save(Path.Combine(basePath, filename + "_" + extension));

Console.WriteLine("Goodbye, World!");
//Console.ReadLine();
