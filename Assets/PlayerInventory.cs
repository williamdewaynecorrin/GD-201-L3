using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private int totalCoins = 0;
    public int TotalCoins => totalCoins;

    public void AddCoins(int amt)
    {
        totalCoins += amt;
    }

    public void ResetCoins()
    {
        totalCoins = 0;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(5, 5, 200, 30), string.Format("COINS: {0}", TotalCoins));
    }
}
