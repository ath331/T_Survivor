using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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
            string resultString = _ProcessString( key );

            if ( value == "" )
                return "Protocol::" + key + "::" + resultString + "_MAX";
            else
                return "Protocol::" + key + "::" + value.ToUpper();
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

    private static string _ProcessString( string input )
    {
        // 입력 문자열의 길이가 1 이하라면 빈 문자열 반환
        if ( string.IsNullOrEmpty( input ) || input.Length <= 1 )
            return "";

        // 1. 첫 번째 문자 제거
        string str = input.Substring( 1 );

        StringBuilder result = new StringBuilder();
        int uppercaseCount = 0;
        bool inserted = false;

        // 2. 남은 문자열에서 두 번째 대문자 앞에 '_' 삽입
        foreach ( char c in str )
        {
            if ( char.IsUpper( c ) )
            {
                uppercaseCount++;
                if ( uppercaseCount == 2 && !inserted )
                {
                    result.Append( '_' );
                    inserted = true;
                }
            }
            result.Append( c );
        }

        // 3. 전체 문자열을 대문자로 변환하여 반환
        return result.ToString().ToUpper();
    }
}
