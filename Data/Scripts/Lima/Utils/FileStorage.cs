using Sandbox.ModAPI;
using System.IO;
using System;
using VRage.Utils;

namespace Lima
{
  public class FileStorage<T> where T : class, new()
  {
    public string FileName { get; private set; }

    public FileStorage(string fileName)
    {
      FileName = fileName;
    }

    public void Save(T content)
    {
      TextWriter writer = null;
      try
      {
        string stringXML = MyAPIGateway.Utilities.SerializeToXML(content);
        writer = MyAPIGateway.Utilities.WriteFileInLocalStorage(FileName, typeof(FileStorage<T>));
        writer.Write(stringXML);
        writer.Flush();
      }
      catch (Exception e)
      {
        MyLog.Default.WriteLineAndConsole($"{e.Message}\n{e.StackTrace}");
      }
      finally
      {
        if (writer != null)
          writer.Close();
      }
    }

    public T Load()
    {
      if (!MyAPIGateway.Utilities.FileExistsInLocalStorage(FileName, typeof(FileStorage<T>)))
        return null;

      TextReader reader = null;
      T content = null;
      try
      {
        reader = MyAPIGateway.Utilities.ReadFileInLocalStorage(FileName, typeof(FileStorage<T>));
        content = MyAPIGateway.Utilities.SerializeFromXML<T>(reader.ReadToEnd());
      }
      catch (Exception e)
      {
        MyLog.Default.WriteLineAndConsole($"{e.Message}\n{e.StackTrace}");
      }
      finally
      {
        if (reader != null)
          reader.Close();
      }

      return content;
    }
  }
}