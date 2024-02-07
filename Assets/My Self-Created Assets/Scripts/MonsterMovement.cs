using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public Transform firstAreaAppearance;
    public Transform area;
    public AudioSource audioSteps;
    public AudioSource audioBreath;
    public AudioSource audioScream;
    public float speed = 4f;
    private Transform targetWaypoint;
    private System.Collections.Generic.List<Transform> areaWaypoints = new System.Collections.Generic.List<Transform>();
    private int waypointIndex = 0;
    public bool busy = false;

    // Start is called before the first frame update
    void Start()
    {
        area = firstAreaAppearance;
        GetWaypoints();
    }

    // Update is called once per frame
    void Update()
    {
        if (!busy)
        {
            Move();
        }
    }
    
    // This function assumes a group of waypoints is grouped under an object named MonsterMovement and are ordered in the steps the ai will take
    public void GetWaypoints()
    {
        waypointIndex = 0;
        areaWaypoints.Clear();
        Transform container = area.Find("MonsterMovement");

        foreach (Transform child in container)
        {
            areaWaypoints.Add(child);
        }
        GameObject.Find("Monster").transform.position = areaWaypoints[0].position;
        targetWaypoint = areaWaypoints[0];
    }

    // Move and look to each waypoint, then choose the next one in the list.
    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(targetWaypoint.position - transform.position);

        if (transform.position == targetWaypoint.transform.position)
        {
            waypointIndex += 1;
            if (waypointIndex == areaWaypoints.Count)
            {
                waypointIndex = 0;
            }
            Transform next = areaWaypoints[waypointIndex].transform;
            targetWaypoint = next;
        }
    }

    // Collides with player RIP
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            busy = true;

            audioSteps.Stop();
            audioBreath.Stop();
            other.gameObject.GetComponent<PlayerMovement>().OnDeath();
            transform.rotation = Quaternion.LookRotation(other.transform.position - transform.position);
            transform.rotation = Quaternion.Euler(20, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            transform.position = Vector3.MoveTowards(transform.position, other.transform.position, 0.1f);
            transform.position += transform.forward * -1.5f;
            GetComponent<Animator>().SetBool("onDeath", true);
            audioScream.Play();
        }
    }
}
