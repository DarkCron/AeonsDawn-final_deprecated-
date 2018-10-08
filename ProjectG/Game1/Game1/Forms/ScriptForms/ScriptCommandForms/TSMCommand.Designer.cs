namespace Game1.Forms.ScriptForms.ScriptCommandForms
{
    partial class TSMCommand
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
            this.SID = new System.Windows.Forms.Label();
            this.SMID = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.MapSearch = new System.Windows.Forms.TextBox();
            this.ObjectSearch = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // TIF
            // 
            this.TIF.AutoSize = true;
            this.TIF.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(64)));
            this.TIF.Location = new System.Drawing.Point(24, 58);
            this.TIF.Name = "TIF";
            this.TIF.Size = new System.Drawing.Size(83, 26);
            this.TIF.TabIndex = 30;
            this.TIF.Text = "@TSM";
            // 
            // SID
            // 
            this.SID.AutoSize = true;
            this.SID.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.SID.Location = new System.Drawing.Point(361, 61);
            this.SID.Name = "SID";
            this.SID.Size = new System.Drawing.Size(19, 26);
            this.SID.TabIndex = 29;
            this.SID.Text = "/";
            // 
            // SMID
            // 
            this.SMID.AutoSize = true;
            this.SMID.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.SMID.Location = new System.Drawing.Point(197, 58);
            this.SMID.Name = "SMID";
            this.SMID.Size = new System.Drawing.Size(19, 26);
            this.SMID.TabIndex = 28;
            this.SMID.Text = "/";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(161, 118);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(115, 27);
            this.label6.TabIndex = 25;
            this.label6.Text = "Scriptbool CHOICE identifier here";
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(156, 189);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(120, 212);
            this.listBox2.TabIndex = 24;
            this.listBox2.SelectedIndexChanged += new System.EventHandler(this.listBox2_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(10, 118);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(120, 27);
            this.label5.TabIndex = 23;
            this.label5.Text = "Scriptbool identifier here";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(10, 189);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(120, 212);
            this.listBox1.TabIndex = 22;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(326, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 38);
            this.label4.TabIndex = 21;
            this.label4.Text = "Object identifier here";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(161, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 39);
            this.label3.TabIndex = 20;
            this.label3.Text = "SubMap identifier here";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(347, 135);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 67);
            this.button4.TabIndex = 19;
            this.button4.Text = "OK";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(285, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 51);
            this.label2.TabIndex = 18;
            this.label2.Text = "-";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(114, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 51);
            this.label1.TabIndex = 17;
            this.label1.Text = "-";
            // 
            // MapSearch
            // 
            this.MapSearch.Location = new System.Drawing.Point(13, 159);
            this.MapSearch.Name = "MapSearch";
            this.MapSearch.Size = new System.Drawing.Size(117, 20);
            this.MapSearch.TabIndex = 31;
            this.MapSearch.TextChanged += new System.EventHandler(this.MapSearch_TextChanged);
            // 
            // ObjectSearch
            // 
            this.ObjectSearch.Location = new System.Drawing.Point(156, 159);
            this.ObjectSearch.Name = "ObjectSearch";
            this.ObjectSearch.Size = new System.Drawing.Size(117, 20);
            this.ObjectSearch.TabIndex = 32;
            this.ObjectSearch.TextChanged += new System.EventHandler(this.ObjectSearch_TextChanged);
            // 
            // TSMCommand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 413);
            this.Controls.Add(this.ObjectSearch);
            this.Controls.Add(this.MapSearch);
            this.Controls.Add(this.TIF);
            this.Controls.Add(this.SID);
            this.Controls.Add(this.SMID);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "TSMCommand";
            this.Text = "TSMCommand";
            this.Load += new System.EventHandler(this.TSMCommand_Load);
            this.Enter += new System.EventHandler(this.TSMCommand_Enter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TIF;
        private System.Windows.Forms.Label SID;
        private System.Windows.Forms.Label SMID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox MapSearch;
        private System.Windows.Forms.TextBox ObjectSearch;
    }
}