using UnityEngine;
using System.Collections;
using Pathfinding;

[RequireComponent(typeof(Seeker))]
public class AIController : MonoBehaviour
{
    public float Speed = 0.1f;
    public float NextWaypointDistance = 1f;

    private Path m_path;
    private Seeker m_seeker;
    private int m_current;

    public GameObject Target { get; set; }

    void Start()
    {
        Debug.Log("Starting");
        m_seeker = GetComponent<Seeker>();
        SeekPlayer(Target);

        StartCoroutine(UpdateAITarget());
    }

    void OnDestroy()
    {
        PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + 1);
    }

    private IEnumerator UpdateAITarget()
    {
        var wait = new WaitForSeconds(Random.Range(1f, 4f));

        while (enabled)
        {
            yield return wait;
            SeekPlayer(Target);
        }
    }

    public void SeekPlayer(GameObject target)
    {
        m_seeker.StartPath(transform.position, target.transform.position, OnPathComplete);
    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("Yay, we got a path back. Did it have an error? " + p.error);

        if (!p.error)
        {
            m_path = p;
            m_current = 0;
        }
    }

    void Update()
    {
        if (m_path == null)
        {
            return;
        }

        if (m_current >= m_path.vectorPath.Count)
        {
            Debug.Log("Finished path");
            m_path = null;
            return;
        }
        
        var dir = (m_path.vectorPath[m_current] - transform.position).normalized;
        dir *= Speed*Time.deltaTime;
        transform.position += dir;

        //Check if we are close enough to the next waypoint
        //If we are, proceed to follow the next waypoint
        if (Vector3.Distance(transform.position, m_path.vectorPath[m_current]) < NextWaypointDistance)
        {
            m_current++;
        }
    }
}
