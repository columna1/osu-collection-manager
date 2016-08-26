using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections_manager.classes
{
    class ScoreDB
    {
        public static Dictionary<string, Map> Maps = new Dictionary<string, Map> { };//list of maps

        public class Map
        {
            public List<Replay> replays;

            public Map(Replay repl)
            {
                replays = new List<Replay>{ repl };
            }
            
        }
        public static void writeScoresDB()
        {
            //default file
        }
        public static void writeScoresDB(string filename)
        {
            //todo
            StreamWriter.writeInt(CollectionDB.osuVersion);
            StreamWriter.writeInt(Maps.Count);
            int errs = 0;
            foreach (KeyValuePair<string, Map> entry in Maps)
            {
                StreamWriter.writeString(entry.Key);
                StreamWriter.writeInt(entry.Value.replays.Count);

                entry.Value.replays.Sort((x, y) => y.score.CompareTo(x.score));//sort the replays by highest score
                
                for(int i = 0;i < entry.Value.replays.Count; i++)
                {
                    StreamWriter.writeByte(entry.Value.replays[i].mode);
                    StreamWriter.writeInt(   entry.Value.replays[i].version     );
                    StreamWriter.writeString(entry.Value.replays[i].hash        );
                    StreamWriter.writeString(entry.Value.replays[i].player      );
                    StreamWriter.writeString(entry.Value.replays[i].replayhash  );
                    StreamWriter.writeShort( entry.Value.replays[i].num300      );
                    StreamWriter.writeShort( entry.Value.replays[i].num100      );
                    StreamWriter.writeShort( entry.Value.replays[i].num50       );
                    StreamWriter.writeShort( entry.Value.replays[i].numGeki     );
                    StreamWriter.writeShort( entry.Value.replays[i].numKatsu    );
                    StreamWriter.writeShort( entry.Value.replays[i].numMiss     );
                    StreamWriter.writeInt(   entry.Value.replays[i].score       );
                    StreamWriter.writeShort( entry.Value.replays[i].maxCombo    );
                    StreamWriter.writeBool(entry.Value.replays[i].perfectCombo);
                    StreamWriter.writeInt(   entry.Value.replays[i].mods        );
                    StreamWriter.writeString("");//entry.Value.replays[i].empty       );
                    StreamWriter.writeLong(  entry.Value.replays[i].timeStamp   );
                    StreamWriter.writeInt(   -1   );
                    StreamWriter.writeLong(0);//  entry.Value.replays[i].scoreID     );
                }
            }
            StreamWriter.flushFileR(filename);
        }

        public static Replay readReplay(string Filename)
        {
            Replay curReplay = new Replay();
            StreamReader.fileData = File.ReadAllBytes(Filename);
            StreamReader.currentPos = 0;

            curReplay.mode = StreamReader.readByte();
            curReplay.version = StreamReader.readInt();
            curReplay.hash = StreamReader.readString();
            curReplay.player = StreamReader.readString();
            curReplay.replayhash = StreamReader.readString();
            curReplay.num300 = StreamReader.readShort();
            curReplay.num100 = StreamReader.readShort();
            curReplay.num50 = StreamReader.readShort();
            curReplay.numGeki = StreamReader.readShort();
            curReplay.numKatsu = StreamReader.readShort();
            curReplay.numMiss = StreamReader.readShort();
            curReplay.score = StreamReader.readInt();
            curReplay.maxCombo = StreamReader.readShort();
            curReplay.perfectCombo = StreamReader.readBool();
            curReplay.mods = StreamReader.readInt();
            curReplay.graph = StreamReader.readString();
            curReplay.timeStamp = StreamReader.readLong();
            curReplay.length = StreamReader.readInt();
            //curReplay.scoreID = StreamReader.readLong();

            return curReplay;
        }
    }
}
