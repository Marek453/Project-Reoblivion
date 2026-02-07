using System.Collections;
using System.Collections.Generic;
using GameCore.Player.Class;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.UI
{
    public class RpcAbilityInterface : MonoBehaviour
    {
        public List<RpcAbilityUi> rpcAbilityUis = new List<RpcAbilityUi>();
        public Transform rectToSpawn;

        public Slider SetEnableAbility(out Coroutine coroutine,RoleType type, string nameAbility, int maxTime, float maxValue)
        {
            RpcAbilityUi rpcAbilityUi = rpcAbilityUis.Find(raui => raui.roleType == type);
            GameObject abilityObject = Instantiate(rpcAbilityUi.prefab,rectToSpawn);
            coroutine = StartCoroutine(DestroyOnTime(abilityObject,maxTime));
            abilityObject.GetComponent<Slider>().maxValue = maxValue;
            return abilityObject.GetComponent<Slider>();
        }

        public IEnumerator DestroyOnTime(GameObject gameObject, float time)
        {
            if(gameObject == null) yield break;

            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }
    }
}
