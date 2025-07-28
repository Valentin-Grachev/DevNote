using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


namespace DevNote.Tutorial.MVC
{
    public class ScoreWindowView : MonoBehaviour
    {
        // Здесь указываем сериализуемые поля, которые будем пробрасывать через инспектор
        [SerializeField] private ScoreProgressWidgetView _scoreProgressWidget;
        [SerializeField] private Button _addButton;
        [SerializeField] private Button _switchAdsButton;
        [SerializeField] private Button _showAdButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Image _adsEnabledImage;
        [SerializeField] private Color _adsDisabledColor;
        [SerializeField] private Color _adsEnabledColor;

        // Сюда прокидываем зависимости от контекста. Для контроллеров здесь всегда приписывайте постфикс Controller.
        private readonly Holder<ScoreController> scoreController = new();
        private readonly Holder<MenuController> menuController = new();

        // Также можем прокидывать сюда зависимости с глобального контекста.
        private readonly Holder<IAds> ads = new();

        // Обратите внимание, что параметры для анимации прописываются константами
        private const float SHOW_ANIMATION_DURATION = 1f;


        private void OnEnable()
        {
            // На события подписываемся в OnEnable
            scoreController.Item.CurrentScore.OnChanged += Display;
            GameState.AdsEnabled.OnChanged += Display;
        }
        private void OnDisable()
        {
            // На события отписываем в OnEnable
            scoreController.Item.CurrentScore.OnChanged -= Display;
            GameState.AdsEnabled.OnChanged -= Display;
        }

        private void Start()
        {
            // Ко всем кнопкам в игре всегда подписываемся через скрипты в методе Start!
            // Никогда не используем Unity Action через инспектор, у них куча проблем!
            // Отписка от событий нажатия кнопки не требуется.
            _addButton.onClick.AddListener(OnAddButtonClick);
            _switchAdsButton.onClick.AddListener(OnSwitchAdsButtonClick);
            _closeButton.onClick.AddListener(OnCloseButtonClick);
            _showAdButton.onClick.AddListener(OnShowAdButtonClick);
        }

        // При подписке на событие можно использовать общий метод Display() - его создаем для многих View
        public void Display()
        {
            int currentScore = scoreController.Item.CurrentScore.Value;

            // Смотрите как аккуратно и удобно мы тут получаем данные из конфигов
            int requiredScore = Configs.Score.GetScoreRequireForNextLevel(currentScore);

            // Смотрите как аккуратно и красиво делегируем отображение счета на виджет,
            // вместо того, чтобы городить в этой вьюхе дополнительную логику.
            _scoreProgressWidget.Display(currentScore, requiredScore);

            _adsEnabledImage.color = GameState.AdsEnabled.Value ? _adsEnabledColor : _adsDisabledColor;
        }

        public void AnimateShow()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(1f, SHOW_ANIMATION_DURATION).SetEase(Ease.OutFlash);
        }

        public void AnimateHide(Action onHidden = null)
        {
            transform.DOScale(0f, SHOW_ANIMATION_DURATION).SetEase(Ease.OutFlash)
                .onComplete += () => onHidden?.Invoke();
        }


        // Подписываясь на событие кнопки, всегда создаем метод с префиксом On и постфиксом ButtonClick, как тут.
        // Никогда не добавляйте в onClick.AddListener() какой-то метод напрямую.
        private void OnAddButtonClick() => scoreController.Item.AddScore();


        // Для совсем короткой логики в одну строчку можно выполнять логику без контроллера-посредника
        private void OnSwitchAdsButtonClick() => GameState.AdsEnabled.Value = !GameState.AdsEnabled.Value;


        private void OnShowAdButtonClick() => ads.Item.ShowRewarded(AdKey.None);


        private void OnCloseButtonClick()
        {
            scoreController.Item.HideScoreWindow();
            menuController.Item.ShowMenuButton();
        }

    }
}



