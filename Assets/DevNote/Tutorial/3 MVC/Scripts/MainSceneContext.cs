using UnityEngine;


namespace DevNote.Tutorial.MVC
{
    // Это контекст сцены. На одной сцене их в теории может быть несколько,
    // но все же, если на то нет особой необходимости - держите один SceneContext на каждую сцену.
    public class MainSceneContext : SceneContext
    {
        // Сюда мы можем со сцены прокидывать зависимости, которые далее пойдут в контроллеры
        [SerializeField] private RectTransform _windowContainer;
        [SerializeField] private MenuButtonView _menuButton;


        public override void RegisterContext()
        {
            // Здесь создаем контроллеры и сразу же регистрируем их.
            // В параметры контроллера можно передавать другие контроллеры
            // Здесь в названии постфикс Controller не нужен. При объявлении всегда используйте var.

            var score = Register(new ScoreController(_windowContainer));
            var menu = Register(new MenuController(_menuButton));
        }

    }
}

