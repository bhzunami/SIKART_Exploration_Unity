using UnityEngine;
using System;
using System.Collections;
using System.IO;

public class ParseDB : MonoBehaviour {
    public string FilePath;
    public int NumRowsWithoutHeader;
    public int NumColumns;
    public bool HasHeaderRow;

    float[,] _dataArray;
    bool _dataValid = false;

	// Use this for initialization
	void Start () {
        if (FilePath.Length > 0)
        {
            if (ReadFileToArray(FilePath))
            {
                NumRowsWithoutHeader = _dataArray.GetLength(0);
                NumColumns = _dataArray.GetLength(1);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool GetDataValid()
    {
        return _dataValid;
    }

    public int GetRowCount()
    {
        return _dataArray.GetLength(0);
    }

    public int GetColumnCount()
    {
        return _dataArray.GetLength(1);
    }

    public bool GetColumnMinMaxValues(ref float minVal, ref float maxVal, int col)
    {
        if (_dataValid && (col >= 0) && (col < NumRowsWithoutHeader) )
        {
            minVal = float.MaxValue;
            maxVal = float.MinValue;

            for (int i=0; i<NumRowsWithoutHeader; i++)
            {
                if (_dataArray[i, col] < minVal)
                    minVal = _dataArray[i, col];

                if (_dataArray[i, col] > maxVal)
                    maxVal = _dataArray[i, col];
            }
        }
        return _dataValid;
    }

    public bool GetArrayValue(ref float value, int row, int col)
    {
        if (_dataValid)
            value = _dataArray[row, col];

        return _dataValid;
    }

    bool ReadFileToArray(string fp)
    {
        int lineCount = 0;
        string line;

        _dataValid = false;

        // construct StreamReader for the file given
        System.IO.StreamReader file = new System.IO.StreamReader(Path.GetFullPath(fp));

        // first pass: determine number of lines in source file
        while (file.ReadLine() != null)
            lineCount++;

        int rowCount = lineCount;
        int columnCount = 0;
        int dataLineCount = 0;
        lineCount = 0;

        // reset file stream to beginning for actual reading of data
        file.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);

        while ((line = file.ReadLine()) != null)
        {
            lineCount++;
            if (HasHeaderRow && lineCount <= 1)
            {
                continue;
            }

            char[] delimiterChars = { ' ', ',', ';', ':', '\t' };
            string[] items = line.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries); //prev (string[])null

            if (items.Length > 0)
            {
                if (dataLineCount == 0)
                {
                    columnCount = items.Length;
                    _dataArray = new float[rowCount, columnCount];
                }

                for (int i=0;i<columnCount;i++)
                {
                    _dataArray[dataLineCount, i] = float.Parse(items[i], System.Globalization.NumberStyles.Any);
                }

                dataLineCount++;
            }
        }

        if ((columnCount > 0) && (rowCount > 0))
            _dataValid = true;

        file.Close();

        return _dataValid;
    }

    

}
