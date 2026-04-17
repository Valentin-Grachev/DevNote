
namespace DevNote
{
#if UNITY_EDITOR
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(ToggleInt))]
    public class EnabledIntDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var boolProp = property.FindPropertyRelative("enabled");
            var intProp = property.FindPropertyRelative("value");

            position = EditorGUI.PrefixLabel(position, label);

            float toggleWidth = 20f;

            Rect boolRect = new Rect(position.x, position.y, toggleWidth, position.height);
            Rect intRect = new Rect(position.x + toggleWidth + 5, position.y,
                                    position.width - toggleWidth - 5, position.height);

            boolProp.boolValue = EditorGUI.Toggle(boolRect, boolProp.boolValue);

            if (boolProp.boolValue)
            {
                EditorGUI.PropertyField(intRect, intProp, GUIContent.none);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
#endif

    [System.Serializable]
    public class ToggleInt
    {
        public bool enabled;
        public int value;
    }

}
