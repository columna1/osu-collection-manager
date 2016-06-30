using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections_manager.classes
{
	class OsuDB
	{
		public static Dictionary<string, Song> Songs;

		public static void cleanUp()
		{
			Songs = null;
		}

		public static Song getSong(string md5hash)
		{
			if (Songs == null)
			{
				// Read the DB.
			}
			else
			{
				if (Songs.ContainsKey(md5hash))
				{
					// Dictionary has it.
					return Songs[md5hash]; // Boom.
				}
				else
				{
					return null; // Or somethin.
				}
			}
			return null;
		}
		public static bool songExists(string song)
		{
			return Songs.ContainsKey(song);
		}

		public static string getOsuPath()
		{
			using (RegistryKey osureg = Registry.ClassesRoot.OpenSubKey("osu\\DefaultIcon"))
			{
				if (osureg != null)
				{
					string osukey = osureg.GetValue(null).ToString();
					string osupath;
					osupath = osukey.Remove(0, 1);
					osupath = osupath.Remove(osupath.Length - 11);
					return osupath;
				}
			}
			return "err";
		}

		public static void ReadOsuDB()
		{
			ReadOsuDB(getOsuPath() + "osu!.db");
		}

		public static void ReadOsuDB(string filename) // Local argument stays local.
		{


			StreamReader.fileData = File.ReadAllBytes(filename); // filename is local, so is fileData.
			StreamReader.currentPos = 0;

			int osuVersion = StreamReader.readInt();
			int folderCount = StreamReader.readInt();
			bool accountUnlocked = StreamReader.readBool();
			long dateTime = StreamReader.readLong();
			string username = StreamReader.readString();
			int numBeatmaps = StreamReader.readInt();

			//Console.WriteLine(osuVersion);

			for (int i = 0 ; i < numBeatmaps; i++)
			{
				Song currentSong = new Song();

				//if (i >= 10)
				//{
				//	break; // For debug stuff.
				//}

				int entrySize = StreamReader.readInt();
				currentSong.artistName = StreamReader.readString();
				currentSong.artistNameUnicode = StreamReader.readString();
				currentSong.songTitle = StreamReader.readString();
				currentSong.songTitleUnicode = StreamReader.readString();
				currentSong.creatorName = StreamReader.readString();
				currentSong.difficulty = StreamReader.readString();
				currentSong.audioFileName = StreamReader.readString();
				currentSong.md5Hash = StreamReader.readString();//used for the dictionary as well
				//DebugHelper.output("CSONG - " + currentSong.md5Hash);
				currentSong.osuFilename = StreamReader.readString();
				currentSong.ranked = StreamReader.readByte();
				currentSong.hitcircles = StreamReader.readShort();
				currentSong.sliders = StreamReader.readShort();
				currentSong.spinners = StreamReader.readShort();
				currentSong.lastModTime = StreamReader.readLong();//in windows ticks
				currentSong.ar = StreamReader.readFloat();
				currentSong.cs = StreamReader.readFloat();
				currentSong.hp = StreamReader.readFloat();
				currentSong.od = StreamReader.readFloat();
				currentSong.sliderVelocity = StreamReader.readDouble();
				//DebugHelper.output("Slider velocity");
				//read int-Double Pairs 
				//read int representing the ammount of int-double pairs then read the int-double pairs

				int length = StreamReader.readInt();
                                                     //DebugHelper.output("Numss is " + length);
                bool gotStar = false;
				for (int j = 0; j < length; j++)
				{
					if (StreamReader.readByte() == 8)
					{
						if (StreamReader.readInt() == 0)
						{
							StreamReader.readByte();
							currentSong.starRating = StreamReader.readDouble();
                            gotStar = true;
						}
						else
						{
							StreamReader.readByte();
							StreamReader.readDouble();
						}
					}
					else
					{
						//Console.WriteLine("misaligned or something");
						throw (new Exception("could not read osu.db(int-double standard)"));
					}
				}
                if (!gotStar)
                    currentSong.starRating = -1;
				length = StreamReader.readInt();
				for (int j = 0; j < length; j++)
				{
					if (StreamReader.readByte() == 0x08)
					{
						StreamReader.readInt();
						StreamReader.readByte();
						StreamReader.readDouble();
					}
					else
					{
						throw (new Exception("could not read osu.db(int-double taiko)"));
					}
				}
				length = StreamReader.readInt();
				for (int j = 0; j < length; j++)
				{
					if (StreamReader.readByte() == 0x08)
					{
						StreamReader.readInt();
						StreamReader.readByte();
						StreamReader.readDouble();
					}
					else
					{
						throw (new Exception("could not read osu.db(int-double ctb)"));
					}
				}
				length = StreamReader.readInt();
				for (int j = 0; j < length; j++)
				{
					if (StreamReader.readByte() == 0x08)
					{
						StreamReader.readInt();
						StreamReader.readByte();
						StreamReader.readDouble();
					}
					else
					{
						throw (new Exception("could not read osu.db(int-double mania)"));
					}
				}
				currentSong.drainTime = StreamReader.readInt();//in seconds
				currentSong.totalTime = StreamReader.readInt();//in miliseconds
				currentSong.previewTime = StreamReader.readInt();//in miliseconds
																 //timing points
				length = StreamReader.readInt();
				for (int j = 0; j < length; j++)
				{
					StreamReader.readDouble();//bpm
					StreamReader.readDouble();//offset in ms
					StreamReader.readBool();//inherited
				}

				currentSong.beatmapID = StreamReader.readInt();
				currentSong.beatmapSetID = StreamReader.readInt();
				currentSong.threadID = StreamReader.readInt();
				currentSong.gradeStandard = StreamReader.readByte();
				currentSong.gradeTaiko = StreamReader.readByte();
				currentSong.gradeCTB = StreamReader.readByte();
				currentSong.gradeMania = StreamReader.readByte();
				currentSong.localOffset = StreamReader.readShort();
				currentSong.stackLeniency = StreamReader.readFloat();
				currentSong.mode = StreamReader.readByte();
				currentSong.songSource = StreamReader.readString();
				currentSong.songTags = StreamReader.readString();
				currentSong.onlineOffset = StreamReader.readShort();
				currentSong.titleFont = StreamReader.readString();
				//DebugHelper.output("Title font = " + currentSong.titleFont);

				// Shit happens here.
				currentSong.unplayed = StreamReader.readBool();//troublemaker?
				//DebugHelper.output("Unplayed = " + currentSong.unplayed);
				currentSong.lastPlayed = StreamReader.readLong(); // Error was here: readShort instead of readLong. Mismatch between C# and Lua.
				//DebugHelper.output("lastplayed = " + currentSong.lastPlayed);
				currentSong.isOsz2 = StreamReader.readBool();
				//DebugHelper.output("isOsz2 = " + currentSong.isOsz2);
				currentSong.folderName = StreamReader.readString(); //realative to songs folder
				//DebugHelper.output("folderName = " + currentSong.folderName);
				currentSong.lastChecked = StreamReader.readLong();
				currentSong.ignoreBeatmapSounds = StreamReader.readBool();
				currentSong.ignoreBeatmapSkin = StreamReader.readBool();
				currentSong.disableStoryboard = StreamReader.readBool();
				currentSong.disableVideo = StreamReader.readBool();
				currentSong.visualOveride = StreamReader.readBool();
				currentSong.lastModifyTime = StreamReader.readInt(); //???
				currentSong.maniaScrollSpeed = StreamReader.readByte();
				if (Songs == null)
				{
					Songs = new Dictionary<string, Song>();
				}
				if (!Songs.ContainsKey(currentSong.md5Hash))
					//Console.WriteLine("osu u dun fukd up(duplicate entry) hash: " + currentSong.md5Hash + " Title: " + currentSong.songTitle);
				//else
					Songs.Add(currentSong.md5Hash, currentSong);
				//if (i == 257)
				//Console.WriteLine();
				//break;
				if (Form1.CancellationPending)
				{
					Console.WriteLine("Canceled");
					break;
				}else
				{
					//Console.WriteLine("Running");
				}
			}
			//int currentPosition = 0;
		}
	}
}

