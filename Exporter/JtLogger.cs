using System;
using System.Diagnostics;
using System.IO;

namespace IfcSpaceZoneBoundaries.Exporter
{
  public class JtLogger
  {
    string _filename;
    StreamWriter _stream;

    /// <summary>
    /// Current log file name
    /// </summary>
    public string Filename
    {
      get
      {
        return _filename;
      }
    }

    /// <summary>
    /// Initialise logging and open output stream
    /// </summary>
    public bool Init( string filepath )
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
      return true;
    }

    /// <summary>
    /// Add a log entry
    /// </summary>
    /// <param name="s"></param>
    public void Log( string s )
    {
      Debug.Print( s );

      string timestamp = DateTime.Now.ToString(
        "yyyy-MM-dd HH:mm:ss.fff" );

      _stream.WriteLine( timestamp + " " + s );
    }

    /// <summary>
    /// Terminate logging and close output stream
    /// </summary>
    public void Done()
    {
      Log( "The End" );
      _stream.Close();
    }
  }
}
