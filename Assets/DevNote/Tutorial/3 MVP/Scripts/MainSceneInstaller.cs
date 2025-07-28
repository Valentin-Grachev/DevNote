using UnityEngine;
using Zenject;


namespace DevNote.Tutorial.MVP
{
    // Это инсталлер для сцены.
    // Делайте весь биндинг в одном инсталлере сцены. Одна сцена - один инсталлер.
    // Не создавайте более одного инсталлера для сцены - это бессмысленно.
    public class MainSceneInstaller : MonoInstaller
    {
        // Сюда мы можем со сцены прокидывать зависимости, которые далее пойдут в контроллеры
        [SerializeField] private RectTransform _windowContainer;
        [SerializeField] private MenuButtonView _menuButton;


        public override void InstallBindings()
        {
            // Это Zenject-контейнер сцены, его оставляем всегда здесь
            new SceneInjector(Container);


            // Здесь создаем контроллер и сразу же биндим его.
            // В параметры контроллера можно передавать другие контроллеры
            // Здесь в названии постфикс Controller не нужен. При объявлении всегда используйте var.
            var score = Bind(new ScoreController(_windowContainer));
            var menu = Bind(new MenuController(_menuButton));

        }


        // Для биндинга всегда используйте эту функцию во всех инсталлерах.
        // Она правильно прокидывает все глобальные зависимости из ProjectContext.
        private T Bind<T>(T controller) where T : class
        {
            //ProjectInstaller.Builder.Register(controller);
            Container.BindInterfacesAndSelfTo<T>().FromInstance(controller).AsSingle();
            return controller;
        }
    }
}

