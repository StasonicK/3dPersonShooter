using UnityEngine;

namespace CodeBase.UI
{
    public class GUIPlatform : MonoBehaviour
    {
        [SerializeField] private GameObject _mobileGui;

        private void Awake()
        {
            if (Application.isMobilePlatform)
                _mobileGui.SetActive(true);
            else
                _mobileGui.SetActive(false);
        }
    }
}