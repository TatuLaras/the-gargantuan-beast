using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class FlyPath : MonoBehaviour
{
    int numPoints = 10;
    [SerializeField] Vector3[] points;
    [SerializeField] Transform[] transforms;

    void OnEnable()
    {
        if(transforms != null && transforms.Length > 0)
        {
            foreach(Transform tr in transforms)
            {
                if(tr)
                    DestroyImmediate(tr.gameObject);
            }
        }

        transforms = new Transform[numPoints];
        
        if(points == null || points.Length < 1)
        {
            points = new Vector3[numPoints];

            for (int i = 0; i < numPoints; i++)
            {
                GameObject go = new GameObject("Node " + (i + 1).ToString());
                points[i] = go.transform.position;
                transforms[i] = go.transform;
                go.transform.parent = this.transform;
            }

        } else
        {
            int i = 0;
            foreach(Vector3 point in points)
            {
                i++;
                GameObject go = new GameObject("Node " + (i + 1).ToString());
                go.transform.position = point;
                go.transform.parent = this.transform;
                transforms[i] = go.transform;
            }
        }
    }

    void Update()
    {
        if(points.Length >= numPoints)
        {
            for(int i = 0; i < transforms.Length; i++) {
                points[i] = transforms[i].position;
            }

        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (i == 0)
                continue;

            Gizmos.DrawLine(points[i - 1], points[i]);
        }
    }

}
