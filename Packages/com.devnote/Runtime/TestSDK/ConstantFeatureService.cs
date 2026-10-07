using UnityEngine;

namespace DevNote
{
    public class ConstantFeatureService : MonoBehaviour, IFeature
    {
        [SerializeField] private bool _isStartMenu;


        bool IFeature.IsStartMenu => _isStartMenu;

        bool IInitializable.Initialized => true;

        bool ISelectableService.IsAvailableForSelection => true;

        void IInitializable.Initialize() { }
    }
}
