using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class JsonMaker : MonoBehaviour
{
    static public void InsertKey( ref string json, string key )
    {
        json += ( "\"" + key + "\"" + " : " );
    }

    static public void InsertValue( ref string json, string key, string value )
    {
        value = ConvertToValueCS( key, ref value );
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

    /* 
     * Data 값은 서버에서 사용하는 자료형들 기준으로 작성되어있으며 
     * 해당 값을 C#에서 사용하는 값으로 변경함
     */
    static public string ConvertToValueCS( string key, ref string value )
    {
        if ( key.StartsWith( "E" ) )
        {
            if ( value == "" )
                return "Protocol::" + key + "::" + key + "Max";
            else
                return "Protocol::" + key + "::" + key + value;
        }
        else
        {
            if ( value == "AtString" )
                return "string";
            else if ( value == "AtInt32" )
                return "int";
            else if ( value == "AtInt64" )
                return "long";
        }

        return value;
    }
}
