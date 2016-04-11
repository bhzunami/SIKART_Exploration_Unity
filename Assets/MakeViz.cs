using UnityEngine;
using System.Collections;

public class MakeViz : MonoBehaviour {
    public float InstanceScale = 1.0f;

    public int DataColumnX = 0;
    public int DataColumnY = 1;
    public int DataColumnZ = 2;
    public int DataColumnColour = 3;

    public float PosScaleX = 10.0f;
    public float PosScaleY = 10.0f;
    public float PosScaleZ = 10.0f;

    // These cache the corresponding public dataColumn fields
    // so that we can detect changes made in Editor in Update()
    int _dcX, _dcY, _dcZ, _dcColour;

    ArrayList _objectArray;
    ParseDB _dataSource;

	// Use this for initialization
	void Start () {
        _dataSource = GetComponent<ParseDB>();
        if (_dataSource != null)
        {
            make3DCloud();
            update3DCloud(DataColumnX, DataColumnY, DataColumnZ,DataColumnColour);
        }
	}
	
	// Update is called once per frame
	void Update () {
        // Update coloumn assignment if one or more fields have changed
	    if ( (DataColumnX != _dcX) || (DataColumnY != _dcY) || (DataColumnZ != _dcZ) || (DataColumnColour != _dcColour) )
            update3DCloud(DataColumnX, DataColumnY, DataColumnZ, DataColumnColour);
    }

    void update3DCloud(int colX, int colY, int colZ,int colColour)
    {
        _dcX = colX;
        _dcY = colY;
        _dcZ = colZ;
        _dcColour = colColour;

        float minX, maxX, minY, maxY, minZ, maxZ, minColour, maxColour, scaleX, scaleY, scaleZ, scaleColour;
        minX = maxX = minY = maxY = minZ = maxZ = minColour = maxColour = 0.0f;
        scaleX = scaleY = scaleZ = scaleColour = 1.0f;

        if (_dataSource.GetDataValid())
        {
            _dataSource.GetColumnMinMaxValues(ref minX, ref maxX, colX);
            _dataSource.GetColumnMinMaxValues(ref minY, ref maxY, colY);
            _dataSource.GetColumnMinMaxValues(ref minZ, ref maxZ, colZ);
            _dataSource.GetColumnMinMaxValues(ref minColour, ref maxColour, colColour);

            scaleX = 1.0f / (maxX - minX);
            scaleY = 1.0f / (maxY - minY);
            scaleZ = 1.0f / (maxZ - minZ);
            scaleColour = 1.0f / (maxColour - minColour);

            // Update position and colour of the 3D cloud objects
            for (int i = 0; i < _dataSource.NumRowsWithoutHeader; i++)
            {
                float newPosX = 0.0f;
                float newPosY = 0.0f;
                float newPosZ = 0.0f;
                float newColour = 0.0f;
                _dataSource.GetArrayValue(ref newPosX, i, colX);
                newPosX = (newPosX - minX) * scaleX * PosScaleX;
                _dataSource.GetArrayValue(ref newPosY, i, colY);
                newPosY = (newPosY - minY) * scaleY * PosScaleY;
                _dataSource.GetArrayValue(ref newPosZ, i, colZ);
                newPosZ = (newPosZ - minZ) * scaleZ * PosScaleZ;
                _dataSource.GetArrayValue(ref newColour, i, colColour);
                newColour = (newColour - minColour) * scaleColour;

                // Compute RGB colour by interpolating between red and green
                Color newRGB;
                newRGB.r = 1.0f - newColour;
                newRGB.g = newColour;
                newRGB.b = 0.0f;
                newRGB.a = 1.0f;

                GameObject obj = (GameObject)_objectArray[i];

                obj.transform.position = new Vector3(newPosX, newPosY, newPosZ);
                obj.transform.localScale = new Vector3(InstanceScale, InstanceScale, InstanceScale);

                // Set colour of object
                MeshRenderer rdr = obj.GetComponent<MeshRenderer>();
                if (rdr != null)
                    rdr.material.color = newRGB;
            }
        }
    }

    // construct a point cloud from database entries
    void make3DCloud ()
    {
        int rowCount = _dataSource.GetRowCount();
        _objectArray = new ArrayList();

        for (int i=0;i<rowCount;i++)
        {
            GameObject newObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            string objectName = string.Format("Instance_{0}", i);
            newObject.name = objectName;
            newObject.transform.parent = gameObject.transform;
            _objectArray.Add(newObject);
        }
    }

}
