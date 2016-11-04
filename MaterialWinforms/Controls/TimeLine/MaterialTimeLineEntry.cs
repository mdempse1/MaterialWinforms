﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MaterialWinforms.Controls
{
    public partial class MaterialTimeLineEntry : MaterialUserControl
    {

        private String _UserName;
        private Graphics StringGraphics;
        public String UserName
        {
            get { return _UserName; }
            set
            {
                _UserName = value;
                UserImage.AvatarLetter = _UserName.Substring(0, 1);
                UserNameLabel.Text = _UserName;
            }
        }
        private Image _User;
        public Image User
        {
            get { return _User; }
            set
            {
                _User = value;
                UserImage.Avatar = value;
            }
        }

        private String _Title;
        public String Title
        {
            get
            {
                return _Title;
            }
            set
            {
                _Title = value;
                Content.Title = _Title;
            }
        }

        private String _Text;
        public new String Text
        {
            get
            {
                return _Text;
            }
            set
            {
                _Text = value;
                CardContent.Text = _Text;
                Content.Height = 80 + CardContent.Height;
                Content.Width = CardContent.Width + 10;
                Height = Content.Height + Content.Location.Y;
                UserNameLabel.Location = new Point(10, CardContent.Bottom);
            }
        }

        private DateTime _Time;
        public DateTime Time
            {
            get { return _Time;}
            set
                {
                    _Time = value;
                    TimeLabel.Text = value.ToString("dd.MM.yyyy")+ "\r\n" +value.ToString("HH:mm:ss");
                }
        }
        private MaterialAvatarView UserImage;
        private MaterialCard Content;
        private MaterialLabel CardContent;
        private MaterialLabel TimeLabel;
        private MaterialLabel UserNameLabel;

        public MaterialTimeLineEntry()
        {
            StringGraphics = Graphics.FromImage(new Bitmap(10, 10));
            AutoSize = true;
            UserImage = new MaterialAvatarView();
            UserImage.Size = new Size(40,40);
            
            UserImage.Location = new Point(90, 20);
            UserImage.Avatar = null;
            Content = new MaterialCard();
            Content.Location = new Point(20, UserImage.Right + 20);
            Content.AutoSize = true;
            Content.Padding = new Padding(10, 30, 10, 10);
            Content.LargeTitle = true;
            CardContent = new MaterialLabel();
            CardContent.AutoSize = true;
            CardContent.Location = new Point(10, 40);
            Content.Controls.Add(CardContent);
            TimeLabel = new MaterialLabel();
            TimeLabel.Location = new Point(0, 30);
            UserNameLabel = new MaterialLabel();
            UserNameLabel.Location = new Point(10, CardContent.Bottom);
            Content.Controls.Add(UserNameLabel);
            Controls.Add(UserImage);
            Controls.Add(TimeLabel);
            Controls.Add(Content);

            UserImage.Click += OnClick;
            Content.Click += OnClick;
            CardContent.Click += OnClick;
            TimeLabel.Click += OnClick;
            UserNameLabel.Click += OnClick;
            
        }

        private void OnClick(object sender, EventArgs e)
        {
            OnClick(e);
        }

        public MaterialCard getContent()
        {
            return Content;
        }

        public MaterialAvatarView getAvatar()
        {
            return UserImage;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Content.Width = Width - UserImage.Right-10;
            Content.Location = new Point(UserImage.Right + 5, 20);
            UserNameLabel.Location = new Point(10, CardContent.Bottom);

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);

            e.Graphics.FillRectangle(MaterialSkinManager.Instance.ColorScheme.PrimaryBrush, new Rectangle(105, 0, 10, Height));

        }
    
    }
}
