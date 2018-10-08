namespace TBAGW.Forms.ScriptForms.ScriptCommandForms
{
    partial class TGOCommand
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
            this.TIF = new System.Windows.Forms.Label();
            this.LI = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.button4 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // TIF
            // 
            this.TIF.AutoSize = true;
            this.TIF.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(64)));
            this.TIF.Location = new System.Drawing.Point(18, 64);
            this.TIF.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TIF.Name = "TIF";
            this.TIF.Size = new System.Drawing.Size(104, 31);
            this.TIF.TabIndex = 34;
            this.TIF.Text = "@TGO";
            // 
            // LI
            // 
            this.LI.AutoSize = true;
            this.LI.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.LI.Location = new System.Drawing.Point(248, 64);
            this.LI.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LI.Name = "LI";
            this.LI.Size = new System.Drawing.Size(23, 31);
            this.LI.TabIndex = 33;
            this.LI.Text = "/";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(200, 9);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(153, 48);
            this.label3.TabIndex = 32;
            this.label3.Text = "Enter a ID here";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(130, 52);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 63);
            this.label1.TabIndex = 31;
            this.label1.Text = "-";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(165, 165);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(188, 22);
            this.numericUpDown1.TabIndex = 35;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(458, 55);
            this.button4.Margin = new System.Windows.Forms.Padding(4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(100, 82);
            this.button4.TabIndex = 36;
            this.button4.Text = "OK";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(13, 193);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(160, 33);
            this.label5.TabIndex = 38;
            this.label5.Text = "Script content here:";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(13, 230);
            this.listBox1.Margin = new System.Windows.Forms.Padding(4);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(421, 260);
            this.listBox1.TabIndex = 37;
            // 
            // TGOCommand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 510);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.TIF);
            this.Controls.Add(this.LI);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Name = "TGOCommand";
            this.Text = "TGOCommand";
            this.Load += new System.EventHandler(this.TGOCommand_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TIF;
        private System.Windows.Forms.Label LI;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox listBox1;
    }
}