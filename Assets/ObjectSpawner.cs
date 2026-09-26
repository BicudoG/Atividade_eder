using UnityEngine;
using System.Collections;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject normalObjectPrefab;   // Prefab do objeto normal
    public GameObject penaltyObjectPrefab;  // Prefab do objeto de penalidade

    [Header("Configurações de Spawn")]
    public float spawnInterval = 1.5f;      // Tempo entre spawns normais
    public float penaltyInterval = 8f;      // Tempo entre spawns de penalidade
    public float objectLifetime = 5f;       // Tempo que o objeto fica vivo (opcional)

    [Header("Área de Spawn (baseada na câmera)")]
    public float margin = 0.5f;             // Margem das bordas da tela

    private Camera mainCamera;
    private float minX, maxX, minY, maxY;
    private float middleX;                  // Linha do meio da tela

    void Start()
    {
        mainCamera = Camera.main;

        // Calcula os limites da tela em coordenadas do mundo
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

        minX = bottomLeft.x + margin;
        maxX = topRight.x - margin;
        minY = bottomLeft.y + margin;
        maxY = topRight.y - margin;

        middleX = (minX + maxX) / 2f;       // Centro da tela no eixo X

        // Inicia os spawns
        StartCoroutine(SpawnNormalObjects());
        StartCoroutine(SpawnPenaltyObjects());
    }

    IEnumerator SpawnNormalObjects()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnObject(false); // false = objeto normal
        }
    }

    IEnumerator SpawnPenaltyObjects()
    {
        while (true)
        {
            yield return new WaitForSeconds(penaltyInterval);
            SpawnObject(true); // true = penalidade
        }
    }

    void SpawnObject(bool isPenalty)
    {
        // Decide se vai nascer no lado esquerdo ou direito
        bool leftSide = Random.value > 0.5f;

        float spawnX;
        string tagToApply;

        if (isPenalty)
        {
            // Penalidade pode nascer em qualquer lado
            spawnX = Random.Range(minX, maxX);
            tagToApply = "Penalty";
        }
        else
        {
            if (leftSide)
            {
                // Lado do Player 1 (esquerda)
                spawnX = Random.Range(minX, middleX);
                tagToApply = "Player1";
            }
            else
            {
                // Lado do Player 2 (direita)
                spawnX = Random.Range(middleX, maxX);
                tagToApply = "Player2";
            }
        }

        float spawnY = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);

        // Escolhe o prefab
        GameObject prefab = isPenalty ? penaltyObjectPrefab : normalObjectPrefab;
        GameObject obj = Instantiate(prefab, spawnPosition, Quaternion.identity);

        // Aplica a tag
        obj.tag = tagToApply;

        // (Opcional) Destrói o objeto depois de um tempo
        if (objectLifetime > 0)
        {
            Destroy(obj, objectLifetime);
        }
    }

    // Método para visualizar a divisão no Editor (Gizmos)
    void OnDrawGizmos()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        if (mainCamera == null) return;

        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 10));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 10));
        float mid = (bottomLeft.x + topRight.x) / 2f;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(mid, bottomLeft.y, 0), new Vector3(mid, topRight.y, 0));
    }
}