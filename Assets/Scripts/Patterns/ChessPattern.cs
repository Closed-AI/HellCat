using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPattern : Pattern
{
    [SerializeField] private GameObject ChessFirePref;
    [SerializeField] private float Distance;                                                   // должно быть 2, но если будем менять спрайты...
    [SerializeField] private float ArenaLeftPos, ArenaRightPos, ArenaUpPose, ArenaDownPos;     // Крайние точки арены
    private float offset = 0;


    private float waveCount;
    // Start is called before the first frame update
    void Start()
    {
        // зависимость от сложности
        waveCount = difficult;
        StartCoroutine(PatternRule());
        Destroy(gameObject, duration);
    }

    override protected IEnumerator PatternRule()
    {
        for (int i = 0; i < waveCount; i++)
        {
            if (offset == 0)
                offset = Distance;
            else
                offset = 0;
            SpawnChess();

            yield return new WaitForSeconds(duration / waveCount);
        }
    }

    private void SpawnChess()
    {
        for (float i = ArenaDownPos; i <= ArenaUpPose; i+= Distance)
        {
            for (float j = ArenaLeftPos + offset; j <= ArenaRightPos; j += Distance * 2)
            {
                Instantiate(ChessFirePref, new Vector2(j,i), transform.rotation);
            }
            if (offset == 0)
                offset = Distance;
            else
                offset = 0;
        }
    }
}
