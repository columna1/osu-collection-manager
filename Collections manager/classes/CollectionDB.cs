using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections_manager.classes
{
	class CollectionDB
	{
		public static Collection[] Collections = {};
		public static int osuVersion;


		public static void cleanUp()
		{
			Collections = null;
		}
		public static void ReadCollectionDB()
		{
			ReadCollectionDB(Form1.osuFolder+"collection.db");
		}
		public static void ReadCollectionDB(string filename)
		{
			//load a collection at a time into a collection object(just an array) then put that into the collections array
			StreamReader.fileData = File.ReadAllBytes(filename); // filename is local, so is fileData.
			StreamReader.currentPos = 0;

			osuVersion = StreamReader.readInt();
			int numCollections = StreamReader.readInt();
			Collections = new Collection[numCollections];
			for(int i = 0; i < numCollections; i++)
			{
				Collection curCollection = new Collection();
				curCollection.name = StreamReader.readString();
				int numEntries = StreamReader.readInt();
				curCollection.hashes = new string[numEntries];
				curCollection.alivehash = new bool[numEntries];
				if (!curCollection.alive)
					curCollection.alive = true;
				for(int j = 0; j < numEntries; j++)
				{
					curCollection.hashes[j] = StreamReader.readString();
					curCollection.alivehash[j] = true;
				}
				Collections[i] = curCollection;
			}
		}

		public static void setAlive(int collectionID, int mapID,bool set)
		{
			Collections[collectionID].alivehash[mapID] = set;
		}
		public static void setAlive(MapHash tag, bool set)
		{
			Collections[tag.collectionID].alivehash[tag.index] = set;
			tag.alive = set;
		}

		public static void writeCollectionDB(string filename)
		{
			//write version then int for collections
			//then for every collection write string for the name
			//then write int for num of entries
			//then write a hash for every entry
			StreamWriter.writeInt(osuVersion);
			int count = 0;
			for (int i = 0; i < Collections.Length; i++)//loop through all collections to see if they are alive
				if (Collections[i].alive)
					count += 1;
			StreamWriter.writeInt(count);
			for (int i = 0; i < Collections.Length; i++)
			{
				if (Collections[i].alive)
				{
					//write name
					StreamWriter.writeString(Collections[i].name);
					int hcount = 0;
					for (int j = 0; j < Collections[i].hashes.Length; j++)//loop through all hashes to see if they are alive
						if (Collections[i].alivehash[j])
							hcount += 1;
					StreamWriter.writeInt(hcount);

					for (int j = 0; j < Collections[i].hashes.Length; j++)
					{
						if (Collections[i].alivehash[j])
						{
							StreamWriter.writeString(Collections[i].hashes[j]);
						}
					}
				}
			}
			StreamWriter.flushFileR(filename);
		}
	}
}
