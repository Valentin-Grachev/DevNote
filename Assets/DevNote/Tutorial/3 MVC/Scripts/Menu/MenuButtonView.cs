using DevNote;
using DevNote.Tutorial.MVC;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonView : MonoBehaviour
{
    // View по своей сути не совсем Отображение, это скорее Presenter, так как он может получать сигналы от пользователя
    // в виде подписки на события кнопки
    [SerializeField] private Button _startButton;

    private readonly Holder<MenuController> menuController = new();
    private readonly Holder<ScoreController> scoreController = new();



    private void Start()
    {
        _startButton.onClick.AddListener(OnStartButtonClick);
    }

    private void OnStartButtonClick()
    {
        menuController.Item.HideMenuButton();
        scoreController.Item.ShowScoreWindow();
    }


}
