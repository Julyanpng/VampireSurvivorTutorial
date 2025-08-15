using System.Collections.Generic;
using UnityEngine;

public class DropRateManager : MonoBehaviour
{
    [System.Serializable]
    public class Drops
    {
        public string name;
        public GameObject itemPrefab;
        public float dropRate;
    }

    public List<Drops> drops;
    bool isQuitting = false;

    void OnApplicationQuit()
    {
        isQuitting = true;
    }

    void OnDestroy()
    {
        if(!gameObject.scene.isLoaded)
        {
            return;
        }

        float randomNumber = UnityEngine.Random.Range(0, 100);
        List<Drops> possibleDrops = new List<Drops>();

        foreach (Drops rate in drops)
        {
            if (isQuitting) return;

            if (randomNumber <= rate.dropRate)
            {
                possibleDrops.Add(rate);
            }
        }
        //check if there are possible drops
        if (possibleDrops.Count > 0)
        {
            Drops drops = possibleDrops[UnityEngine.Random.Range(0, possibleDrops.Count)];
            Instantiate(drops.itemPrefab, transform.position, Quaternion.identity);
        }

    }
}
