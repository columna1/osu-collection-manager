using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections_manager.classes
{
	class Song
	{

		public string artistName;
		public string artistNameUnicode;
		public string songTitle;
		public string songTitleUnicode;
		public string creatorName;
		public string difficulty;
		public string audioFileName;
		public string md5Hash;//used for the dictionary as well
		public string osuFilename;
		public byte   ranked;
		public short  hitcircles;
		public short  sliders;
		public short  spinners;
		public long   lastModTime;//in windows ticks
		public float  ar;
		public float  cs;
		public float  hp;
		public float  od;
		public double sliderVelocity;
		//int double pair things for star ratings
		//public standard
		//public taiko
		//public ctb
		//public mania
		public int    drainTime;//in seconds
		public int    totalTime;//in miliseconds
		public int    previewTime;//in miliseconds
							   // //list of timing points
		public int    beatmapID;
		public int    beatmapSetID;
		public int    threadID;
		public byte   gradeStandard;
		public byte   gradeTaiko;
		public byte   gradeCTB;
		public byte   gradeMania;
		public float  localOffset;
		public float  stackLeniency;
		public byte   mode;
		public string songSource;
		public string songTags;
		public float  onlineOffset;
		public string titleFont;
		public bool   unplayed;
		public long   lastPlayed;
		public bool   isOsz2;
		public string folderName; //realative to songs folder
		public long   lastChecked;
		public bool   ignoreBeatmapSounds;
		public bool   ignoreBeatmapSkin;
		public bool   disableStoryboard;
		public bool   disableVideo;
		public bool   visualOveride;
		//if less than 20140609 then random unknown float
		public int    lastModifyTime;//???
		public byte   maniaScrollSpeed;

		public double starRating;



		//public Song(string ArtistName, string ArtistNameUnicode, string SongTitle,string SongTitleUnicode, string CreatorName, string Difficulty, string AudioFileName, string Md5Hash, string OsuFilename, byte Ranked, short Hitcircles, short Sliders, short Spinners, long LastModTime, float Ar, float Cs, float Hp, float Od, double SliderVelocity, int DrainTime, int TotalTime, int PreviewTime, int BeatmapID, int BeatmapSetID, int ThreadID, byte GradeStandard, byte GradeTaiko, byte GradeCTB, byte GradeMania, float LocalOffset, float StackLeniency, byte Mode, string SongSource, string SongTags, float OnlineOffset, string TitleFont, bool Unplayed, long LastPlayed, bool IsOsz2, string FolderName,  long LastChecked, bool IgnoreBeatmapSounds, bool IgnoreBeatmapSkin, bool DisableStoryboard, bool DisableVideo, bool VisualOveride, int LastModifyTime, byte ManiaScrollSpeed)
		//{
		//	artistName = ArtistName                  ;
		//	artistNameUnicode = ArtistNameUnicode	 ;
		//	songTitle = SongTitle					 ;
		//	songTitleUnicode = SongTitleUnicode		 ;
		//	creatorName = CreatorName				 ;
		//	difficulty = Difficulty					 ;
		//	audioFileName = AudioFileName			 ;
		//	md5Hash = Md5Hash						 ;
		//	osuFilename = OsuFilename				 ;
		//	ranked = Ranked							 ;
		//	hitcircles = Hitcircles					 ;
		//	sliders = Sliders						 ;
		//	spinners = Spinners						 ;
		//	lastModTime = LastModTime				 ;
		//	ar = Ar									 ;
		//	cs = Cs									 ;
		//	hp = Hp									 ;
		//	od = Od									 ;
		//	sliderVelocity = SliderVelocity			 ;
		//	drainTime = DrainTime					 ;
		//	totalTime = TotalTime					 ;
		//	previewTime = PreviewTime				 ;
		//	beatmapID = BeatmapID					 ;
		//	beatmapSetID = BeatmapSetID				 ;
		//	threadID = ThreadID						 ;
		//	gradeStandard = GradeStandard			 ;
		//	gradeTaiko = GradeTaiko					 ;
		//	gradeCTB = GradeCTB						 ;
		//	gradeMania = GradeMania					 ;
		//	localOffset = LocalOffset				 ;
		//	stackLeniency = StackLeniency			 ;
		//	mode = Mode								 ;
		//	songSource = SongSource					 ;
		//	songTags = SongTags						 ;
		//	onlineOffset = OnlineOffset				 ;
		//	titleFont = TitleFont					 ;
		//	unplayed = Unplayed						 ;
		//	lastPlayed = LastPlayed					 ;
		//	isOsz2 = IsOsz2							 ;
		//	folderName = FolderName					 ;
		//	lastChecked = LastChecked				 ;
		//	ignoreBeatmapSounds = IgnoreBeatmapSounds;
		//	ignoreBeatmapSkin = IgnoreBeatmapSkin	 ;
		//	disableStoryboard = DisableStoryboard	 ;
		//	disableVideo = DisableVideo				 ;
		//	visualOveride = VisualOveride			 ;
		//	lastModifyTime = LastModifyTime			 ;
		//	maniaScrollSpeed = ManiaScrollSpeed      ;
		//}



	}
}
