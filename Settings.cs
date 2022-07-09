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
}