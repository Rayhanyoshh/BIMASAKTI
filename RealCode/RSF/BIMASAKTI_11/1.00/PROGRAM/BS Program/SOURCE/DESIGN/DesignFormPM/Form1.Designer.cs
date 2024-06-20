namespace DesignFormGS
{
    partial class DesignFormPMR
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
            PMR01000 = new Button();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            PMR02100SummaryButton = new Button();
            button4 = new Button();
            button5 = new Button();
            button6 = new Button();
            SuspendLayout();
            // 
            // PMR01000
            // 
            PMR01000.Location = new Point(24, 24);
            PMR01000.Margin = new Padding(2);
            PMR01000.Name = "PMR01000";
            PMR01000.Size = new Size(78, 20);
            PMR01000.TabIndex = 0;
            PMR01000.Text = "PMR01001";
            PMR01000.UseVisualStyleBackColor = true;
            PMR01000.Click += PMR01001_Click;
            // 
            // button1
            // 
            button1.Location = new Point(24, 69);
            button1.Margin = new Padding(2);
            button1.Name = "button1";
            button1.Size = new Size(78, 20);
            button1.TabIndex = 1;
            button1.Text = "PMR01002";
            button1.UseVisualStyleBackColor = true;
            button1.Click += PMR01002_Click;
            // 
            // button2
            // 
            button2.Location = new Point(24, 115);
            button2.Margin = new Padding(2);
            button2.Name = "button2";
            button2.Size = new Size(78, 20);
            button2.TabIndex = 2;
            button2.Text = "PMR01003";
            button2.UseVisualStyleBackColor = true;
            button2.Click += PMR01003_Click;
            // 
            // button3
            // 
            button3.Location = new Point(128, 24);
            button3.Margin = new Padding(2);
            button3.Name = "button3";
            button3.Size = new Size(78, 20);
            button3.TabIndex = 3;
            button3.Text = "PMR02200";
            button3.UseVisualStyleBackColor = true;
            button3.Click += PMR02200_Click;
            // 
            // PMR02100SummaryButton
            // 
            PMR02100SummaryButton.Location = new Point(223, 119);
            PMR02100SummaryButton.Margin = new Padding(2);
            PMR02100SummaryButton.Name = "PMR02100SummaryButton";
            PMR02100SummaryButton.Size = new Size(135, 27);
            PMR02100SummaryButton.TabIndex = 4;
            PMR02100SummaryButton.Text = "PMR02104Summary";
            PMR02100SummaryButton.UseVisualStyleBackColor = true;
            PMR02100SummaryButton.Click += PMR02104SummaryButton_Click;
            // 
            // button4
            // 
            button4.Location = new Point(223, 88);
            button4.Margin = new Padding(2);
            button4.Name = "button4";
            button4.Size = new Size(135, 27);
            button4.TabIndex = 5;
            button4.Text = "PMR02103Summary";
            button4.UseVisualStyleBackColor = true;
            button4.Click += PMR02103SummaryButton_Click;
            // 
            // button5
            // 
            button5.Location = new Point(223, 57);
            button5.Margin = new Padding(2);
            button5.Name = "button5";
            button5.Size = new Size(135, 27);
            button5.TabIndex = 6;
            button5.Text = "PMR02102Summary";
            button5.UseVisualStyleBackColor = true;
            button5.Click += PMR02102SummaryButton_Click;
            // 
            // button6
            // 
            button6.Location = new Point(223, 24);
            button6.Margin = new Padding(2);
            button6.Name = "button6";
            button6.Size = new Size(135, 27);
            button6.TabIndex = 7;
            button6.Text = "PMR02101Summary";
            button6.UseVisualStyleBackColor = true;
            button6.Click += PMR02101SummaryButton_Click;
            // 
            // DesignFormPMR
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(680, 270);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(PMR02100SummaryButton);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(PMR01000);
            Margin = new Padding(2);
            Name = "DesignFormPMR";
            Text = "DesignFormPMR";
            Load += DesignFormPMR_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button PMR01000;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button PMR02100SummaryButton;
        private Button button4;
        private Button button5;
        private Button button6;
    }
}