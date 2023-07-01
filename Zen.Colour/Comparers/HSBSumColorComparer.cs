namespace Zen.Colour
{
	public class HSBSumColorComparer : IComparer<ColourHSB>
	{
		public int Compare(ColourHSB a, ColourHSB b)
			=> (a.Hue + a.Saturation + a.Brightness).CompareTo(b.Hue + b.Saturation + b.Brightness);
	}
}
