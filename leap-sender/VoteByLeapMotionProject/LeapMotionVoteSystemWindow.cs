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
        /// <summary>
        /// Action done with a click on the button buttonVote
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonVote_Click(object sender, EventArgs e)
        {
            election = (Election)this.electionComboBox.SelectedItem;
            //each time we click on the button, we put the label error to void and depending of the error occuring, 
            //we fullfil it with valuable informartion for the user
            this.labelError.Text ="";
            if (election != null)
            {
                String nom = this.textBoxName.Text;
                String password = this.textBoxPassword.Text;
                //we verify that the password and the name are not void and we see if it contains test inside, 
                //it's done insteed of the API connection system because this one is not yet available.
                if (nom != null && password != null && nom.Length > 0 && password.Length > 0 && nom.Contains("test") && password.Contains("test"))
                {
                    // if we are valid login and password and an election is selected, 
                    // we open a new window, sending it the election selected and the name of the user
                    VoteWindow voteForm = new VoteWindow(election, nom);
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
