using Modding;
using System;

namespace BrothersOfBattle
{
    [Serializable]
    public class LocalSettings
    {
        private bool _altStatue = false;
        public bool AltStatue
        {
            get => _altStatue;
            set => _altStatue = value;
        }

        private BossStatue.Completion _statueStateBrothers = new();
        public BossStatue.Completion StatueStateBrothers 
        { 
            get => _statueStateBrothers; 
            set => _statueStateBrothers = value; 
        }
    }
    public sealed partial class globalsettings : IGlobalSettings<GlobalSettings>
    {
        public static GlobalSettings GlobalSettings { get; private set; } = new();
        public void OnLoadGlobal(GlobalSettings s) => GlobalSettings = s;
        public GlobalSettings OnSaveGlobal() => GlobalSettings;
    }
    public sealed class GlobalSettings
    {
        public static bool healthshare = false;
        public static bool sly = false;
    }
}