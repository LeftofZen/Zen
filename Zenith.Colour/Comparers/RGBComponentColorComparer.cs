namespace Zenith.Colour
{
	public class RGBComponentColorComparer : IComparer<ColourRGB>
	{
		readonly int rMult;
		readonly int gMult;
		readonly int bMult;

		public RGBComponentColorComparer(RGBComparerComponents rgbComponents)
		{
			if (rgbComponents == RGBComparerComponents.Empty)
			{
				throw new ArgumentException("RGBComparerComponents cannot be empty");
			}

			rMult = (int)(rgbComponents & RGBComparerComponents.Red) >> 0;
			gMult = (int)(rgbComponents & RGBComparerComponents.Green) >> 1;
			bMult = (int)(rgbComponents & RGBComparerComponents.Blue) >> 2;
		}

		public int Compare(ColourRGB a, ColourRGB b)
			=> (rMult * a.R.CompareTo(b.R))
			+ (gMult * a.G.CompareTo(b.G))
			+ (bMult * a.B.CompareTo(b.B));
	}
}
