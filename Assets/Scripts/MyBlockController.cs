using System.Collections;
using UnityEngine;
using System.Collections.Generic;



public class MyBlockController : MonoBehaviour
{
    public List<Transform> ListPiece => listPiece;
    [SerializeField]private List<Transform> listPiece = new List<Transform>();
    void Start()
    {
       
        StartCoroutine(MoveDown());
        delay = GameManager.Instance.GameSpeed;

    }

    float delay;
    IEnumerator MoveDown()
    {
        while (true)
        {

            //Debug.Log(delay);
            yield return new WaitForSeconds(delay);
            delay -= 5f * Time.deltaTime;
            var isMoveble = GameManager.Instance.IsInside(GetPreviewPosition());
            if (isMoveble)
           
            Move();
            else
            {
                foreach (var piece in listPiece)
                {
                    int x =Mathf.RoundToInt(piece.position.x);
                    int y =Mathf.RoundToInt(piece.position.y);
                    GameManager.Instance.Grid[x, y] = true;
                }
                GameManager.Instance.UpdateRemoveObjectController();
                GameManager.Instance.Spawn();
                //new object spawn
                break;
            }

          
        }

    }
    private List<Vector2> GetPreviewPosition()
    {
        var result = new List<Vector2>();
        foreach (var piece in listPiece)
        {
            var position = piece.position;
            position.y--;
            result.Add(position);
            
        }
        return result;
    }


    private void Move()
    {
        var position = transform.position;
        position.y--;
        transform.position = position;
    }
}

