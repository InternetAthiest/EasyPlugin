using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace EasyPlugin
{
    public class EasyPlugin
    {
		/// <summary>
		/// Load the plugins.
		/// </summary>
		/// <returns>Plugins that were loaded.</returns>
		/// <param name="PluginFolderPath">Plugin folder path.</param>
		/// <typeparam name="PluginInterface">The plugin interface.</typeparam>
		public static PluginInterface[] LoadPlugins<PluginInterface>(string PluginFolderPath)
        {
			//Output list for all of the plugin classes
			List<PluginInterface> output = new List<PluginInterface> ();

            //Get all file names that has the file type .dll.
            string[] dllFileNames = Directory.GetFiles(PluginFolderPath, "*.dll");

            //Load all dll files
            foreach (string dllFile in dllFileNames)
            {
                Assembly.Load(AssemblyName.GetAssemblyName(dllFile));
            }

			//Try to load the plugin in a for loop
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type t in a.GetTypes())
                {
					//Try the class if its not null and if it's the same type, saving time because you're not erroring
					if (t.GetInterface(typeof(PluginInterface).Name) == typeof(PluginInterface) && t.GetInterface(typeof(PluginInterface).Name) != null)
                    {
                        try
						{
							//Adds the main plugin class to the list.
							output.Add((PluginInterface)Activator.CreateInstance(t));
                        }
                        catch
                        {
                        }
                    }
                }
            }
			//Return the output.
			return output.ToArray ();
        }
    }
}
