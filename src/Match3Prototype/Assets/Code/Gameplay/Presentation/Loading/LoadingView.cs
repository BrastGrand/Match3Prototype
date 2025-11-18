using System.Text;
using Code.Infrastructure.UI.View;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Code.Gameplay.Presentation.Loading
{
    public class LoadingView : BaseView
    {
        [SerializeField] private TextMeshProUGUI _loadingText;

        private const string _BASE_TEXT = "Loading";
        private readonly StringBuilder _stringBuilder = new StringBuilder();
        private int _dotCount = 0;
        private float _timer = 0f;

        protected override UniTask OnShow()
        {
            _dotCount = 0;
            _timer = 0f;

            UpdateText();

            return UniTask.CompletedTask;
        }

        private void Update()
        {
            if (!IsActive)
            {
                return;
            }

            _timer += Time.deltaTime;

            if (_timer >= 0.8f)
            {
                _timer = 0f;
                _dotCount = (_dotCount + 1) % 4;
                UpdateText();
            }
        }

        private void UpdateText()
        {
            _stringBuilder.Clear();
            _stringBuilder.Append(_BASE_TEXT);

            for (int i = 0; i < _dotCount; i++)
            {
                _stringBuilder.Append('.');
            }

            _loadingText.text = _stringBuilder.ToString();
        }
    }
}