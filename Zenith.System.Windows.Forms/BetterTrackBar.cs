namespace all_rgb_gui
{
	public partial class BetterTrackBar : UserControl
	{
		public BetterTrackBar()
			=> InitializeComponent();

		public string Caption { get => lblCaption.Text; set => lblCaption.Text = value; }
		public int Minimum { get => trbInner.Minimum; set => trbInner.Minimum = value; }
		public int Maximum { get => trbInner.Maximum; set => trbInner.Maximum = value; }
		public string Value { get => tbValue.Text; set => tbValue.Text = value; }

		public float ValueAsNormalisedFloat
			=> float.TryParse(Value, out var result) ? (result - trbInner.Minimum) / (trbInner.Maximum - trbInner.Minimum) : float.NaN;

		public int ValueAsInt
			=> int.TryParse(Value, out var result) ? result : 0;

		private void trbInner_ValueChanged(object sender, System.EventArgs e)
		{
			var newVal = trbInner.Value.ToString();
			if (tbValue.Text != newVal)
			{
				tbValue.Text = newVal;
			}
		}

		private void tbValue_TextChanged(object sender, System.EventArgs e)
		{
			if (int.TryParse(tbValue.Text, out var newVal))
			{
				if (trbInner.Value != newVal)
				{
					trbInner.Value = newVal;
				}
			}
		}
	}
}
