using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class LinearGradient5KeysGUI : ShaderGUI
{
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        Material targetMat = materialEditor.target as Material;

        MaterialProperty colorCountProp = FindProperty("_ColorCount", properties);

        int count = Mathf.Clamp((int)colorCountProp.floatValue, 1, 5);
        count = EditorGUILayout.IntSlider("Number of Colors", count, 1, 5);
        colorCountProp.floatValue = count;

        for (int i = 0; i < 5; i++)
        {
            if (i < count)
            {
                MaterialProperty color = FindProperty("_Color" + i, properties);
                MaterialProperty pos = FindProperty("_Pos" + i, properties);
                materialEditor.ShaderProperty(color, $"Color {i}");
                materialEditor.ShaderProperty(pos, $"Pos {i}");
            }
        }

        MaterialProperty angle = FindProperty("_Angle", properties);
        materialEditor.ShaderProperty(angle, "Gradient Angle");

        MaterialProperty mainTex = FindProperty("_MainTex", properties);
        materialEditor.ShaderProperty(mainTex, "Main Texture");
    }
}

#endif


