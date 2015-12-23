namespace VoteByLeapMotionProject
{
    partial class VoteWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            
            this.labelTitleElection = new System.Windows.Forms.Label();
            this.labelAction = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelInfoChoix creating dynamicaly label for each choice contained inside the election
            // the posY change the vertical position for each label added 
            // 
            int posY = 62;
            foreach (int idChoix in election.choix.Keys)
            {
                System.Windows.Forms.Label labelInfoChoix = new System.Windows.Forms.Label();
                labelInfoChoix.AutoSize = true;
                labelInfoChoix.Location = new System.Drawing.Point(241, posY);
                labelInfoChoix.Name = "labelChoix" + election.choix[idChoix].nom;
                labelInfoChoix.Size = new System.Drawing.Size(0, 13);
                labelInfoChoix.TabIndex = 0;
                this.Controls.Add(labelInfoChoix);
                posY = posY + 28;
            }

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
            this.labelAction.Text = "";
            // 
            // VoteWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(952, 410);
            this.Controls.Add(this.labelAction);
            this.Controls.Add(this.labelTitleElection);
            this.Name = "Vote Window";
            this.Text = "Vous allez voter !";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        private System.Windows.Forms.Label labelTitleElection;
        private System.Windows.Forms.Label labelAction;
    }
}