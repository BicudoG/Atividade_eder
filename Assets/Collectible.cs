using UnityEngine;

public class Collectible : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void OnMouseDown()
    {
        // Funciona no PC e também no celular (Unity trata toque como mouse)
        Collect();
    }

    void Collect()
    {
        string tag = gameObject.tag;

        if (tag == "Penalty")
        {
            float middleX = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0f)).x;
            string side = transform.position.x < middleX ? "Player1" : "Player2";

            ScoreManager.Instance.ApplyPenalty(side);
        }
        else if (tag == "Player1" || tag == "Player2")
        {
            ScoreManager.Instance.AddScore(tag);
        }

        Destroy(gameObject);
    }
}