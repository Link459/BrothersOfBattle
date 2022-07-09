using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using USceneManager = UnityEngine.SceneManagement.SceneManager;

// Taken and modified from https://github.com/5FiftySix6/HollowKnight.Pale-Prince/blob/master/Pale%20Prince/PrinceFinder.cs

namespace BrothersOfBattle
{
    internal class OroFinder : MonoBehaviour
    {
        private void Start()
        {
            USceneManager.activeSceneChanged += SceneChanged;
        }

        private void SceneChanged(Scene previousScene, Scene nextScene)
        {
            if (nextScene.name == "GG_Workshop") SetStatue();
            if (nextScene.name != "GG_Nailmasters" ||
                !PlayerData.instance.statueStateNailmasters.usingAltVersion) 
                return;
        }

        private void SetStatue()
        {
            GameObject statue = GameObject.Find("GG_Statue_Nailmasters");

            BossScene scene = ScriptableObject.CreateInstance<BossScene>();
            scene.sceneName = "GG_Nailmasters";

            BossStatue bs = statue.GetComponent<BossStatue>();
            bs.dreamBossScene = scene;
            bs.dreamStatueStatePD = "statueStateNailmasters";

            bs.SetPlaquesVisible(bs.StatueState.isUnlocked && bs.StatueState.hasBeenSeen || bs.isAlwaysUnlocked);

            Destroy(statue.transform.Find("Base/StatueAlt").gameObject);

            GameObject displayStatue = bs.statueDisplay;

            GameObject alt = Instantiate
            (
                displayStatue,
                displayStatue.transform.parent,
                true
            );
            alt.SetActive(bs.UsingDreamVersion);
            SpriteRenderer spriteRenderer = alt.GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.sprite = BrothersOfBattle.Sprites[0];
            spriteRenderer.transform.position += Vector3.up * 1.8f;
            alt.name = "StatueAlt";
            bs.statueDisplayAlt = alt;

            BossStatue.BossUIDetails details = new BossStatue.BossUIDetails();
            details.nameKey = details.nameSheet = "Nailmaster_Name";
            details.descriptionKey = details.descriptionSheet = "Nailmaster_Desc";
            bs.dreamBossDetails = details;

            GameObject altLever = statue.transform.Find("alt_lever").gameObject;
            altLever.SetActive(true);
            altLever.transform.position = new Vector3(150.7f, 37.5f, 0.9f);

            GameObject switchBracket = altLever.transform.Find("GG_statue_switch_bracket").gameObject;
            switchBracket.SetActive(true);

            GameObject switchLever = altLever.transform.Find("GG_statue_switch_lever").gameObject;
            switchLever.SetActive(true);

            BossStatueLever toggle = statue.GetComponentInChildren<BossStatueLever>();
            toggle.SetOwner(bs);
            toggle.SetState(true);
        }

        private void OnDestroy()
        {
            USceneManager.activeSceneChanged -= SceneChanged;
        }
    }
}