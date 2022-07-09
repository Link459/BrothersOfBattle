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

        private void StartPhase2() => gameObject.AddComponent<P2HealthController>();

        private void CallSheo()
        {
            Vector2 sheoPos = new Vector2(45.0f, 6.9f);
            Quaternion rotation = Quaternion.identity;
            _sheo = Instantiate(BrothersOfBattle.PreloadedGameObjects["Sheo"], sheoPos, rotation);
            _sheo.SetActive(true);
            _sheo.AddComponent<Sheo>();
            
            Vector2 paintingPos = new Vector2(47.8f, 6.4f);
            GameObject painting = Instantiate(BrothersOfBattle.PreloadedGameObjects["Painting"], paintingPos, rotation);
            painting.SetActive(true);
        }
    }
}