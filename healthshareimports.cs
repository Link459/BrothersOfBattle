using UnityEngine;
using System.Collections.Generic;
using Osmi.Utils;

namespace BrothersOfBattle
{
    internal class Healthshareimports : HealthManager
    { 
        public void Healthshare()
        {
            P2HealthController._oroHealth = GameObject.Find("Oro").GetComponent<HealthManager>();
            P2HealthController._matoHealth = GameObject.Find("Mato").GetComponent<HealthManager>();
            P2HealthController._sheoHealth = GameObject.Find("Mato").GetComponent<HealthManager>();
            if (GlobalSettings.healthshare == true)
            {
               

            }

        }
    }
}
