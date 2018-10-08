namespace Game1.Forms.ScriptForms.ScriptCommandForms
{
    partial class TRECommand
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
            this.label5 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // TIF
            // 
            this.TIF.AutoSize = true;
            this.TIF.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(64)));
            this.TIF.Location = new System.Drawing.Point(26, 61);
            this.TIF.Name = "TIF";
            this.TIF.Size = new System.Drawing.Size(81, 26);
            this.TIF.TabIndex = 30;
            this.TIF.Text = "@TRE";
            // 
            // LI
            // 
            this.LI.AutoSize = true;
            this.LI.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.LI.Location = new System.Drawing.Point(199, 61);
            this.LI.Name = "LI";
            this.LI.Size = new System.Drawing.Size(19, 26);
            this.LI.TabIndex = 28;
            this.LI.Text = "/";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(12, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(120, 27);
            this.label5.TabIndex = 23;
            this.label5.Text = "Script content here:";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 151);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(193, 212);
            this.listBox1.TabIndex = 22;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(163, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 39);
            this.label3.TabIndex = 20;
            this.label3.Text = "Repeating line here";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(383, 296);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 67);
            this.button4.TabIndex = 19;
            this.button4.Text = "OK";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(116, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 51);
            this.label1.TabIndex = 17;
            this.label1.Text = "-";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(234, 151);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown1.TabIndex = 31;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(234, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 27);
            this.label2.TabIndex = 32;
            this.label2.Text = "@THE_id to mark repeat line:\r\n";
            // 
            // TRECommand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 399);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.TIF);
            this.Controls.Add(this.LI);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label1);
            this.Name = "TRECommand";
            this.Text = "TRECommand";
            this.Load += new System.EventHandler(this.TRECommand_Load);
            this.Enter += new System.EventHandler(this.TRECommand_Enter);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TIF;
        private System.Windows.Forms.Label LI;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label2;
    }
}