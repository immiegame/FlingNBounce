using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformType 
{
    Normal,
    Moving,
    Healing,
    Spike
}


public class PlatformGenerator : MonoBehaviour
{
    public GameObject NormalPlatform;
    public GameObject MovingPlatform;
    public GameObject HealingPlatform;
    public GameObject SpikePlatform;

    private Vector2 screenBounds;
    public JumpMan player;

    public float MaxHeight = -1;

    public List<GameObject> GlobalPlatforms;



    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 300;
        GlobalPlatforms = new List<GameObject>();

        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        Vector2 toSpawn = new Vector2(screenBounds.x / 3, screenBounds.y / 2);

        CreateStartingPlatforms();
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    private void CreatePlatform(Vector2 Position, PlatformType platform)
    {
        if(!IsPlatformNearAnother(Position, 1.5f))
        {
            GameObject spawnedObject = Instantiate(getPlatform(platform)) as GameObject;
            spawnedObject.transform.position = Position;

            GlobalPlatforms.Add(spawnedObject);
        }
    }

    public GameObject getPlatform(PlatformType platform)
    {
        switch (platform) 
        {           
            case PlatformType.Moving:
                return MovingPlatform;

            case PlatformType.Healing:
                return HealingPlatform;
            
            case PlatformType.Spike:
                return SpikePlatform;

            default:
                return NormalPlatform;
        }
    }

    public void HandlePlayerJump()
    {
        if(MaxHeight < player.MaxHeight)
        {
            int PlatformsToGenerate = Random.Range(3, 12);
            PlatformType[] WeightedPlatforms = new PlatformType[] { PlatformType.Normal, PlatformType.Normal, PlatformType.Normal, PlatformType.Normal, PlatformType.Normal, PlatformType.Normal, PlatformType.Moving, PlatformType.Moving, PlatformType.Moving, PlatformType.Healing, PlatformType.Healing, PlatformType.Spike};
            for (int i = 0; i < PlatformsToGenerate; i++)
            {
                CreatePlatform(new Vector2(Random.Range(-2.2f, 2.3f), Random.Range(Camera.main.transform.position.y + 6, Camera.main.transform.position.y + 12)), WeightedPlatforms[Random.Range(0, WeightedPlatforms.Length)]);
            }
            MaxHeight = player.MaxHeight;
        }
    }

    public void CreateStartingPlatforms()
    {
        int PlatformsToGenerate = Random.Range(8, 24);
        for(int i = 0; i < PlatformsToGenerate; i++)
        {
            CreatePlatform(new Vector2(Random.Range(-2.2f, 2.3f), Random.Range(-1.5f, screenBounds.y + 2)), PlatformType.Normal);
        }

    }

    public bool IsPlatformNearAnother(Vector2 platform, float distanceThreshold)
    {
        
        for(int i = 0; i < GlobalPlatforms.Count; i++)
        {
            if(GlobalPlatforms[i] == null)
            {
                GlobalPlatforms.RemoveAt(i);

                continue;
            }
            Vector2 distVec;
            float dist;
            distVec.x = Mathf.Pow(GlobalPlatforms[i].transform.position.x - platform.x, 2f);
            distVec.y = Mathf.Pow(GlobalPlatforms[i].transform.position.y - platform.y, 2f);

            dist = Mathf.Sqrt(distVec.x + distVec.y);

            if (dist < distanceThreshold)
            {
                //Debug.Log("IS TOO CLOSE");
                return true;
            }

        }

        return false;
    }
}
