using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Modding;
using UnityEngine;
using UnityEngine.SceneManagement;
using USceneManager = UnityEngine.SceneManagement.SceneManager;
using UObject = UnityEngine.Object;

namespace BrothersOfBattle
{
    public class BrothersOfBattle : Mod, ILocalSettings<LocalSettings>
    {
        public static readonly List<Sprite> Sprites = new List<Sprite>();
        
        public static readonly Dictionary<string, GameObject> PreloadedGameObjects = new Dictionary<string,GameObject>();

        private LocalSettings _localSettings = new();
        public LocalSettings LocalSettings => _localSettings;

        public override string GetVersion() => "1.0.0.0";

        public override List<(string, string)> GetPreloadNames()
        {
            return new List<(string, string)>
            {
                ("GG_Painter", "Battle Scene/Sheo Boss"),
                ("GG_Painter", "painting_nailsmith_0001_paint2"),
            };
        }
        
        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Log("Storing GameObjects");
            PreloadedGameObjects.Add("Sheo", preloadedObjects["GG_Painter"]["Battle Scene/Sheo Boss"]);
            PreloadedGameObjects.Add("Painting", preloadedObjects["GG_Painter"]["painting_nailsmith_0001_paint2"]);

            ModHooks.BeforeSavegameSaveHook += BeforeSaveGameSave;
            ModHooks.AfterSavegameLoadHook += SaveGame;
            ModHooks.SavegameSaveHook += SaveGameSave;
            ModHooks.LanguageGetHook += OnLangGet;
            ModHooks.SetPlayerVariableHook += SetVariableHook;
            ModHooks.GetPlayerVariableHook += GetVariableHook;
            On.PlayMakerFSM.Start += OnPFSMStart;
            USceneManager.activeSceneChanged += SceneChanged;

            GameManager.instance.gameObject.AddComponent<OroFinder>();

            // Taken from https://github.com/SalehAce1/PaleChampion/blob/master/PaleChampion/PaleChampion/PaleChampion.cs
            int index = 0;
            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (string resource in assembly.GetManifestResourceNames())
            {
                if (!resource.EndsWith(".png"))
                {
                    continue;
                }
                
                using (Stream stream = assembly.GetManifestResourceStream(resource))
                {
                    if (stream == null) continue;

                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    stream.Dispose();

                    // Create texture from bytes
                    var texture = new Texture2D(1, 1);
                    texture.LoadImage(buffer, true);
                    Sprites.Add(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)));

                    Log("Created sprite from embedded image: " + resource + " at index " + index);
                    index++;
                }
            }
            
            Log("Initialized.");
        }
        
        private object SetVariableHook(Type t, string key, object obj)
        {
            if (key == "statueStateNailmasters")
                _localSettings.StatueStateBrothers = (BossStatue.Completion)obj;
            return obj;
        }

        private object GetVariableHook(Type t, string key, object orig)
        {
            return key == "statueStateNailmasters"
                ? _localSettings.StatueStateBrothers
                : orig;
        }

        public void OnLoadLocal(LocalSettings localSettings) => _localSettings = localSettings;
        public LocalSettings OnSaveLocal() => _localSettings;

        private string _previousScene;

        private void SceneChanged(Scene previousScene, Scene nextScene)
        {
            _previousScene = previousScene.name;
        }

        private string OnLangGet(string key, string sheettitle, string orig)
        {
            switch (key)
            {
                case "PAINTMASTER_SUPER" when _previousScene == "GG_Workshop" && PlayerData.instance.statueStateNailmasters.usingAltVersion:
                    return "Brothers";
                case "PAINTMASTER_MAIN" when _previousScene == "GG_Workshop" && PlayerData.instance.statueStateNailmasters.usingAltVersion:
                    return "of Battle";
                case "Nailmaster_Name":
                    return "Brothers of Battle";
                case "Nailmaster_Desc":
                    return "Loyal brother gods of nail and brush";
                case "TEMP_NM_SUPER" when _previousScene == "GG_Workshop" && PlayerData.instance.statueStateNailmasters.usingAltVersion:
                    return "Brothers";
                case "TEMP_NM_MAIN" when _previousScene == "GG_Workshop" && PlayerData.instance.statueStateNailmasters.usingAltVersion:
                    return "of Battle";
                default:
                    return orig;
            }
        }
        
        private void BeforeSaveGameSave(SaveGameData data) => _localSettings.AltStatue = PlayerData.instance.statueStatePaintmaster.usingAltVersion;

        private void SaveGame(SaveGameData data) => SaveGameSave();

        private void SaveGameSave(int id = 0) => PlayerData.instance.statueStateNailmasters.usingAltVersion = _localSettings.AltStatue;

        private void OnPFSMStart(On.PlayMakerFSM.orig_Start orig, PlayMakerFSM self)
        {
            if (self.name == "Oro")
            {
                self.gameObject.AddComponent<Oro>();
            }
            else if (self.name == "Mato")
            {
                self.gameObject.AddComponent<Mato>();
            }
            else if (self.name.Contains("Sheo Boss"))
            {
                self.gameObject.AddComponent<Sheo>();
            }

            orig(self);
        }
    }
}
