using System;
using Random = UnityEngine.Random;

namespace CustomVariables
{
	[Serializable]
	public class RangedMinMaxFloat
	{
		public float minValue;
		public float maxValue;

		public float GetRandom() => Random.Range(minValue, maxValue);
	}
}