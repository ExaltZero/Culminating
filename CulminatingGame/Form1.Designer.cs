namespace CulminatingGame
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pbWorld = new System.Windows.Forms.PictureBox();
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.tmrMove = new System.Windows.Forms.Timer(this.components);
            this.tmrAnimate = new System.Windows.Forms.Timer(this.components);
            this.tmrPolice = new System.Windows.Forms.Timer(this.components);
            this.lbScreenLoc = new System.Windows.Forms.Label();
            this.lbHide = new System.Windows.Forms.Label();
            this.lbWall = new System.Windows.Forms.Label();
            this.tmrEnAnimate = new System.Windows.Forms.Timer(this.components);
            this.Clock = new System.Windows.Forms.Timer(this.components);
            this.lbInfo = new System.Windows.Forms.Label();
            this.InfoDisplay = new System.Windows.Forms.Timer(this.components);
            this.tmrFin = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pbWorld)).BeginInit();
            this.SuspendLayout();
            // 
            // pbWorld
            // 
            this.pbWorld.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.pbWorld.Location = new System.Drawing.Point(0, 0);
            this.pbWorld.Name = "pbWorld";
            this.pbWorld.Size = new System.Drawing.Size(984, 765);
            this.pbWorld.TabIndex = 0;
            this.pbWorld.TabStop = false;
            this.pbWorld.Paint += new System.Windows.Forms.PaintEventHandler(this.pbWorld_Paint);
            // 
            // timerRefresh
            // 
            this.timerRefresh.Enabled = true;
            this.timerRefresh.Interval = 1;
            this.timerRefresh.Tick += new System.EventHandler(this.timerRefresh_Tick);
            // 
            // tmrMove
            // 
            this.tmrMove.Enabled = true;
            this.tmrMove.Interval = 1;
            this.tmrMove.Tick += new System.EventHandler(this.tmrMove_Tick);
            // 
            // tmrAnimate
            // 
            this.tmrAnimate.Enabled = true;
            this.tmrAnimate.Interval = 50;
            this.tmrAnimate.Tick += new System.EventHandler(this.tmrAnimate_Tick);
            // 
            // tmrPolice
            // 
            this.tmrPolice.Enabled = true;
            this.tmrPolice.Interval = 1;
            // 
            // lbScreenLoc
            // 
            this.lbScreenLoc.AutoSize = true;
            this.lbScreenLoc.Location = new System.Drawing.Point(12, 87);
            this.lbScreenLoc.Name = "lbScreenLoc";
            this.lbScreenLoc.Size = new System.Drawing.Size(35, 13);
            this.lbScreenLoc.TabIndex = 1;
            this.lbScreenLoc.Text = "label1";
            // 
            // lbHide
            // 
            this.lbHide.AutoSize = true;
            this.lbHide.Location = new System.Drawing.Point(12, 136);
            this.lbHide.Name = "lbHide";
            this.lbHide.Size = new System.Drawing.Size(35, 13);
            this.lbHide.TabIndex = 2;
            this.lbHide.Text = "label1";
            // 
            // lbWall
            // 
            this.lbWall.AutoSize = true;
            this.lbWall.Location = new System.Drawing.Point(12, 113);
            this.lbWall.Name = "lbWall";
            this.lbWall.Size = new System.Drawing.Size(35, 13);
            this.lbWall.TabIndex = 3;
            this.lbWall.Text = "label1";
            // 
            // tmrEnAnimate
            // 
            this.tmrEnAnimate.Enabled = true;
            this.tmrEnAnimate.Interval = 75;
            this.tmrEnAnimate.Tick += new System.EventHandler(this.tmrEnAnimate_Tick);
            // 
            // Clock
            // 
            this.Clock.Interval = 1000;
            this.Clock.Tick += new System.EventHandler(this.Clock_Tick);
            // 
            // lbInfo
            // 
            this.lbInfo.AutoSize = true;
            this.lbInfo.BackColor = System.Drawing.Color.LightSeaGreen;
            this.lbInfo.Font = new System.Drawing.Font("Courier New", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbInfo.ForeColor = System.Drawing.Color.PaleTurquoise;
            this.lbInfo.Location = new System.Drawing.Point(12, 9);
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.Size = new System.Drawing.Size(302, 31);
            this.lbInfo.TabIndex = 4;
            this.lbInfo.Text = "Find The Artifact.";
            this.lbInfo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // InfoDisplay
            // 
            this.InfoDisplay.Enabled = true;
            this.InfoDisplay.Tick += new System.EventHandler(this.InfoDisplay_Tick);
            // 
            // tmrFin
            // 
            this.tmrFin.Tick += new System.EventHandler(this.tmrFin_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 762);
            this.Controls.Add(this.lbInfo);
            this.Controls.Add(this.lbWall);
            this.Controls.Add(this.lbHide);
            this.Controls.Add(this.lbScreenLoc);
            this.Controls.Add(this.pbWorld);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.pbWorld)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbWorld;
        private System.Windows.Forms.Timer timerRefresh;
        private System.Windows.Forms.Timer tmrMove;
        private System.Windows.Forms.Timer tmrAnimate;
        private System.Windows.Forms.Timer tmrPolice;
        private System.Windows.Forms.Label lbScreenLoc;
        private System.Windows.Forms.Label lbHide;
        private System.Windows.Forms.Label lbWall;
        private System.Windows.Forms.Timer tmrEnAnimate;
        private System.Windows.Forms.Timer Clock;
        private System.Windows.Forms.Label lbInfo;
        private System.Windows.Forms.Timer InfoDisplay;
        private System.Windows.Forms.Timer tmrFin;
    }
}

