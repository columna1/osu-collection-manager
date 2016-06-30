using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections_manager.classes
{
	class Collection
	{
		public string name;
		public bool alive = true;//will be turned to false if this is to be deleted/removed
		public string[] hashes;
		public bool[] alivehash;//will be set to false if a certain entry is to be removed
	}
	class MapHash//will be used in the tags for the tableview
	{
		public int collectionID;
		public int index;
		public string hash;
		public bool alive;
		public MapHash(int collectionid,int entryindex, string songHash, bool isAlive)
		{
			collectionID = collectionid;
			index = entryindex;
			hash = songHash;
			alive = isAlive;
		}
	}
}
