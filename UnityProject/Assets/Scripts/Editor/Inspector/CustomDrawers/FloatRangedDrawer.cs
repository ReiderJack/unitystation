using CustomVariables.Float;
using UnityEditor;
using UnityEngine;

namespace Inspector.CustomDrawers
{
	[CustomPropertyDrawer(typeof(FloatRanged),true)]
	public class FloatRangedDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property,GUIContent label)
		{
			label = EditorGUI.BeginProperty(position, label, property);
			position = EditorGUI.PrefixLabel(position, label);

			SerializedProperty minProp = property.FindPropertyRelative("minValue");
			SerializedProperty maxProp = property.FindPropertyRelative("maxValue");

			float minValue = minProp.floatValue;
			float maxValue = maxProp.floatValue;

			float rangeMin = 0;
			float rangeMax = 1;

			var floatRange = fieldInfo.GetCustomAttributes(typeof(FloatRangeAttribute), true) as FloatRangeAttribute[];

			if (floatRange.Length > 0)
			{
				 rangeMin = floatRange[0].Min;
				 rangeMax = floatRange[0].Max;
			}

			const float floatRangeBoundsWidth = 30f;

			var rangeBoundsLabel1Rect = new Rect(position)
			{
				width = floatRangeBoundsWidth
			};

			GUI.Label(rangeBoundsLabel1Rect, new GUIContent(minValue.ToString("F2")));
			position.xMin += floatRangeBoundsWidth;

			var rangeBoundsLabel2Rect = new Rect(position)
			{
				xMin = position.xMax - floatRangeBoundsWidth
			};

			GUI.Label(rangeBoundsLabel2Rect, new GUIContent(maxValue.ToString("F2")));
			position.xMax -= floatRangeBoundsWidth;

			EditorGUI.BeginChangeCheck();
			EditorGUI.MinMaxSlider(position, ref minValue, ref maxValue, rangeMin, rangeMax);
			if (EditorGUI.EndChangeCheck())
			{
				minProp.floatValue = minValue;
				maxProp.floatValue = maxValue;
			}

			EditorGUI.EndProperty();
		}
	}
}