namespace VoteByLeapMotionProject
{
    partial class LeapMotionVoteSystemWindow
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
            this.titleLabel = new System.Windows.Forms.Label();
            this.nextElection = new System.Windows.Forms.Button();
            this.previousElection = new System.Windows.Forms.Button();
            this.addElection = new System.Windows.Forms.Button();

            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(270, 9);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(237, 25);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Leap Motion Vote System";
            // 
            // nextElection
            // 
            this.nextElection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nextElection.Location = new System.Drawing.Point(651, 199);
            this.nextElection.Name = "nextElection";
            this.nextElection.Size = new System.Drawing.Size(101, 23);
            this.nextElection.TabIndex = 1;
            this.nextElection.Text = "-->";
            this.nextElection.UseVisualStyleBackColor = true;
            this.nextElection.Click += new System.EventHandler(this.nextElection_Click);
            // 
            // previousElection
            // 
            this.previousElection.Location = new System.Drawing.Point(12, 199);
            this.previousElection.Name = "previousElection";
            this.previousElection.Size = new System.Drawing.Size(101, 23);
            this.previousElection.TabIndex = 2;
            this.previousElection.Text = "<--";
            this.previousElection.UseVisualStyleBackColor = true;
            this.previousElection.Click += new System.EventHandler(this.button2_Click);
            // 
            // addElection
            // 
            this.addElection.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addElection.Location = new System.Drawing.Point(651, 12);
            this.addElection.Name = "addElection";
            this.addElection.Size = new System.Drawing.Size(101, 70);
            this.addElection.TabIndex = 3;
            this.addElection.Text = "Ajouter une élection";
            this.addElection.UseVisualStyleBackColor = true;
            this.addElection.Click += new System.EventHandler(this.addElection_Click);
            // 
            // LeapMotionVoteSystemWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 417);
            this.Controls.Add(this.addElection);
            this.Controls.Add(this.previousElection);
            this.Controls.Add(this.nextElection);
            this.Controls.Add(this.titleLabel);
            this.Name = "LeapMotionVoteSystemWindow";
            this.Text = "Leap Motion Vote System Window";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Button nextElection;
        private System.Windows.Forms.Button previousElection;
        private System.Windows.Forms.Button addElection;

    }
}