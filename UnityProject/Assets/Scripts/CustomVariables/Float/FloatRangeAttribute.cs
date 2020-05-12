using System;
namespace CustomVariables.Float
{
	public class FloatRangeAttribute : Attribute
	{
		public float Min { get; }

		public float Max { get; }

		public FloatRangeAttribute(float min, float max)
		{
			Min = min;
			Max = max;
		}
	}
}