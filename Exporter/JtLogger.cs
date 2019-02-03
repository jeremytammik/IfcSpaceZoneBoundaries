﻿using System;
using System.Diagnostics;
using System.IO;

namespace IfcSpaceZoneBoundaries.Exporter
{
  public class JtLogger
  {
    /// <summary>
    /// Singleton instance
    /// </summary>
    static JtLogger _instance = null;

    /// <summary>
    /// Initialised message logging to specific file
    /// </summary>
    /// <param name="path"></param>
    public static void Init( string path )
    {
      _instance = new JtLogger( path );
    }

    /// <summary>
    /// Terminate logging and close output stream
    /// </summary>
    public static void Done()
    {
      _instance.Done2();
      _instance = null;
    }

    /// <summary>
    /// Provide access to the logging functionality
    /// </summary>
    //public static JtLogger Logger
    //{
    //  get { return _instance; }
    //}

    /// <summary>
    /// Add a message to the log file
    /// </summary>
    /// <param name="msg"></param>
    public static void Log( string msg )
    {
      _instance.Log2( msg );
    }


    string _filename;
    StreamWriter _stream;

    /// <summary>
    /// Current log file name
    /// </summary>
    //public string Filename
    //{
    //  get
    //  {
    //    return _filename;
    //  }
    //}

    /// <summary>
    /// Initialise logging and open output stream
    /// </summary>
    JtLogger( string filepath )
    {
      Debug.Assert( null != filepath
        && 0 < filepath.Length,
        "expected valid filename" );

      // Construct log file name from filepath 
      // and try to open file. Project file name is 
      // assumed to be valid (expected to be called 
      // on an open doc).

      _filename = filepath;
      _stream = new StreamWriter( _filename );
      _stream.AutoFlush = true;
      Log( "Log file begin" );
    }

    /// <summary>
    /// Add a log entry
    /// </summary>
    void Log2( string s )
    {
      Debug.Print( s );

      string timestamp = DateTime.Now.ToString(
        "yyyy-MM-dd HH:mm:ss.fff" );

      _stream.WriteLine( timestamp + " " + s );
    }

    /// <summary>
    /// Terminate logging and close output stream
    /// </summary>
    void Done2()
    {
      Log( "The End" );
      _stream.Close();
    }
  }
}
