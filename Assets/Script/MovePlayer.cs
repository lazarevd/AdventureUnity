
using UnityEngine;

public class MovePlayer : MonoBehaviour
{

    public Transform player;

    void OnMouseDrag()
    {
        Vector3 mousePosition =Camera.main.ScreenToWorldPoint (Input.mousePosition);
        // player.position = Vector2.MoveTowards(player.position, new Vector2(mousePosition.x, player.position.y), 10.0f*Time.deltaTime);
        player.position = Vector2.MoveTowards(player.position, new Vector2(mousePosition.x, player.position.y), 10.0f * Time.deltaTime);
    }

}
