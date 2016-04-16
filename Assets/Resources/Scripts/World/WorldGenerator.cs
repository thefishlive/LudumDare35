using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class WorldGenerator : MonoBehaviour
{
    private const byte c_tunnelVertical = 0;
    private const byte c_tunnelHorizontal = 1;

    [Header("Random seed")]
    public int RandomSeed = -1;

    [Header("Room Settings")]
    public int MinWidth = 5;
    public int MaxWidth = 10;
    public int MinHeight = 5;
    public int MaxHeight = 10;
    public int WorldSize = 5;

    [Header("Environment root")]
    public Transform Env;

    [Header("Prefabs")]
    public GameObject FloorPrefab;
    public GameObject PlayerPrefab;
    public GameObject WallPrefab;

    private int m_roomId;

	// Use this for initialization
	void Start ()
	{
	    Random.seed = RandomSeed == -1 ? (int) DateTime.Now.Ticks : RandomSeed;

        SpawnRoom(RandomWidth(), RandomHeight(), new Vector2(0, 0), 0);

	    foreach (Transform child in Env.transform)
	    {
	        child.GetComponent<BoxCollider2D>().enabled = false;
            child.GetComponent<Room>().GenerateWalls();
	    }

	    SpawnPlayer(new Vector2(0, 0));
	}

    private void SpawnPlayer(Vector2 vector2)
    {
        var player = Instantiate(PlayerPrefab, vector2, Quaternion.identity) as GameObject;
        player.name = "Player";
    }

    private Room SpawnRoom(int w, int h, Vector2 pos, int distance)
    {
        // Spawn floor
        var obj = Instantiate(FloorPrefab, pos, Quaternion.identity) as GameObject;
        obj.transform.parent = Env;
        obj.transform.localScale = new Vector3(w, h, 1);
        obj.name = "Room " + m_roomId++;

        var room = obj.GetComponent<Room>();
        room.Width = w;
        room.Height = h;

        if (distance > WorldSize)
        {
            // Deadend room
            return room;
        }

        int w2, h2;
        Vector2 pos2;
        
        // Right
        w2 = RandomWidth();
        h2 = RandomHeight();

        pos2 = pos + new Vector2((w/2f) + 1 + (w2/2f), 0);

        if (Physics2D.BoxCastAll(pos2, new Vector2(w2, h2), 0f, Vector2.zero).Length == 0)
        {
            room.ConnectionRight = SpawnRoom(w2, h2, pos2, distance + 1);
            SpawnTunnel(pos + new Vector2((w/2f) + 0.5f, 0), c_tunnelHorizontal);

            if (room.ConnectionRight != null)
            {
                room.ConnectionRight.ConnectionLeft = room;
            }
        }

        // Left
        w2 = RandomWidth();
        h2 = RandomHeight();

        pos2 = pos + new Vector2(-((w/2f) + 1 + (w2/2f)), 0);

        if (Physics2D.BoxCastAll(pos2, new Vector2(w2, h2), 0f, Vector2.zero).Length == 0)
        {
            room.ConnectionLeft = SpawnRoom(w2, h2, pos2, distance + 1);
            SpawnTunnel(pos + new Vector2(-((w/2f) + 0.5f), 0), c_tunnelHorizontal);

            if (room.ConnectionLeft != null)
            {
                room.ConnectionLeft.ConnectionRight = room;
            }
        }

        // Up
        w2 = RandomWidth();
        h2 = RandomHeight();

        pos2 = pos + new Vector2(0, (h/2f) + 1 + (h2/2f));

        if (Physics2D.BoxCastAll(pos2, new Vector2(w2, h2), 0f, Vector2.zero).Length == 0)
        {
            room.ConnectionUp = SpawnRoom(w2, h2, pos2, distance + 1);
            SpawnTunnel(pos + new Vector2(0, (h/2f) + 0.5f), c_tunnelVertical);

            if (room.ConnectionUp != null)
            {
                room.ConnectionUp.ConnectionDown = room;
            }
        }

        // Down
        w2 = RandomWidth();
        h2 = RandomHeight();

        pos2 = pos + new Vector2(0, -((h/2f) + 1 + (h2/2f)));

        if (Physics2D.BoxCastAll(pos2, new Vector2(w2, h2), 0f, Vector2.zero).Length == 0)
        {
            room.ConnectionDown = SpawnRoom(w2, h2, pos2, distance + 1);
            SpawnTunnel(pos + new Vector2(0, -((h/2f) + 0.5f)), c_tunnelVertical);

            if (room.ConnectionDown != null)
            {
                room.ConnectionDown.ConnectionUp = room;
            }
        }

        return room;
    }

    private void SpawnTunnel(Vector2 tunnelPos, byte direction)
    {
        var tunnel = Instantiate(FloorPrefab, tunnelPos, Quaternion.identity) as GameObject;
        tunnel.name = "Tunnel" + m_roomId++;
        tunnel.transform.parent = Env;
        tunnel.transform.localScale = direction == c_tunnelHorizontal ? new Vector3(1, 2) : new Vector3(2, 1);

        var wallPos1 = tunnelPos + (direction == c_tunnelHorizontal ? new Vector2(0, 1.5f) : new Vector2(1.5f, 0));
        var tunnelWall1 = Instantiate(WallPrefab, wallPos1, Quaternion.identity) as GameObject;
        tunnelWall1.name = "Tunnel Wall 1";
        tunnelWall1.transform.parent = tunnel.transform;
        tunnelWall1.transform.localScale = direction == c_tunnelHorizontal ? new Vector3(1, 0.5f, 1) : new Vector3(0.5f, 1, 1);

        var wallPos2 = tunnelPos + (direction == c_tunnelHorizontal ? new Vector2(0, -1.5f) : new Vector2(-1.5f, 0));
        var tunnelWall2 = Instantiate(WallPrefab, wallPos2, Quaternion.identity) as GameObject;
        tunnelWall2.name = "Tunnel Wall 2";
        tunnelWall2.transform.parent = tunnel.transform;
        tunnelWall2.transform.localScale = direction == c_tunnelHorizontal ? new Vector3(1, 0.5f, 1) : new Vector3(0.5f, 1, 1);
    }

    private bool RandomBool()
    {
        return true;
    }

    private int RandomWidth()
    {
        return Random.Range(MinWidth, MaxWidth)*2;
    }

    private int RandomHeight()
    {
        return Random.Range(MinHeight, MaxHeight) * 2;
    }
}
