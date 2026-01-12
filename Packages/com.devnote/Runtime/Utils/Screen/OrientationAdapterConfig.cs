using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;


namespace DevNote
{
    [CreateAssetMenu(menuName = "DevNote/OrientationAdapter", fileName = "OrientationAdapter")]
    public class OrientationAdapterConfig : ScriptableObject
    {
        [field: BoxGroup("Active:"), ShowIf(nameof(IsChangeActive)), SerializeField, Label("Is Active In Portrait")] public bool PortraitGameObjectIsActive = true;
        [field: BoxGroup("Active:"), ShowIf(nameof(IsChangeActive)), SerializeField, Label("Is Active In Landscape")] public bool LandscapeGameObjectIsActive = true;

        [field: BoxGroup("Local Position:"), ShowIf(nameof(IsChangeLocalPosition)), SerializeField, Label("Portrait Position")] public Vector2 PortraitLocalPosition;
        [field: BoxGroup("Local Position:"), ShowIf(nameof(IsChangeLocalPosition)), SerializeField, Label("Landscape Position")] public Vector2 LandscapeLocalPosition;

        [field: BoxGroup("Scale:"), ShowIf(nameof(IsChangeScale)), SerializeField, Label("Portrait Scale")] public float PortraitScale;
        [field: BoxGroup("Scale:"), ShowIf(nameof(IsChangeScale)), SerializeField, Label("Landscape Scale")] public float LandscapeScale;

        [BoxGroup(""), SerializeField] private List<OrientationAdapter.ChangeOption> _changeOptions = new();

        public bool IsChangeScale => _changeOptions.Contains(OrientationAdapter.ChangeOption.Scale);
        public bool IsChangeLocalPosition => _changeOptions.Contains(OrientationAdapter.ChangeOption.LocalPosition);
        public bool IsChangeActive => _changeOptions.Contains(OrientationAdapter.ChangeOption.Active);




    }
}
