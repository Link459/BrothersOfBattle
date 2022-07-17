using System.Collections;
using UnityEngine;

namespace BrothersOfBattle
{
    public class Oro : MonoBehaviour
    {
        private GameObject _sheo;

        private PlayMakerFSM _oroControl;
        private void Awake()
        {
            _oroControl = gameObject.LocateMyFSM("nailmaster");
        }

        private IEnumerator Start()
        {
            yield return null;

            while (HeroController.instance == null) yield return null;

            if (!PlayerData.instance.statueStateNailmasters.usingAltVersion) yield break;

            _oroControl.InsertMethod("Look 2", 0, SetSheoState);
            _oroControl.InsertMethod("Reactivate", 0, StartPhase2);

            CallSheo();
        }

        private void SetSheoState() => _sheo.LocateMyFSM("nailmaster_sheo").SetState("Look");

        private void StartPhase2() { gameObject.AddComponent<P2HealthController>(); gameObject.transform.position = new Vector3(45.0f, 6.9f, 0); }

        private void CallSheo()
        {
            Vector3 sheoPos = new Vector3(45.0f, 6.9f,2);
            Quaternion rotation = Quaternion.identity;
            _sheo = Instantiate(BrothersOfBattle.PreloadedGameObjects["Sheo"], sheoPos, rotation);
            _sheo.SetActive(true);
            _sheo.AddComponent<Sheo>();
            
            Vector3 paintingPos = new Vector3(47.8f, 6.4f,2);
            GameObject painting = Instantiate(BrothersOfBattle.PreloadedGameObjects["Painting"], paintingPos, rotation);
            painting.SetActive(true);
        }
    }
}