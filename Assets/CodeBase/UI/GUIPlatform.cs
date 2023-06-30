using UnityEngine;

namespace CodeBase.UI
{
    public class GUIPlatform : MonoBehaviour
    {
        [SerializeField] private GameObject _mobileGui;

        // private void Awake() =>
        //     CheckPlatform();

        private void CheckPlatform()
        {
            if (Application.isMobilePlatform)
                _mobileGui.SetActive(true);
            else
                _mobileGui.SetActive(false);
        }
    }
}