using System.Collections;
using UnityEngine;

namespace BrothersOfBattle
{
    public class Mato : MonoBehaviour
    {
        private PlayMakerFSM _matoControl;
        private HealthManager _hm;
        private void Awake()
        {
            _matoControl = gameObject.LocateMyFSM("nailmaster");
            _hm = GetComponent<HealthManager>();
        }

        private IEnumerator Start()
        {
            yield return null;

            while (HeroController.instance == null) yield return null;
            
            if (!PlayerData.instance.statueStateNailmasters.usingAltVersion) yield break;

            _hm.hp = 99999;
        }
    }
}