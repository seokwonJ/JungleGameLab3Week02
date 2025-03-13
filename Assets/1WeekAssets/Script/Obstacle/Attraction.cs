using UnityEngine;

[RequireComponent(typeof(Collider2D))] // Collider ������Ʈ ������ �ڵ����� �߰�
public abstract class Attraction : MonoBehaviour
{
    CircleCollider2D _collider;
    protected bool isExistPlayer = false; 

    void Start()
    {
        // ���� �ȿ� ������ �� �����ϱ� ����
        _collider = GetComponent<CircleCollider2D>();
        _collider.isTrigger = true;
    }

    /// <summary>
    /// ������� ����
    /// </summary>
    /// <param name="attractableObject"></param>
    /// <returns></returns>
    public abstract Vector3 GetAttractionDirection(ApplyAttractionObject attractableObject);

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out ApplyAttractionObject gravityObject))
        {
            isExistPlayer = true;
            gravityObject.AddGravityField(this);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out ApplyAttractionObject gravityObject))
        {
            gravityObject.RemoveGravityField(this);
            isExistPlayer = false;
        }
    }

    void OnDrawGizmos()
    {
        if (_collider != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _collider.radius);
        }
    }
}
