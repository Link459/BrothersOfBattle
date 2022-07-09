using System.Collections;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace BrothersOfBattle
{
    public class Sheo : MonoBehaviour
    {
        private PlayMakerFSM _sheoControl;
        private PlayMakerFSM _corpseControl;
        private PlayMakerFSM _stunControl;
        private HealthManager _hm;

        private void Awake()
        {
            _sheoControl = gameObject.LocateMyFSM("nailmaster_sheo");
            _corpseControl = gameObject.transform.Find("Corpse Sheo(Clone)").gameObject.LocateMyFSM("Death Land");
;            _stunControl = gameObject.LocateMyFSM("Stun Control");
            _hm = GetComponent<HealthManager>();
        }

        private IEnumerator Start()
        {
            yield return null;

            while (HeroController.instance == null) yield return null;
            
            Destroy(_stunControl);

            _sheoControl.RemoveTransition("Painting", "Look");
            
            _sheoControl.GetAction<Wait>("Look").time.Value = 1.25f;
            _corpseControl.GetAction<Wait>("Death Land").time = 1.25f;
            
            _hm.hp = 99999;
        }
    }
}