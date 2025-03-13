using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���������� ������Ʈ
/// </summary>
public class ApplyAttractionObject : MonoBehaviour
{
    Rigidbody2D _rb;
    public List<Attraction> attractionList;
    public float AttractionForce = 100f;
    public float F = 9.81f;

    // ������� ����
    public Vector2 AttractionDirection
    {
        get { return GetAttractionForce(); }
    }

    /// <summary>
    /// ������� ��
    /// </summary>
    /// <returns></returns>
    Vector2 GetAttractionForce()
    {
        if (attractionList == null || attractionList.Count == 0) return Vector2.zero;

        // �߷��� ������ 0�� �Ǿ� ǥ������ �ʵ��� �ϱ� ����
        // �ٸ� �༺�� �ִµ�, �߷��� �� ū �༺�� �������� �ʱ� ����
        // �̸� ���� ���� ����� �༺���� �Ÿ�, ��, ����
        float closetPlanet = 9999999f;
        float closetPlanetForce = 0.0f;
        Vector2 closetPlanetVector = Vector3.zero;

        float objectMass = _rb.mass;
        Vector2 totalForce = Vector2.zero;
        for (int i = 0; i < attractionList.Count; i++)
        {
            float pivotMass = attractionList[i].GetComponent<Rigidbody2D>().mass;                // �༺ ����
            Vector2 distVector = attractionList[i].transform.position - transform.position;      // �༺�� �Ÿ� ����
            float dist = distVector.magnitude;                                                   // �༺���� �Ÿ�
            float force = F * pivotMass * objectMass / Mathf.Pow(dist, 2);                       // �����η�

            // �÷��̾� ����
            if (closetPlanet > dist)
            {
                closetPlanet = dist;
                closetPlanetForce = force;
                closetPlanetVector = distVector;
            }

            totalForce += distVector.normalized * force;                                           // Ÿ�ٿ� ����Ǵ� ��� �� ���ϱ�
        }

        return totalForce.normalized;
    }

    void Awake()
    {
        _rb = transform.GetComponent<Rigidbody2D>();
        attractionList = new List<Attraction>();
    }

    void FixedUpdate()
    {
        // ���� �޴� �������� ���� ����
        _rb.AddForce(AttractionDirection * (AttractionForce * Time.fixedDeltaTime), ForceMode2D.Impulse);
        StandOnGround();
    }

    public void AddGravityField(Attraction gravityField)
    {
        attractionList.Add(gravityField);
    }

    public void RemoveGravityField(Attraction gravityField)
    {
        attractionList.Remove(gravityField);
    }

    void OnDrawGizmos()
    {
        if (AttractionDirection == Vector2.zero)
            return;

        Debug.DrawRay(transform.position, AttractionDirection * 5f, Color.red);
    }
    public void StandOnGround()
    {
        // ���� ���ϱ�
        Vector2 dist = AttractionDirection - (Vector2)transform.up;
        float angle = Mathf.Atan2(dist.y, dist.x) * Mathf.Rad2Deg;

        // ��ü ȸ��
        transform.rotation = Quaternion.Euler(0f, 0f, angle + 90); // z�� ȸ��
    }

    public bool IsInField()
    {
        return (attractionList.Count > 0) ? true : false;
    }
}
