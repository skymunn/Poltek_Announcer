﻿using System;
using System.Drawing;
using System.Windows.Forms;
using PoltekAnnouncer.Constant;

namespace PoltekAnnouncer.Assets
{
    public partial class Launcher
    {
        // Shared Function
        private ControllerName CName = new ControllerName();
        private OpenDialog _OpenDialog = new OpenDialog();
        private DataPayload _DataPayload = new DataPayload();
        private Viewer viewer = new Viewer();
        private Startup startup = new Startup();

		/// <summary>Shorthand for OpenDialog</summary>
		private class OpenDialog
        {
			/// <summary>Get the file after execution.</summary>
            public string PathFile { get; private set; }

			/// <summary>Open the dialog</summary>
            public void Open(string filter)
            {
                var p = new Launcher();
                using (OpenFileDialog file = p.FileDialog)
                {
                    file.InitialDirectory = @"C:\";
                    file.Filter = filter;
                    file.RestoreDirectory = true;
                    file.Title = "Pilih File";

                    if (file.ShowDialog() == DialogResult.OK)
                        PathFile = file.FileName;
                }
            }
        }

        /// <summary>Get the limit of the page.</summary>
        public int CountLimit
        {
            get { return TabControl.TabPages.Count; }
        }

		// Local Function
        /// <summary>Shorthand for adding pages in this project</summary>
        /// <param name="pagination">Index of page.</param>
        /// <param name="choose">The choosen controller.</param>
        /// <param name="path">Optional, for Image.</param>
        public void AddPages(int pagination, AddPagesEnum choose, string path = "", string value ="")
        {
            TabPage tabpage = new TabPage
            {
                Name = "page" + pagination,
                Text = "Halaman " + pagination
            };

            int imgX = 300;
            int imgY = 175;

            string nameTxt = CName.keyText + pagination;
            string nameImg = CName.keyImg + pagination;
            string nameLoadImg = CName.keyLoadImg + pagination;
            string nameDelBtn = CName.keyDelBtn + pagination;

            switch (choose)
            {
                case AddPagesEnum.Text:
                    TextBox textbox = new TextBox
                    {
                        Name = nameTxt,
                        ScrollBars = ScrollBars.Both,
                        Multiline = true,
                        WordWrap = false,
                        Size = new Size(imgX, imgY),
                        Location = new Point(6, 4),
                        Dock = DockStyle.Top,
                        Text = value
                    };
                    tabpage.Controls.Add(textbox);

                    break;

                case AddPagesEnum.Image:
                    PictureBox picture = new PictureBox
                    {
						Name = nameImg,
                        ImageLocation = path,
						Location = new Point(6, 4),
						Size = new Size(imgX, imgY),
						SizeMode = PictureBoxSizeMode.Zoom,
                        Dock = DockStyle.Top
                    };
                    tabpage.Controls.Add(picture);

                    Button buttonLoadImg = new Button
                    {
                        Name = nameLoadImg,
                        Location = new Point(6, 4),
                        Size = new Size(imgX, imgY),
                        Text = "Muat Gambar",
						Visible = false,
                        Dock = DockStyle.Top
                    };
                    buttonLoadImg.Click += ButtonLoadImg_Click;
                    tabpage.Controls.Add(buttonLoadImg);
                    break;
            }

            Button buttonDel = new Button
            {
                Name = nameDelBtn,
                Text = "Hapus",
                Location = new Point(5, 173),
                Size = new Size(302, 20),
                Dock = DockStyle.Bottom
            };
            buttonDel.Click += ButtonDeletePage_Click;
            tabpage.Controls.Add(buttonDel);

            TabControl.TabPages.Add(tabpage);
            TabControl.SelectTab(tabpage);

			// Yang di bawah ini untuk ngelawak, ga lebih ga kurang...
            PageJustice.Text = pagination.ToString();
        }

        /// <summary>Shorthand for enuming in AddPage.</summary>
        public enum AddPagesEnum
        {
            Text, Image
        }

        /// <summary>Shorthand for all clicking Button in tab.</summary>
        private void ButtonDeletePage_Click(object sender, EventArgs e)
        {
            TabPage selected = TabControl.SelectedTab;
            int index = TabControl.TabPages.IndexOf(selected);
            int justice = int.Parse(PageJustice.Text);

            string keyText = CName.keyText + index.ToString();
            string keyImg = CName.keyImg + index.ToString();
            string keyLoadImg = CName.keyLoadImg + index.ToString();

            // searching delete logic
            if (0 < index && index < justice)
            {
                if (selected.Controls.ContainsKey(keyText))
                {
                    TextBox text = (TextBox)selected.Controls[keyText];
                    text.Text = "";
                }
				else if (selected.Controls.ContainsKey(keyImg))
                {
                    PictureBox pict = (PictureBox)selected.Controls[keyImg];
                    Button btnLoadImg = (Button)selected.Controls[keyLoadImg];

                    pict.ImageLocation = string.Empty;
                    pict.Visible = false;
                    btnLoadImg.Visible = true;
                }
            }
            else
            {
                TabControl.TabPages.Remove(selected);
                PageJustice.Text = (int.Parse(PageJustice.Text) - 1).ToString();
            }
        }

		/// <summary>Shorthand for all ButtonLoadImg</summary>
		private void ButtonLoadImg_Click(object sender, EventArgs e)
        {
            TabPage selected = TabControl.SelectedTab;
            int index = TabControl.TabPages.IndexOf(selected);
            string keyImg = CName.keyImg + index.ToString();
            string keyLoadImg = CName.keyLoadImg + index.ToString();

            PictureBox pict = (PictureBox)selected.Controls[keyImg];
            Button btnLoadImg = (Button)selected.Controls[keyLoadImg];

            _OpenDialog.Open("File foto yang umum (*.png, *.jpg)|*.PNG;*.JPG");
            pict.Visible = true;
            pict.ImageLocation = _OpenDialog.PathFile;
            btnLoadImg.Visible = false;
        }

        private void DebugLevelExtend(string info)
        {
            DebugLabel.Text = "Info: " + info;
        }
    }
}
