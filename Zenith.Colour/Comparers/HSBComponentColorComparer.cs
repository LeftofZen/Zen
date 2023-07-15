namespace Zenith.Colour
{
	public class HSBComponentColorComparer : IComparer<ColourHSB>
	{
		readonly int hMult;
		readonly int sMult;
		readonly int bMult;

		public HSBComponentColorComparer(HSBComparerComponents hsbComponents)
		{
			if (hsbComponents == HSBComparerComponents.Empty)
			{
				throw new ArgumentException("HSBComparerComponents cannot be empty");
			}

			hMult = (int)(hsbComponents & HSBComparerComponents.Hue) >> 0;
			sMult = (int)(hsbComponents & HSBComparerComponents.Saturation) >> 1;
			bMult = (int)(hsbComponents & HSBComparerComponents.Brightness) >> 2;
		}

		public int Compare(ColourHSB a, ColourHSB b)
			=> (hMult * a.Hue.CompareTo(b.Hue))
			+ (sMult * a.Saturation.CompareTo(b.Saturation))
			+ (bMult * a.Brightness.CompareTo(b.Brightness));
	}
}
