namespace VoteByLeapMotionProject
{
    partial class VoteWindow
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
            this.labelInfoChoix1 = new System.Windows.Forms.Label();
            this.labelInfoChoix2 = new System.Windows.Forms.Label();
            this.labelTitleElection = new System.Windows.Forms.Label();
            this.labelAction = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelInfoChoix1
            // 
            this.labelInfoChoix1.AutoSize = true;
            this.labelInfoChoix1.Location = new System.Drawing.Point(241, 62);
            this.labelInfoChoix1.Name = "labelInfoChoix1";
            this.labelInfoChoix1.Size = new System.Drawing.Size(0, 13);²
            this.labelInfoChoix1.TabIndex = 0;
            // 
            // labelInfoChoix2
            // 
            this.labelInfoChoix2.AutoSize = true;
            this.labelInfoChoix2.Location = new System.Drawing.Point(241, 90);
            this.labelInfoChoix2.Name = "labelInfoChoix2";
            this.labelInfoChoix2.Size = new System.Drawing.Size(0, 13);
            this.labelInfoChoix2.TabIndex = 1;
            // 
            // labelTitleElection
            // 
            this.labelTitleElection.AutoSize = true;
            this.labelTitleElection.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitleElection.Location = new System.Drawing.Point(439, 9);
            this.labelTitleElection.Name = "labelTitleElection";
            this.labelTitleElection.Size = new System.Drawing.Size(0, 25);
            this.labelTitleElection.TabIndex = 3;
            // 
            // labelAction
            // 
            this.labelAction.AutoSize = true;
            this.labelAction.Location = new System.Drawing.Point(334, 221);
            this.labelAction.Name = "labelAction";
            this.labelAction.Size = new System.Drawing.Size(59, 13);
            this.labelAction.TabIndex = 4;
            this.labelAction.Text = "labelAction";
            // 
            // VoteWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(952, 410);
            this.Controls.Add(this.labelAction);
            this.Controls.Add(this.labelTitleElection);
            this.Controls.Add(this.labelInfoChoix2);
            this.Controls.Add(this.labelInfoChoix1);
            this.Name = "VoteWindow";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelInfoChoix1;
        private System.Windows.Forms.Label labelInfoChoix2;
        private System.Windows.Forms.Label labelTitleElection;
        private System.Windows.Forms.Label labelAction;
    }
}