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
     * Data ���� �������� ����ϴ� �ڷ����� �������� �ۼ��Ǿ������� 
     * �ش� ���� C#���� ����ϴ� ������ ������
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
