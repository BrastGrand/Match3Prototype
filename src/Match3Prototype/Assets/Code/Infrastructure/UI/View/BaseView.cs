using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Code.Infrastructure.UI.View
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class BaseView : MonoBehaviour, IView
    {
        [SerializeField] protected CanvasGroup canvasGroup;
        [SerializeField] protected float fadeDuration = 0.5f;

        private bool _isInitialized = false;

        protected virtual void Awake()
        {
            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
            }
        }

        public virtual void Initialize()
        {
            if (_isInitialized)
            {
                return;
            }

            OnInitialize();
            _isInitialized = true;
        }

        public virtual async UniTask Show()
        {
            if (!_isInitialized)
            {
                Initialize();
            }

            gameObject.SetActive(true);
            await ShowAnimation();
            await OnShow();
        }

        public virtual async UniTask Hide()
        {
            await OnHide();
            await HideAnimation();
            gameObject.SetActive(false);
        }

        public virtual void Cleanup()
        {
            OnCleanup();
            _isInitialized = false;
        }

        protected virtual void OnInitialize() { }

        protected virtual UniTask OnShow() => UniTask.CompletedTask;

        protected virtual UniTask OnHide() => UniTask.CompletedTask;

        protected virtual void OnCleanup() { }

        protected virtual void OnDestroy()
        {
            Cleanup();
        }

        protected virtual async UniTask ShowAnimation()
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
                await canvasGroup.DOFade(1f, fadeDuration).AsyncWaitForCompletion();
            }
            else
            {
                await UniTask.Delay((int)(fadeDuration * 1000));
            }
        }

        protected virtual async UniTask HideAnimation()
        {
            if (canvasGroup != null)
            {
                await canvasGroup.DOFade(0f, fadeDuration).AsyncWaitForCompletion();
            }
            else
            {
                await UniTask.Delay((int)(fadeDuration * 1000));
            }
        }
    }
}