using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
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

        //
        // this function is done to get all the elections from the API, that allow the user to choose for which election he is voting
        // a Default election is done named BDE
        //

        public void addElectionsFromUrlIntoComboBox()
        {
            List<Election> elections = new List<Election>();
            elections.Add(new Election("BDE"));
            
            // We will try to catch information for the different elections directly from the server, a problem is occuring on this part, making the project unrunnable
            string urlToCall = ConfigurationManager.AppSettings["urlServer"];
           /* try
             {
                 HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(urlToCall);
                 request.ContentType = "application/json";
                 request.Method = "GET";
                 HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                 Stream streamReader = response.GetResponseStream();

                 using (Stream stream = response.GetResponseStream())
                 {
                     Type CollectionDataContractAttribute = typeof(List<Election>);
                     DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(CollectionDataContractAttribute);

                     elections = (List<Election>)jsonSerializer.ReadObject(stream);
                 }
             }
             catch (WebException we)
             {
                 System.Diagnostics.Debug.WriteLine(we);
                 labelError.Text = "Problème de connexion avec le serveur";
             }*/
             
            //this part add choice for each election depending to file app.config, to know more about choice generation, please watch the readme file of the leap motion part
            try
            {
                foreach (Election election in elections)
                {
                    string[] varSplit = { "," };
                    string[] tabLabelChoix = ConfigurationManager.AppSettings["Choix"].Split(varSplit, StringSplitOptions.None);
                    foreach (string labelChoix in tabLabelChoix)
                    {
                        int idChoix = 0;
                        try
                        {
                            idChoix = int.Parse(ConfigurationManager.AppSettings[labelChoix]);
                            election.choix.Add(idChoix, new Choix() { id = idChoix, nom = labelChoix });
                        }
                        catch (Exception e)
                        {
                            System.Diagnostics.Debug.WriteLine(e);
                            labelError.Text = "Problème avec le fichier App.config, les lignes avec les noms des choix ne contiennent pas de chiffre sur leurs valeurs";
                        }

                    }
                }

            }

            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }

            foreach (Election election in elections)
            {
                this.electionComboBox.Items.Add(election);
            }
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.titleLabel = new System.Windows.Forms.Label();
            this.electionComboBox = new System.Windows.Forms.ComboBox();
            this.selectVoteLabel = new System.Windows.Forms.Label();
            this.buttonVote = new System.Windows.Forms.Button();
            this.labelInstructionPassword = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.labelInstructionName = new System.Windows.Forms.Label();
            this.labelError = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(286, 9);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(237, 25);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Leap Motion Vote System";
            // 
            // electionComboBox
            // 
            this.electionComboBox.FormattingEnabled = true;
            this.electionComboBox.Location = new System.Drawing.Point(275, 120);
            this.electionComboBox.Name = "electionComboBox";
            this.electionComboBox.Size = new System.Drawing.Size(248, 21);
            this.electionComboBox.TabIndex = 1;
            // 
            // selectVoteLabel
            // 
            this.selectVoteLabel.AutoSize = true;
            this.selectVoteLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectVoteLabel.Location = new System.Drawing.Point(271, 97);
            this.selectVoteLabel.Name = "selectVoteLabel";
            this.selectVoteLabel.Size = new System.Drawing.Size(267, 20);
            this.selectVoteLabel.TabIndex = 2;
            this.selectVoteLabel.Text = "Selectionnez l\'élection de votre choix";
            // 
            // buttonVote
            // 
            this.buttonVote.Location = new System.Drawing.Point(326, 289);
            this.buttonVote.Name = "buttonVote";
            this.buttonVote.Size = new System.Drawing.Size(141, 55);
            this.buttonVote.TabIndex = 13;
            this.buttonVote.Text = "Commencer le vote";
            this.buttonVote.UseVisualStyleBackColor = true;
            this.buttonVote.Click += new System.EventHandler(this.buttonVote_Click);
            // 
            // labelInstructionPassword
            // 
            this.labelInstructionPassword.AutoSize = true;
            this.labelInstructionPassword.Location = new System.Drawing.Point(138, 215);
            this.labelInstructionPassword.Name = "labelInstructionPassword";
            this.labelInstructionPassword.Size = new System.Drawing.Size(168, 13);
            this.labelInstructionPassword.TabIndex = 12;
            this.labelInstructionPassword.Text = "Veuillez saisir votre mot de passe :";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(321, 208);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(273, 20);
            this.textBoxPassword.TabIndex = 11;
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(321, 182);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(273, 20);
            this.textBoxName.TabIndex = 10;
            // 
            // labelInstructionName
            // 
            this.labelInstructionName.AutoSize = true;
            this.labelInstructionName.Location = new System.Drawing.Point(181, 185);
            this.labelInstructionName.Name = "labelInstructionName";
            this.labelInstructionName.Size = new System.Drawing.Size(125, 13);
            this.labelInstructionName.TabIndex = 9;
            this.labelInstructionName.Text = "Veuillez saisir votre nom :";
            // 
            // labelError
            // 
            this.labelError.AutoSize = true;
            this.labelError.ForeColor = System.Drawing.Color.Red;
            this.labelError.Location = new System.Drawing.Point(378, 385);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(0, 13);
            this.labelError.TabIndex = 14;
            // 
            // LeapMotionVoteSystemWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 417);
            addElectionsFromUrlIntoComboBox();
            this.Controls.Add(this.labelError);
            this.Controls.Add(this.buttonVote);
            this.Controls.Add(this.labelInstructionPassword);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.labelInstructionName);
            this.Controls.Add(this.selectVoteLabel);
            this.Controls.Add(this.electionComboBox);
            this.Controls.Add(this.titleLabel);
            this.Name = "LeapMotionVoteSystemWindow";
            this.Text = "Leap Motion Vote System Window";
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.ComboBox electionComboBox;
        private System.Windows.Forms.Label selectVoteLabel;
        private System.Windows.Forms.Button buttonVote;
        private System.Windows.Forms.Label labelInstructionPassword;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label labelInstructionName;
        private System.Windows.Forms.Label labelError;

    }
}