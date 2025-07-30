using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    public float speed = 0.1f;

    private void Awake()
    {

    }

    private void Start()
    {

    }

    private void Update()
    {
        //transform.position += Vector3.down * speed * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        //transform.position += Vector3.down * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    { 
        if(collision.gameObject.tag == "enemy")
        GameObject.Destroy(this.gameObject);
    }
}
