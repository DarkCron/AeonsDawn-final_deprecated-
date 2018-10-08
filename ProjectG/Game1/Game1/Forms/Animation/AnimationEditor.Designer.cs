namespace TBAGW.Forms.Animation
{
    partial class AnimationEditor
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
            this.button10 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button62 = new System.Windows.Forms.Button();
            this.label66 = new System.Windows.Forms.Label();
            this.frameHeight = new System.Windows.Forms.NumericUpDown();
            this.label67 = new System.Windows.Forms.Label();
            this.frameWidth = new System.Windows.Forms.NumericUpDown();
            this.label68 = new System.Windows.Forms.Label();
            this.xOffSet = new System.Windows.Forms.NumericUpDown();
            this.yOffSet = new System.Windows.Forms.NumericUpDown();
            this.label70 = new System.Windows.Forms.Label();
            this.label69 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.frameHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.frameWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xOffSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yOffSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(11, 523);
            this.button10.Margin = new System.Windows.Forms.Padding(4);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(179, 86);
            this.button10.TabIndex = 61;
            this.button10.Text = "Done Editing Frames";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(104, 430);
            this.button9.Margin = new System.Windows.Forms.Padding(4);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(85, 86);
            this.button9.TabIndex = 60;
            this.button9.Text = "Select frames new sheet";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button62
            // 
            this.button62.Location = new System.Drawing.Point(11, 430);
            this.button62.Margin = new System.Windows.Forms.Padding(4);
            this.button62.Name = "button62";
            this.button62.Size = new System.Drawing.Size(85, 86);
            this.button62.TabIndex = 59;
            this.button62.Text = "Select frames current sheet";
            this.button62.UseVisualStyleBackColor = true;
            this.button62.Click += new System.EventHandler(this.button62_Click);
            // 
            // label66
            // 
            this.label66.AutoSize = true;
            this.label66.Location = new System.Drawing.Point(13, 249);
            this.label66.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(93, 17);
            this.label66.TabIndex = 58;
            this.label66.Text = "Frame Height";
            // 
            // frameHeight
            // 
            this.frameHeight.Location = new System.Drawing.Point(17, 269);
            this.frameHeight.Margin = new System.Windows.Forms.Padding(4);
            this.frameHeight.Maximum = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.frameHeight.Name = "frameHeight";
            this.frameHeight.Size = new System.Drawing.Size(160, 22);
            this.frameHeight.TabIndex = 57;
            this.frameHeight.Value = new decimal(new int[] {
            64,
            0,
            0,
            0});
            // 
            // label67
            // 
            this.label67.AutoSize = true;
            this.label67.Location = new System.Drawing.Point(13, 196);
            this.label67.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(88, 17);
            this.label67.TabIndex = 56;
            this.label67.Text = "Frame Width";
            // 
            // frameWidth
            // 
            this.frameWidth.Location = new System.Drawing.Point(17, 216);
            this.frameWidth.Margin = new System.Windows.Forms.Padding(4);
            this.frameWidth.Maximum = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.frameWidth.Name = "frameWidth";
            this.frameWidth.Size = new System.Drawing.Size(160, 22);
            this.frameWidth.TabIndex = 55;
            this.frameWidth.Value = new decimal(new int[] {
            64,
            0,
            0,
            0});
            // 
            // label68
            // 
            this.label68.AutoSize = true;
            this.label68.Location = new System.Drawing.Point(13, 103);
            this.label68.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(62, 17);
            this.label68.TabIndex = 54;
            this.label68.Text = "Y-OffSet";
            // 
            // xOffSet
            // 
            this.xOffSet.Location = new System.Drawing.Point(17, 61);
            this.xOffSet.Margin = new System.Windows.Forms.Padding(4);
            this.xOffSet.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.xOffSet.Name = "xOffSet";
            this.xOffSet.Size = new System.Drawing.Size(160, 22);
            this.xOffSet.TabIndex = 50;
            // 
            // yOffSet
            // 
            this.yOffSet.Location = new System.Drawing.Point(17, 122);
            this.yOffSet.Margin = new System.Windows.Forms.Padding(4);
            this.yOffSet.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.yOffSet.Name = "yOffSet";
            this.yOffSet.Size = new System.Drawing.Size(160, 22);
            this.yOffSet.TabIndex = 53;
            // 
            // label70
            // 
            this.label70.AutoSize = true;
            this.label70.Location = new System.Drawing.Point(13, 9);
            this.label70.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label70.Name = "label70";
            this.label70.Size = new System.Drawing.Size(150, 17);
            this.label70.TabIndex = 51;
            this.label70.Text = "Item texture/animation:";
            // 
            // label69
            // 
            this.label69.AutoSize = true;
            this.label69.Location = new System.Drawing.Point(13, 41);
            this.label69.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label69.Name = "label69";
            this.label69.Size = new System.Drawing.Size(62, 17);
            this.label69.TabIndex = 52;
            this.label69.Text = "X-OffSet";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 328);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 17);
            this.label1.TabIndex = 63;
            this.label1.Text = "Frame Interval";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(17, 348);
            this.numericUpDown1.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            -727379969,
            232,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(160, 22);
            this.numericUpDown1.TabIndex = 62;
            this.numericUpDown1.Value = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // AnimationEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(212, 617);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button62);
            this.Controls.Add(this.label66);
            this.Controls.Add(this.frameHeight);
            this.Controls.Add(this.label67);
            this.Controls.Add(this.frameWidth);
            this.Controls.Add(this.label68);
            this.Controls.Add(this.xOffSet);
            this.Controls.Add(this.yOffSet);
            this.Controls.Add(this.label70);
            this.Controls.Add(this.label69);
            this.Name = "AnimationEditor";
            this.Text = "AnimationEditor";
            this.Load += new System.EventHandler(this.AnimationEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.frameHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.frameWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xOffSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yOffSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button62;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.NumericUpDown frameHeight;
        private System.Windows.Forms.Label label67;
        private System.Windows.Forms.NumericUpDown frameWidth;
        private System.Windows.Forms.Label label68;
        private System.Windows.Forms.NumericUpDown xOffSet;
        private System.Windows.Forms.NumericUpDown yOffSet;
        private System.Windows.Forms.Label label70;
        private System.Windows.Forms.Label label69;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
    }
}