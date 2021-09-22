using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPattern : Pattern
{
    [SerializeField] private GameObject ChessFirePref;
    [SerializeField] private float Size;                                                       // Размер клеток доски
    [SerializeField] private float ArenaLeftPos, ArenaRightPos, ArenaUpPose, ArenaDownPos;     // Крайние точки арены
    private float offset = 0;


    private float waveCount;
    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        // зависимость от сложности
        waveCount = difficult;
        speed = (waveCount / duration);
        ChessFirePref.transform.localScale = new Vector3(Size / 2f, Size / 2f, 0);
        StartCoroutine(PatternRule());
        Destroy(gameObject, duration);
    }

    override protected IEnumerator PatternRule()
    {
        for (int i = 0; i < waveCount; i++)
        {
            if (offset == 0)
                offset = Size;
            else
                offset = 0;
            SpawnChess();

            yield return new WaitForSeconds(duration / waveCount);
        }
    }

    private void SpawnChess()
    {
        float localOffset = offset;
        GameObject ChessFieldPart;

        for (float i = ArenaDownPos; i <= ArenaUpPose + Size/2f; i+= Size)
        {
            for (float j = ArenaLeftPos + localOffset; j <= ArenaRightPos + Size / 2f; j += Size * 2) 
            {
                ChessFieldPart = Instantiate(ChessFirePref, new Vector2(j, i), transform.rotation);
                ChessFieldPart.GetComponent<Animator>().SetFloat("Speed", speed);
                Destroy(ChessFieldPart.gameObject, 1f / speed);
            }
            if (localOffset == 0)
                localOffset = Size;
            else
                localOffset = 0;
        }

        
    }
}
