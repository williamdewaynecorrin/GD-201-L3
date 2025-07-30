using UnityEngine;

public class Pickup : MonoBehaviour
{
    public int coinValue = 1;
    public SpriteAnimator animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<PlayerInventory>(out PlayerInventory inventory))
        {
            inventory.AddCoins(coinValue);
            GameObject.Destroy(this.gameObject);
        }
    }
}
