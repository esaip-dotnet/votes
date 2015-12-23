using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VoteByLeapMotionProject
{
    public partial class LeapMotionVoteSystemWindow : Form
    {
        private Election election;
        public LeapMotionVoteSystemWindow()
        {
            InitializeComponent();
        }

        private void buttonVote_Click(object sender, EventArgs e)
        {
            election = (Election)this.electionComboBox.SelectedItem;
            this.labelError.Text ="";
            if (election != null)
            {
                 String nom = this.textBoxName.Text;
                String password = this.textBoxPassword.Text;
                //we verify that the password and the name are not void and we see if it contains test inside, 
                //it's done insteed of the API connection system because this one is not yet available.
                if (nom != null && password != null && nom.Length > 0 && password.Length > 0 && nom.Contains("test") && password.Contains("test"))
                {
                    foreach (Choix choix in election.choix)
                    {
                        System.Diagnostics.Debug.WriteLine(choix.id);
                    }
                    VoteWindow voteForm = new VoteWindow(election);
                    voteForm.Show();
                }
                else{
                    this.labelError.Text = "nom et/ou mot de passe incorrect";
                }
                
            }
            else
            {
                this.labelError.Text = "Aucune élection de sélectionnée.";
            }
            
        }

    }
}
