﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MaterialWinforms.Controls
{
    public partial class MaterialSlider : Control, IMaterialControl
    {
        [Browsable(false)]
        public int Depth { get; set; }
        [Browsable(false)]
        public MaterialSkinManager SkinManager { get { return MaterialSkinManager.Instance; } }
        [Browsable(false)]
        public MouseState MouseState { get; set; }
        public Color BackColor { get { return Parent == null ? SkinManager.GetApplicationBackgroundColor() : typeof(IShadowedMaterialControl).IsAssignableFrom(Parent.GetType()) ? ((IMaterialControl)Parent).BackColor : Parent.BackColor; } }
        [Browsable(false)]

        private string _MaxValueLabel = null;
        private string _MinValueLabel = null;
        public string MaxValueLabel
        {
            get { return _MaxValueLabel; }
            set { _MaxValueLabel = value; }
        }
        public string MinValueLabel
        {
            get { return _MinValueLabel; }
            set { _MinValueLabel = value; }
        }

        public delegate void ValueChanged(double newValue);
        public event ValueChanged onValueChanged;

        private double _Value;
        public double Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                MouseX = (int)((_Value-MinValue) / (MaxValue - MinValue) * (Width - IndicatorSize));
                RecalculateIndicator();
            }
        }
        private double _MaxValue;
        public double MaxValue
        {
            get { return _MaxValue; }
            set
            {
                _MaxValue = value;
                MouseX = (int)((_Value - MinValue) / (MaxValue - MinValue) * (Width - IndicatorSize));
                RecalculateIndicator();
            }
        }
        private double _MinValue;
        public double MinValue
        {
            get { return _MinValue; }
            set
            {
                _MinValue = value;
                MouseX = (int)((_Value - MinValue) / (MaxValue - MinValue) * (Width - IndicatorSize));
                RecalculateIndicator();
            }
        }
        private double _Interval;
        public double Interval
        {
            get { return _Interval; }
            set
            {
                _Interval = value;
                MouseX = (int)((_Value - MinValue) / (MaxValue - MinValue) * (Width - IndicatorSize));
                RecalculateIndicator();
            }
        }

        private bool MousePressed;
        private int MouseX;

        private int IndicatorSize;
        private bool hovered = false;

        private Rectangle IndicatorRectangle;
        private Rectangle IndicatorRectangleNormal;
        private Rectangle IndicatorRectanglePressed;
        private Rectangle IndicatorRectangleDisabled;

        public MaterialSlider()
        {
            InitializeComponent();
            SetStyle(ControlStyles.Selectable, true);
            IndicatorSize = 30;
            MaxValue = 100;
            Width = 80;
            MinValue = 0;
            Height = IndicatorSize + 10;
            Interval = 1;
            Value = 50;

            IndicatorRectangle = new Rectangle(0, 10, IndicatorSize, IndicatorSize);
            IndicatorRectangleNormal = new Rectangle();
            IndicatorRectanglePressed = new Rectangle();

            EnabledChanged += MaterialSlider_EnabledChanged;

            DoubleBuffered = true;

        }

        void MaterialSlider_EnabledChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Height = IndicatorSize + 10;
            MouseX = (int)((_Value-_MinValue) / (MaxValue - MinValue) * (Width - IndicatorSize));
            RecalculateIndicator();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            hovered = true;
            Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            hovered = false;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == System.Windows.Forms.MouseButtons.Left && e.Y > IndicatorRectanglePressed.Top && e.Y < IndicatorRectanglePressed.Bottom)
            {
                MousePressed = true;
                if (e.X >= IndicatorSize / 2 && e.X <= Width - IndicatorSize / 2)
                {
                    MouseX = e.X - IndicatorSize / 2;
                    double ValuePerPx = ((double)(MaxValue - MinValue)) / (Width - IndicatorSize);
                    double v = _MinValue + _Interval * (int)((ValuePerPx * MouseX) / _Interval);
                    //int v = (int)(ValuePerPx * MouseX);
                    if (v != _Value)
                    {
                        _Value = v;
                        if (onValueChanged != null) onValueChanged(_Value);
                    }
                    RecalculateIndicator();
                }
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            hovered = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            hovered = false;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            MousePressed = false;
            Invalidate();
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (MousePressed)
            {
                if (e.X >= IndicatorSize / 2 && e.X <= Width - IndicatorSize / 2)
                {
                    MouseX = e.X - IndicatorSize / 2;
                    double ValuePerPx = ((double)(MaxValue - MinValue)) / (Width - IndicatorSize);
                    double v = _MinValue + _Interval *  (int)((ValuePerPx * MouseX)/_Interval);
                    if (v != _Value)
                    {
                        _Value = v;
                        if (onValueChanged != null) onValueChanged(_Value);
                    }
                    RecalculateIndicator();
                }
            }
        }

        private void RecalculateIndicator()
        {
            int iWidht = Width - IndicatorSize;

            IndicatorRectangle = new Rectangle(MouseX, Height - IndicatorSize, IndicatorSize, IndicatorSize);
            IndicatorRectangleNormal = new Rectangle(IndicatorRectangle.X + (int)(IndicatorRectangle.Width * 0.25), IndicatorRectangle.Y + (int)(IndicatorRectangle.Height * 0.25), (int)(IndicatorRectangle.Width * 0.5), (int)(IndicatorRectangle.Height * 0.5));
            IndicatorRectanglePressed = new Rectangle(IndicatorRectangle.X + (int)(IndicatorRectangle.Width * 0.165), IndicatorRectangle.Y + (int)(IndicatorRectangle.Height * 0.165), (int)(IndicatorRectangle.Width * 0.66), (int)(IndicatorRectangle.Height * 0.66));
            IndicatorRectangleDisabled = new Rectangle(IndicatorRectangle.X + (int)(IndicatorRectangle.Width * 0.34), IndicatorRectangle.Y + (int)(IndicatorRectangle.Height * 0.34), (int)(IndicatorRectangle.Width * 0.33), (int)(IndicatorRectangle.Height * 0.33));
            Invalidate();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Bitmap bmp = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(bmp);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(BackColor);
            Color LineColor;
            Brush DisabledBrush;
            Color BalloonColor;

            if (SkinManager.Theme == MaterialSkinManager.Themes.DARK)
            {
                LineColor = Color.FromArgb((int)(2.55 * 30), 255, 255, 255);
            }
            else
            {
                LineColor = Color.FromArgb((int)(2.55 * (hovered ? 38 : 26)), 0, 0, 0);
            }

            DisabledBrush = new SolidBrush(LineColor);
            BalloonColor = Color.FromArgb((int)(2.55 * 30), (Value == 0 ? Color.Gray : SkinManager.ColorScheme.AccentColor));

            Pen LinePen = new Pen(LineColor, 2);

            g.DrawLine(LinePen, IndicatorSize / 2, Height / 2 + (Height - IndicatorSize) / 2, Width - IndicatorSize / 2, Height / 2 + (Height - IndicatorSize) / 2);

            if (Enabled)
            {
                g.DrawLine(SkinManager.ColorScheme.AccentPen, IndicatorSize / 2, Height / 2 + (Height - IndicatorSize) / 2, IndicatorRectangleNormal.X, Height / 2 + (Height - IndicatorSize) / 2);

                if (MousePressed)
                {
                    if (Value > MinValue)
                    {
                        g.FillEllipse(SkinManager.ColorScheme.AccentBrush, IndicatorRectanglePressed);
                    }
                    else
                    {
                        g.FillEllipse(new SolidBrush(SkinManager.GetApplicationBackgroundColor()), IndicatorRectanglePressed);
                        g.DrawEllipse(LinePen, IndicatorRectanglePressed);
                    }
                }
                else
                {
                    if (Value > MinValue)
                    {
                        g.FillEllipse(SkinManager.ColorScheme.AccentBrush, IndicatorRectangleNormal);
                    }
                    else
                    {
                        g.FillEllipse(new SolidBrush(SkinManager.GetApplicationBackgroundColor()), IndicatorRectangleNormal);
                        g.DrawEllipse(LinePen, IndicatorRectangleNormal);
                    }


                    if (hovered)
                    {
                        g.FillEllipse(new SolidBrush(BalloonColor), IndicatorRectangle);
                    }
                }
            }
            else
            {
                if (Value > MinValue)
                {
                    g.FillEllipse(new SolidBrush(SkinManager.GetApplicationBackgroundColor()), IndicatorRectangleNormal);
                    g.FillEllipse(DisabledBrush, IndicatorRectangleDisabled);
                }
                else
                {
                    g.FillEllipse(new SolidBrush(SkinManager.GetApplicationBackgroundColor()), IndicatorRectangleNormal);
                    g.DrawEllipse(LinePen, IndicatorRectangleDisabled);
                }
            }

            if (_MinValueLabel == null)
            {
                g.DrawString(MinValue.ToString(), SkinManager.FONT_CONTROL_SMALL, SkinManager.GetPrimaryTextBrush(), new PointF(0, 0));
            }
            else
            {
                g.DrawString(_MinValueLabel, SkinManager.FONT_CONTROL_SMALL, SkinManager.GetPrimaryTextBrush(), new PointF(0, 0));
            }
            if (_MaxValueLabel == null)
            {
                g.DrawString(MaxValue.ToString(), SkinManager.FONT_CONTROL_SMALL, SkinManager.GetPrimaryTextBrush(), new PointF(Width - g.MeasureString(MaxValue.ToString(), SkinManager.FONT_CONTROL_SMALL).Width, 0f));
            }
            else
            {
                g.DrawString(_MaxValueLabel, SkinManager.FONT_CONTROL_SMALL, SkinManager.GetPrimaryTextBrush(), new PointF(Width - g.MeasureString(_MaxValueLabel, SkinManager.FONT_CONTROL_SMALL).Width, 0f));
            }
            if (_MaxValueLabel == null && _MinValueLabel == null)
            {
                g.DrawString(Value.ToString(), SkinManager.FONT_CONTROL_SMALL, SkinManager.GetPrimaryTextBrush(), new PointF(Width / 2 - g.MeasureString(Value.ToString(), SkinManager.FONT_CONTROL_SMALL).Width / 2, 0f));
            }
            e.Graphics.DrawImage((Image)bmp.Clone(), 0, 0);
        }
    }
}
