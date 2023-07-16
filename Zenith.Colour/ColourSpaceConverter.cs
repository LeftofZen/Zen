using Zenith.Core;

namespace Zenith.Colour
{
	public static class ColourSpaceConverter
	{
		public static ColourHSB RGBtoHSB(ColourRGB rgb)
		{
			double delta, min;
			double h = 0, s, b;

			min = Math.Min(Math.Min(rgb.R, rgb.G), rgb.B);
			b = Math.Max(Math.Max(rgb.R, rgb.G), rgb.B);
			delta = b - min;

			s = b == 0.0 ? 0 : delta / b;

			if (s == 0)
			{
				h = 0.0;
			}
			else
			{
				if (rgb.R == b)
				{
					h = (rgb.G - rgb.B) / delta;
				}
				else if (rgb.G == b)
				{
					h = 2 + ((rgb.B - rgb.R) / delta);
				}
				else if (rgb.B == b)
				{
					h = 4 + ((rgb.R - rgb.G) / delta);
				}

				h *= 60;

				if (h < 0.0)
				{
					h += 360;
				}
			}

			return new ColourHSB { Hue = (float)h / 360f, Saturation = (float)s, Brightness = (float)b };
		}

		public static ColourRGB HSBtoRGB(ColourHSB hsb)
		{
			double r;
			double g;
			double b;

			if (hsb.Saturation == 0)
			{
				r = hsb.Brightness;
				g = hsb.Brightness;
				b = hsb.Brightness;
			}
			else
			{
				int i;
				double f, p, q, t;
				var hue = hsb.Hue * 360;

				if ((int)hue == 360)
				{
					hue = 0;
				}
				else
				{
					hue /= 60;
				}

				i = (int)Math.Truncate(hue);
				f = hue - i;

				p = hsb.Brightness * (1.0 - hsb.Saturation);
				q = hsb.Brightness * (1.0 - (hsb.Saturation * f));
				t = hsb.Brightness * (1.0 - (hsb.Saturation * (1.0 - f)));

				switch (i)
				{
					case 0:
						r = hsb.Brightness;
						g = t;
						b = p;
						break;

					case 1:
						r = q;
						g = hsb.Brightness;
						b = p;
						break;

					case 2:
						r = p;
						g = hsb.Brightness;
						b = t;
						break;

					case 3:
						r = p;
						g = q;
						b = hsb.Brightness;
						break;

					case 4:
						r = t;
						g = p;
						b = hsb.Brightness;
						break;

					default:
						r = hsb.Brightness;
						g = p;
						b = q;
						break;
				}
			}

			return new ColourRGB { R = (float)r, G = (float)g, B = (float)b };
		}
	}
}