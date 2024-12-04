using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    Collider2D collider2d;
    [SerializeField]
    public List<Collider2D> colliders = new List<Collider2D> ();

    private void Awake()
    {
        collider2d = GetComponent<Collider2D>();
    }
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D collision)
    {
        colliders.Add(collider2d);
    }

    // Update is called once per frame
    private void OnTriggerExit2D(Collider2D collision)
    {
        colliders.Remove(collider2d);
    }
}
