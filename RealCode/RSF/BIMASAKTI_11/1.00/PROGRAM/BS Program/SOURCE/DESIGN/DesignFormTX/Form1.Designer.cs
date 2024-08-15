namespace DesignFormGS
{
    partial class DesignFormTX
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            TXR00100 = new Button();
            SuspendLayout();
            // 
            // TXR00100
            // 
            TXR00100.Location = new Point(48, 82);
            TXR00100.Name = "TXR00100";
            TXR00100.Size = new Size(75, 23);
            TXR00100.TabIndex = 0;
            TXR00100.Text = "TXR00100";
            TXR00100.UseVisualStyleBackColor = true;
            TXR00100.Click += TXR00100_Click_1;
            // 
            // DesignFormTX
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(398, 345);
            Controls.Add(TXR00100);
            Margin = new Padding(2);
            Name = "DesignFormTX";
            Text = "DesignFormPMR";
            Load += DesignFormPMR_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button TXR00100;
    }
}