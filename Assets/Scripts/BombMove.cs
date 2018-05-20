using UnityEngine;

public class BombMove : MonoBehaviour
{
    public float Speed = 10;
    public GameObject BombStandPrefab;
    
    private Vector3 _targetPosition;

    public void SetTargetPosition(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _targetPosition) <= 0.1f)
        {
            Instantiate(BombStandPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Speed * Time.deltaTime);
        //transform.localPosition += transform.forward  * Speed * Time.deltaTime;
    }
}
