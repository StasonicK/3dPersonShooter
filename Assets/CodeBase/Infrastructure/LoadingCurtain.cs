using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class LoadingCurtain : MonoBehaviour, ILoadingCurtain
    {
        [SerializeField] private CanvasGroup _curtain;

        private const int MinimumAlpha = 0;
        private const int MaximumAlpha = 1;
        private const float StepAlpha = 0.03f;
        private const float PrepareWaiting = 2f;
        private bool _isInitial = true;

        public event Action FadedOut;

        private void Awake() =>
            DontDestroyOnLoad(this);

        public void Show()
        {
            gameObject.SetActive(true);
            _curtain.alpha = MaximumAlpha;
        }

        public void Hide()
        {
            StartCoroutine(FadeOut());
        }

        private IEnumerator FadeOut()
        {
            yield return new WaitForSeconds(PrepareWaiting);

            while (_curtain.alpha > MinimumAlpha)
            {
                _curtain.alpha -= StepAlpha;
                yield return new WaitForSeconds(StepAlpha);
            }

            FadedOut?.Invoke();
            gameObject.SetActive(false);
        }
    }
}