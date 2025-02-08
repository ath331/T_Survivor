using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonMaker : MonoBehaviour
{
    static public void InsertKey( ref string json, string key )
    {
        json += ( "\"" + key + "\"" + " : " );
    }

    static public void InsertValue( ref string json, string value )
    {
        json += "\"" + value + "\"";
    }

    static public void ConnectStr( ref string json, string value )
    {
        json += value;
    }

    static public void InsertBeginBrace( ref string json )
    {
        json += "{ ";
    }

    static public void InsertEndBrace( ref string json )
    {
        json += "}";
    }

    static public void InsertNewLine( ref string json )
    {
        json += "\n";
    }

    static public void InsertRest( ref string json )
    {
        json += ",";
    }

    static public void InsertSpace( ref string json, short count = 4 )
    {
        for( int i = 0; i < count; i++ )
            json += " ";
    }
}
