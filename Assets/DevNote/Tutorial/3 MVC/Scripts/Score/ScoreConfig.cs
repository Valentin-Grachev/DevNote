using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DevNote.Tutorial.MVC
{
    // Прописываем в сектор Configs/ , называем без постфикса Config
    [CreateAssetMenu(menuName = "Configs/Score", fileName = "Score")]
    public class ScoreConfig : ScriptableObject
    {
        // Вложенные структуры удобно использовать в конфигах для организации данных
        // Кстати, аттрибуты можно также указывать в той же строчке, если хватает места.
        [Serializable] private struct LevelUpData
        {
            public int requiredScore;
            public int scorePerClick;
        }


        // Все поля в конфиге должны быть доступны на чтение, но никогда - для изменения!
        // Если это префаб - указываем постфиксом Prefab
        [field: SerializeField] public ScoreWindowView ScoreWindowPrefab { get; private set; }

        // На основе этого листа с данными, доступными для редактирования из инспектора,
        // Мы делаем публичные методы получения данных из этого конфига, представленные ниже
        [SerializeField] private List<LevelUpData> _levelUps;


        public int GetScorePerClick(int currentScore)
        {
            foreach (var levelUp in _levelUps)
            {
                if (currentScore < levelUp.requiredScore)
                    return levelUp.scorePerClick;
            }

            return _levelUps.Last().scorePerClick;
        }


        public int GetLevel(int currentScore)
        {
            for (int i = 0; i < _levelUps.Count; i++)
            {
                if (currentScore < _levelUps[i].requiredScore)
                    return i + 1;
            }

            return _levelUps.Count;
        }

        public int GetScoreRequireForNextLevel(int currentScore)
        {
            foreach (var levelUp in _levelUps)
            {
                if (currentScore < levelUp.requiredScore)
                    return levelUp.requiredScore;
            }

            return 0;
        }

        public bool IsMaxLevel(int currentScore) => currentScore >= _levelUps.Last().requiredScore;

    }
}


