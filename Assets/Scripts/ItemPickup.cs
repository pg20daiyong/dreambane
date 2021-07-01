using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickup : MonoBehaviour
{    
    [Header("Amount To Fill On Pickup")]
    [Range(0,100)]
    [SerializeField] private float amountToFill = 25f;

    private SimpleAudioPlayer clipsToPlay;

    private void Start()
    {
        clipsToPlay = GetComponent<SimpleAudioPlayer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        clipsToPlay.Play();
        GameMgr.instance.AddWaterToBucket(amountToFill);
        StartCoroutine(ExecuteAfterTime(.2f));
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
