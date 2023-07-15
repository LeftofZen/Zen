namespace Zenith.Colour
{
	public class RGBSumColorComparer : IComparer<ColourRGB>
	{
		public int Compare(ColourRGB a, ColourRGB b)
			=> (a.R + a.B + a.G).CompareTo(b.R + b.B + b.G);
	}
}
