using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections_manager.classes
{
	class DebugHelper
	{
		public static void output (string data)
		{
			File.AppendAllText("DEBUG.txt", "@" + StreamReader.currentPos + " - " + data + "\n");
		}
	}
}
