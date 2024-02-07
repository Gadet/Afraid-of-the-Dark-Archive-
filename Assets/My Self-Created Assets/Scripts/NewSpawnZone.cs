using UnityEngine;

public class NewSpawnZone : MonoBehaviour
{
    private bool dirty = false;
    public GameObject door;
    public GameObject monster;
    public GameObject canvas;
    public GameObject antiBacktrack;
    private MonsterMovement monsterScript;

    private void Start()
    {
        monsterScript = monster.GetComponent<MonsterMovement>();
    }

    // This should happen when entering a new area.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            door.GetComponent<Animator>().SetBool("open", false);

            if (!dirty)
            {
                dirty = true;
                canvas.SetActive(true);
                monsterScript.area = transform.parent;
                monsterScript.GetWaypoints();
                antiBacktrack.GetComponent<BoxCollider>().enabled = true;
            }
            
        }
    }
}
