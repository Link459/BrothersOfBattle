using System.Collections;
using HutongGames.PlayMaker.Actions;
using UnityEngine;

namespace BrothersOfBattle
{
    public class sly : MonoBehaviour
    {
        private PlayMakerFSM _slyControl;
        private PlayMakerFSM _stunControl;
        private HealthManager _hm;

        private void Awake()
        {
            _slyControl = gameObject.LocateMyFSM("Sly Boss");
;            _stunControl = gameObject.LocateMyFSM("Stun Control");
            _hm = GetComponent<HealthManager>();
        }

        private IEnumerator Start()
        {
            yield return null;

            while (HeroController.instance == null) yield return null;
            
            Destroy(_stunControl);

            _slyControl.GetAction<Wait>("Look").time.Value = 1.25f;
            
            _hm.hp = 99999;
        }
    }
}