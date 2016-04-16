using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class WorldGenerator : MonoBehaviour
{
    [Header("Room Settings")]
    public int MinWidth = 5;
    public int MaxWidth = 15;
    public int MinHeight = 5;
    public int MaxHeight = 15;
    public int WorldSize = 5;

    [Header("Environment root")]
    public Transform Env;

    [Header("Prefabs")]
    public GameObject FloorPrefab;
    public GameObject PlayerPrefab;

    private int m_roomId;

	// Use this for initialization
	void Start ()
	{
	    Random.seed = (int) DateTime.Now.Ticks;
        var w = Random.Range(MinWidth, MaxWidth);
        var h = Random.Range(MinHeight, MaxHeight);

        SpawnRoom(w, h, new Vector2(0, 0), 0);

	    foreach (Transform child in Env.transform)
	    {
	        child.GetComponent<BoxCollider2D>().enabled = false;
	    }

	    SpawnPlayer(new Vector2(0, 0));
	}

    private void SpawnPlayer(Vector2 vector2)
    {
        var player = Instantiate(PlayerPrefab, vector2, Quaternion.identity) as GameObject;
    }

    private void SpawnRoom(int w, int h, Vector2 pos, int distance)
    {
        var obj = Instantiate(FloorPrefab, pos, Quaternion.identity) as GameObject;
        obj.transform.parent = Env;
        obj.transform.localScale = new Vector3(w, h, 1);
        obj.name = "Floor " + m_roomId++;

        if (distance > WorldSize)
        {
            return;
        }

        int w2, h2;
        Vector2 pos2;
        
        // Right
        if (RandomBool())
        {
            w2 = Random.Range(MinWidth, MaxWidth);
            h2 = Random.Range(MinHeight, MaxHeight);

            pos2 = pos + new Vector2((w/2f) + 1 + (w2/2f), 0);

            if (Physics2D.BoxCastAll(pos2, new Vector2(w2, h2), 0f, Vector2.zero).Length == 0)
            {
                SpawnRoom(w2, h2, pos2, distance + 1);

                var tunnelPos = pos + new Vector2((w/2f) + 0.5f, 0);

                var tunnel = Instantiate(FloorPrefab, tunnelPos, Quaternion.identity) as GameObject;
                tunnel.transform.parent = Env;
            }
        }

        // Left
        if (RandomBool())
        {
            w2 = Random.Range(MinWidth, MaxWidth);
            h2 = Random.Range(MinHeight, MaxHeight);

            pos2 = pos + new Vector2(-((w / 2f) + 1 + (w2 / 2f)), 0);

            if (Physics2D.BoxCastAll(pos2, new Vector2(w2, h2), 0f, Vector2.zero).Length == 0)
            {
                SpawnRoom(w2, h2, pos2, distance + 1);

                var tunnelPos = pos + new Vector2(-((w / 2f) + 0.5f), 0);

                var tunnel = Instantiate(FloorPrefab, tunnelPos, Quaternion.identity) as GameObject;
                tunnel.transform.parent = Env;
            }
        }

        // Up
        if (RandomBool())
        {
            w2 = Random.Range(MinWidth, MaxWidth);
            h2 = Random.Range(MinHeight, MaxHeight);

            pos2 = pos + new Vector2(0, (h / 2f) + 1 + (h2 / 2f));
            
            if (Physics2D.BoxCastAll(pos2, new Vector2(w2, h2), 0f, Vector2.zero).Length == 0)
            {
                SpawnRoom(w2, h2, pos2, distance + 1);

                var tunnelPos = pos + new Vector2(0, (h / 2f) + 0.5f);

                var tunnel = Instantiate(FloorPrefab, tunnelPos, Quaternion.identity) as GameObject;
                tunnel.transform.parent = Env;
            }
        }

        // Down
        if (RandomBool())
        {
            w2 = Random.Range(MinWidth, MaxWidth);
            h2 = Random.Range(MinHeight, MaxHeight);

            pos2 = pos + new Vector2(0, -((h / 2f) + 1 + (h2 / 2f)));
            
            if (Physics2D.BoxCastAll(pos2, new Vector2(w2, h2), 0f, Vector2.zero).Length == 0)
            {
                SpawnRoom(w2, h2, pos2, distance + 1);

                var tunnelPos = pos + new Vector2(0, -((h / 2f) + 0.5f));

                var tunnel = Instantiate(FloorPrefab, tunnelPos, Quaternion.identity) as GameObject;
                tunnel.transform.parent = Env;
            }
        }
    }

    private bool RandomBool()
    {
        return true;
    }
}
