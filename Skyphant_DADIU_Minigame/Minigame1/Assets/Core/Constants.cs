using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace Assets.Core
{
    public static class Constants
    {
        public static class Tags
        {
            public static string GameMaster = "GameMaster";
            public static string Obstacle = "Obstacle";
        }

        public static List<List<PressType>> Combos = new List<List<PressType>>
        {
            new List<PressType> {PressType.Short, PressType.Short, PressType.Short},
            new List<PressType> {PressType.Short, PressType.Short, PressType.Long},
            new List<PressType> {PressType.Short, PressType.Long, PressType.Long},
			new List<PressType> {PressType.Short, PressType.Long, PressType.Long},
			new List<PressType> {PressType.Short, PressType.Long, PressType.Short},
			new List<PressType> {PressType.Long, PressType.Short, PressType.Short},
			new List<PressType> {PressType.Long, PressType.Long, PressType.Short},
			new List<PressType> {PressType.Long, PressType.Long, PressType.Long},
			new List<PressType> {PressType.Long, PressType.Short, PressType.Long}

        };

        public static class Scenes
        {
            public static string MainMenuSceneName = "MainMenu";
            public static string MainLevelSceneName = "EndlessFallingPrototypeToiPhone6";
        }

    }
}
