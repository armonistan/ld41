using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour {
    public SpriteRenderer Endzone;
    public BoxCollider2D EdgePrefab;
    public int YardsPerSection;

    public PlanExecutor Plan;

    private bool _initialized;
    private SpriteRenderer _field;
    private SpriteRenderer _instanciatedEndzone;
    private BoxCollider2D _instanciatedLeftEdge;
    private BoxCollider2D _instanciatedRightEdge;
    private BoxCollider2D _instanciatedBottomEdge;

    public float YardLength
    {
        get
        {
            return _field.bounds.size.y / YardsPerSection / _numberOfSections;
        }
    }

    private int _numberOfSections
    {
        get
        {
            return Mathf.CeilToInt(Plan.YardageGoal / (float)YardsPerSection);
        }
    }

	// Use this for initialization
	void Start () {
        _field = GetComponent<SpriteRenderer>();
        _field.size = new Vector2(_field.size.x, 1.2f * _numberOfSections);

        _instanciatedEndzone = Instantiate(Endzone, Vector3.zero, Quaternion.identity);

        _instanciatedLeftEdge = Instantiate(EdgePrefab, Vector3.zero, Quaternion.identity);
        _instanciatedRightEdge = Instantiate(EdgePrefab, Vector3.zero, Quaternion.identity);
        _instanciatedBottomEdge = Instantiate(EdgePrefab, Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(0, _field.bounds.size.y / 2, 5);

        _instanciatedEndzone.transform.position = new Vector3(0, _field.bounds.size.y + Endzone.bounds.size.y / 2, 5);

        _instanciatedLeftEdge.transform.position = new Vector3((_field.bounds.size.x / 2 + 0.5f) * -1, transform.position.y);
        _instanciatedLeftEdge.size = new Vector2(1,  _field.bounds.size.y);

        _instanciatedRightEdge.transform.position = new Vector3(_field.bounds.size.x / 2 + 0.5f, transform.position.y);
        _instanciatedRightEdge.size = new Vector2(1, _field.bounds.size.y);

        _instanciatedBottomEdge.transform.position = new Vector3(0, -0.5f);
        _instanciatedBottomEdge.size = new Vector2(_field.bounds.size.x, 1);
    }
}
