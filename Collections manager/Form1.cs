using Collections_manager.classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Collections_manager
{
	public partial class Form1 : Form
	{
		public static string osuFolder = OsuDB.getOsuPath();
		bool lastLoadSuccess;
		public static bool CancellationPending = false;

		public Form1()
		{
			InitializeComponent();
		}

		private void loadFiles()
		{
			bool gotFiles = true;
			//pic is osu/data/bt/[setId]l.jpg
			
			if (File.Exists(osuFolder + "collection.db"))
			{
				CollectionDB.ReadCollectionDB();
			}
			else
				gotFiles = false;

			if (File.Exists(osuFolder + "osu!.db"))
			{
				OsuDB.ReadOsuDB(osuFolder + "osu!.db");
			}
			else
				gotFiles = false;

			if (!gotFiles)
			{
				MessageBox.Show("Could not find osu files, please select your osu path manually","Oops!");
				lastLoadSuccess = false;
			} else
				lastLoadSuccess = true;
		}

		private void populateForm()
		{
			//populate tree view

			//make and populate image list
			ImageList iconList = new ImageList();
			iconList.Images.Add(Properties.Resources.rankingXH);
			iconList.Images.Add(Properties.Resources.rankingSH);
			iconList.Images.Add(Properties.Resources.rankingX);
			iconList.Images.Add(Properties.Resources.rankingS);
			iconList.Images.Add(Properties.Resources.rankingA);
			iconList.Images.Add(Properties.Resources.rankingB);
			iconList.Images.Add(Properties.Resources.rankingC);
			iconList.Images.Add(Properties.Resources.rankingD);
			iconList.Images.Add(Properties.Resources.dots);
			//iconList.Images.Add(Properties.Resources.blank);

			treeView1.ImageList = iconList;
			treeView1.ImageIndex = 8;
			treeView1.SelectedImageIndex = 8;

			//for every collection create the child nodes then add them to the collection parent node

			for (int c = 0; c < CollectionDB.Collections.Length; c++)
			{
				List<TreeNode> nodeData = new List<TreeNode>();
				for (int h = 0; h < CollectionDB.Collections[c].hashes.Length; h++)//add child nodes
				{
					if (OsuDB.songExists(CollectionDB.Collections[c].hashes[h]))
					{
						if (OsuDB.getSong(CollectionDB.Collections[c].hashes[h]).gradeStandard < 8)//between 0 and 7?
							nodeData.Add(new TreeNode(OsuDB.getSong(CollectionDB.Collections[c].hashes[h]).artistName + " - " + OsuDB.getSong(CollectionDB.Collections[c].hashes[h]).songTitle + "[" + OsuDB.getSong(CollectionDB.Collections[c].hashes[h]).difficulty + "]", OsuDB.getSong(CollectionDB.Collections[c].hashes[h]).gradeStandard, OsuDB.getSong(CollectionDB.Collections[c].hashes[h]).gradeStandard));
						else
							nodeData.Add(new TreeNode(OsuDB.getSong(CollectionDB.Collections[c].hashes[h]).artistName + " - " + OsuDB.getSong(CollectionDB.Collections[c].hashes[h]).songTitle + "[" + OsuDB.getSong(CollectionDB.Collections[c].hashes[h]).difficulty + "]",8,8));
						nodeData[nodeData.Count - 1].Tag = new MapHash(c, h, CollectionDB.Collections[c].hashes[h], CollectionDB.Collections[c].alivehash[h]);
					}
					//nodeData[nodeData.Count - 1].ImageIndex = OsuDB.getSong(CollectionDB.Collections[c].hashes[h]).gradeStandard;
				}
				//add collection nodes
				TreeNode treenode = new TreeNode(CollectionDB.Collections[c].name, nodeData.ToArray());
				treeView1.Nodes.Add(treenode);
			}
		}

		private void whenDone()
		{
			populateForm();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			idLabel.Text = "ID: ?";
			label1.Visible = false;
			//pic is osu/data/bt/[setId]l.jpg
			idLabel.Links.Add(4,5, "https://osu.ppy.sh/");
			statusLabel.Text = "Loading osu! db...";
			backgroundWorker1.RunWorkerAsync();
			folderBrowserDialog1.SelectedPath = osuFolder;
			//CollectionDB.writeCollectionDB("testcollection.db");
			//Console.WriteLine("Done writing new collection db");
			//Thread loadThread = new Thread(new ThreadStart(loadFiles) => { whenDone(); });
			//populateForm();
		}

		private void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			//TreeNode node = treeView1.SelectedNode;
			//Console.WriteLine(node.ForeColor);
			//MapHash tag = (MapHash)node.Tag;
			//Console.WriteLine(tag.hash);
			//if (node.ForeColor == Color.Red)
			//	node.ForeColor = Color.Black;
			//else
			//	node.ForeColor = Color.Red;
		}

		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{
			//different thing selected? update info box
			TreeNode node = e.Node;
			if (node.Tag != null)
			{
				MapHash tag = (MapHash)node.Tag;
				string hash = tag.hash;
				//Console.WriteLine("Hash is" + hash);
				if (OsuDB.songExists(hash))
				{
					Song cursong = OsuDB.getSong(hash);
					mapperLabel.Text = "Mapped by: " + cursong.creatorName;
					arLabel.Text = "AR: " + cursong.ar.ToString();
					csLabel.Text = "CS: " + cursong.cs.ToString();
					odLabel.Text = "OD: " + cursong.od.ToString();
					idLabel.Text = "ID: " + cursong.beatmapID + " (D)";
					if (cursong.starRating > 0)
					{
						if (cursong.starRating.ToString().Length > 4)
						{
							starsLabel.Text = "Stars: " + cursong.starRating.ToString().Substring(0, 4);
						}
						else
							starsLabel.Text = "Stars: " + cursong.starRating.ToString();
					}
					else
						starsLabel.Text = "Stars: N/A";
					for (int l = idLabel.Links.Count-1; l > -1; l--)
						idLabel.Links.RemoveAt(l);
					idLabel.Links.Add(4, cursong.beatmapID.ToString().Length, "https://osu.ppy.sh/b/" + cursong.beatmapID);
					idLabel.Links.Add(5 + cursong.beatmapID.ToString().Length, 3, "osu://b/" + cursong.beatmapID);
					if (File.Exists(osuFolder + "data\\bt\\" + cursong.beatmapSetID + "l.jpg"))
						pictureBox1.ImageLocation = osuFolder + "data\\bt\\" + cursong.beatmapSetID + "l.jpg";
					else if (File.Exists(osuFolder + "data\\bt\\" + cursong.beatmapSetID + ".jpg"))
						pictureBox1.ImageLocation = osuFolder + "data\\bt\\" + cursong.beatmapSetID + ".jpg";
					else
						pictureBox1.ImageLocation = null;

					//set ranking picturebox
					//Console.WriteLine(cursong.gradeStandard);
					if(cursong.gradeStandard < 8)
						label1.Visible = true;
					switch (cursong.gradeStandard)
					{
						case 0:
							pictureBox2.Image = Properties.Resources.rankingXH;
							break;
						case 1:
							pictureBox2.Image = Properties.Resources.rankingSH;
							break;
						case 2:
							pictureBox2.Image = Properties.Resources.rankingX;
							break;
						case 3:
							pictureBox2.Image = Properties.Resources.rankingS;
							break;
						case 4:
							pictureBox2.Image = Properties.Resources.rankingA;
							break;
						case 5:
							pictureBox2.Image = Properties.Resources.rankingB;
							break;
						case 6:
							pictureBox2.Image = Properties.Resources.rankingC;
							break;
						case 7:
							pictureBox2.Image = Properties.Resources.rankingD;
							break;
						default:
							pictureBox2.Image = null;
							label1.Visible = false;
							pictureBox2.Update();
							break;
					}       

				}
			}
			//Console.WriteLine(node.Tag);
		}

		private void idLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
		}
		private void runWorker()
		{
			if (!backgroundWorker1.IsBusy)
				backgroundWorker1.RunWorkerAsync();
		}

		private void browseButton_Click(object sender, EventArgs e)
		{
			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				bool oldLastSuccess = lastLoadSuccess;
				string oldOsuFolder = osuFolder;
				osuFolder = folderBrowserDialog1.SelectedPath + "\\";
				//Console.WriteLine("Folder selected: " + osuFolder);
				treeView1.Nodes.Clear();
				statusLabel.Text = "Loading new data...";
				OsuDB.cleanUp();
				CollectionDB.cleanUp();
				loadFiles();
				if (!lastLoadSuccess)
				{
					osuFolder = oldOsuFolder;
					lastLoadSuccess = oldLastSuccess;
					MessageBox.Show("Going back to old path", "Reverting");
					statusLabel.Text = "Loading previous data...";
					treeView1.Nodes.Clear();
					runWorker();
				}
				else
				{
					populateForm();
					statusLabel.Text = "Loaded new osu! folder, osu.db has " + OsuDB.Songs.Count + " entries, " + CollectionDB.Collections.Count() + " collections loaded";
				}
				folderBrowserDialog1.SelectedPath = osuFolder;
				//Console.WriteLine(osuFolder);
			}
		}

		private void saveButton_Click(object sender, EventArgs e)
		{
			if (lastLoadSuccess && File.Exists(osuFolder + "collection.db"))
			{
				//copy collection file BEFORE ANYTHING ELSE
				bool backup = true;
				int count = 0;
				while (backup)
				{
					if (!Directory.Exists(osuFolder + "collection backups"))
						Directory.CreateDirectory(osuFolder + "collection backups");
					if (!File.Exists(osuFolder + "collection backups\\collection" + count + ".db"))
					{
						File.Copy(osuFolder + "collection.db", osuFolder + "collection backups\\collection" + count + ".db");
						backup = false;
					} else
						count++;
				}
				//now that it is backed up we can delete the current collection and write our own
				File.Delete(osuFolder + "collection.db");
				CollectionDB.writeCollectionDB(osuFolder + "collection.db");
				MessageBox.Show("Collection succesfully saved and old version backed up as " + osuFolder + "collection backups\\collection" + count + ".db","Saved!");
			}
		}

		private void toggleDeleteState(TreeNode node)
		{
			//mark entry for deletion
			//get song
			if (node.Tag != null)
			{
				MapHash tag = (MapHash)node.Tag;
				string hash = tag.hash;
				if (OsuDB.songExists(hash))
				{
					//flip the alive state
					CollectionDB.setAlive(tag, !tag.alive);
					//set the text to red or black
					if (tag.alive)
						node.ForeColor = Color.Black;
					else
						node.ForeColor = Color.Red;
					//node.ForeColor = 
				}
			}
		}

		private void deleteButton_Click(object sender, EventArgs e)
		{
			//mark entry for deletion
			//get song
			toggleDeleteState(treeView1.SelectedNode);
		}

		private void treeView1_KeyDown(object sender, KeyEventArgs e)
		{
			//Console.WriteLine("---------");
			//Console.WriteLine(e.KeyValue);
			switch (e.KeyValue)
			{
				case 46:
					toggleDeleteState(treeView1.SelectedNode);
					break;
				case 8:
					toggleDeleteState(treeView1.SelectedNode);
					break;
				case 116://F5 refresh
					OsuDB.cleanUp();
					CollectionDB.cleanUp();
					treeView1.Nodes.Clear();
					statusLabel.Text = "Refreshing Data...";
					runWorker();
					break;
				default:
					break;
			}
		}

		private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{
			loadFiles();
		}

		private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			statusLabel.Text = "Loaded " + OsuDB.Songs.Count + " entries from osu!.db and " + CollectionDB.Collections.Count() + " collections";
			populateForm();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AboutBox1 box = new AboutBox1();
			box.ShowDialog();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void forumPostToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			//open link eventually
			MessageBox.Show("Not released yet!","Not yet!");
		}
	}
}
