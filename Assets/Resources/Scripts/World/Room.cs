using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour
{
    [Header("Connections")]
    public Room ConnectionLeft;
    public Room ConnectionRight;
    public Room ConnectionUp;
    public Room ConnectionDown;

    [Header("Room data")]
    public int Width;
    public int Height;

    [Header("Prefabs")]
    public GameObject WallPrefab;
        
    public void GenerateWalls()
    {
        // Top
        var pos = transform.position + new Vector3(-(Width / 2f - 0.5f), Height / 2f - 0.5f, 0) ;

        for (var i = 0; i < Width - 1; i++)
        {
            if (!(ConnectionUp != null && (i == (Width / 2) || i == (Width / 2) - 1)))
            {
                var wall = Instantiate(WallPrefab, pos, Quaternion.identity) as GameObject;
                wall.transform.parent = transform;
            }

            pos += new Vector3(1, 0, 0);
        }

        // Right
        pos = transform.position + new Vector3(Width / 2f - 0.5f, Height / 2f - 0.5f, 0);

        for (var i = 0; i < Height - 1; i++)
        {
            if (!(ConnectionRight != null && (i == (Height / 2) || i == (Height / 2) - 1)))
            {
                var wall = Instantiate(WallPrefab, pos, Quaternion.identity) as GameObject;
                wall.transform.parent = transform;
            }

            pos += new Vector3(0, -1, 0);
        }

        // Bottom
        pos = transform.position + new Vector3(Width / 2f - 0.5f, -(Height / 2f - 0.5f), 0);

        for (var i = 0; i < Width - 1; i++)
        {
            if (!(ConnectionDown != null && (i == (Width / 2) || i == (Width / 2) - 1)))
            {
                var wall = Instantiate(WallPrefab, pos, Quaternion.identity) as GameObject;
                wall.transform.parent = transform;
            }

            pos += new Vector3(-1, 0, 0);
        }

        // Left
        pos = transform.position + new Vector3(-(Width / 2f - 0.5f), -(Height / 2f - 0.5f), 0);

        for (var i = 0; i < Height - 1; i++)
        {
            if (!(ConnectionLeft != null && (i == (Height / 2) || i == (Height / 2) - 1)))
            {
                var wall = Instantiate(WallPrefab, pos, Quaternion.identity) as GameObject;
                wall.transform.parent = transform;
            }

            pos += new Vector3(0, 1, 0);
        }

    }
}
