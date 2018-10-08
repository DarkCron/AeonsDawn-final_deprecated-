namespace Game1.Forms.ScriptForms.ScriptCommandForms
{
    partial class TSPCommand
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
            this.TI = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // TIF
            // 
            this.TIF.AutoSize = true;
            this.TIF.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(64)));
            this.TIF.Location = new System.Drawing.Point(26, 55);
            this.TIF.Name = "TIF";
            this.TIF.Size = new System.Drawing.Size(80, 26);
            this.TIF.TabIndex = 37;
            this.TIF.Text = "@TSP";
            // 
            // TI
            // 
            this.TI.AutoSize = true;
            this.TI.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.TI.Location = new System.Drawing.Point(199, 55);
            this.TI.Name = "TI";
            this.TI.Size = new System.Drawing.Size(19, 26);
            this.TI.TabIndex = 36;
            this.TI.Text = "/";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(12, 115);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(120, 27);
            this.label5.TabIndex = 35;
            this.label5.Text = "Available teams:";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 145);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(120, 212);
            this.listBox1.TabIndex = 34;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(163, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 39);
            this.label3.TabIndex = 33;
            this.label3.Text = "TEAM ID here:";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(349, 132);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 67);
            this.button4.TabIndex = 32;
            this.button4.Text = "OK";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(116, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 51);
            this.label1.TabIndex = 31;
            this.label1.Text = "-";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(166, 115);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 27);
            this.label2.TabIndex = 39;
            this.label2.Text = "Characters in the selected team: (only for overview)";
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(166, 145);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(120, 212);
            this.listBox2.TabIndex = 38;
            // 
            // TSPCommand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 418);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.TIF);
            this.Controls.Add(this.TI);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label1);
            this.Name = "TSPCommand";
            this.Text = "TSPCommand";
            this.Load += new System.EventHandler(this.TSPCommand_Load);
            this.Enter += new System.EventHandler(this.TSPCommand_Enter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TIF;
        private System.Windows.Forms.Label TI;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBox2;
    }
}