using UnityEngine;

namespace BrothersOfBattle
{
    public class P2HealthController : MonoBehaviour
    {
        private HealthManager _oroHealth;
        private HealthManager _matoHealth;
        private HealthManager _sheoHealth;

        private int _healthPool = 1;
        
        private void Awake()
        {
            _oroHealth = GetComponent<HealthManager>();
            _matoHealth = GameObject.Find("Mato").GetComponent<HealthManager>();
            _sheoHealth = GameObject.Find("Sheo Boss(Clone)(Clone)").GetComponent<HealthManager>();

            On.HealthManager.TakeDamage += HealthManagerTakeDamage;
        }
        
        private void HealthManagerTakeDamage(On.HealthManager.orig_TakeDamage orig, HealthManager self, HitInstance hit)
        {
            _healthPool -= hit.DamageDealt;

            orig(self, hit);
        }
        
        private void Start()
        {
            _oroHealth.hp = 99999;
            _matoHealth.hp = 99999;
            _sheoHealth.hp = 99999;
            
            int bossLevel = BossSceneController.Instance.BossLevel;
            if (bossLevel > 0)
            {
                _healthPool = 1000 + 1000 + 1450;
            }
            else
            {
                _healthPool = 600 + 1000 + 950;
            }
        }

        private void Update()
        {
            if (_healthPool <= 0)
            {
                _oroHealth.Die(0, AttackTypes.Nail, true);
                _matoHealth.Die(0, AttackTypes.Nail, true);
                _sheoHealth.Die(0, AttackTypes.Nail, true);
                Destroy(this);
            }
        }
    }
}
