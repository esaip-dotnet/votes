using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoteByLeapMotionProject
{
    class PanelElection
    {
            private System.Windows.Forms.Panel panelElection;
            private System.Windows.Forms.Label labelElection;
            private System.Windows.Forms.Button buttonResetElection;
            private System.Windows.Forms.Button buttonGoElection;
        /// <summary>
        /// Class allowing to add a panel containing the data and the button for the election on the systeme votes
        /// </summary>
        /// <param name="idElection"></param>
        public PanelElection(Election election)
        {
            this.panelElection = new System.Windows.Forms.Panel();
            this.labelElection = new System.Windows.Forms.Label();
            this.buttonResetElection = new System.Windows.Forms.Button();
            this.buttonGoElection = new System.Windows.Forms.Button();
            this.panelElection.SuspendLayout();

            this.panelElection.Controls.Add(this.labelElection);
            this.panelElection.Controls.Add(this.buttonResetElection);
            this.panelElection.Controls.Add(this.buttonGoElection);
            this.panelElection.Location = new System.Drawing.Point(305, 157);
            this.panelElection.Name = "panelElection";
            this.panelElection.Size = new System.Drawing.Size(159, 134);
            this.panelElection.TabIndex = 4;
            // 
            // labelElection
            // 
            this.labelElection.AutoSize = true;
            this.labelElection.Location = new System.Drawing.Point(63, 33);
            this.labelElection.Name = "labelElection";
            this.labelElection.Size = new System.Drawing.Size(35, 13);
            this.labelElection.TabIndex = 2;
            this.labelElection.Text = election.nom;
            // 
            // buttonResetElection
            // 
            this.buttonResetElection.Location = new System.Drawing.Point(43, 108);
            this.buttonResetElection.Name = "buttonResetElection";
            this.buttonResetElection.Size = new System.Drawing.Size(75, 23);
            this.buttonResetElection.TabIndex = 1;
            this.buttonResetElection.Text = "Réinitialiser les votes de cette élection";
            this.buttonResetElection.UseVisualStyleBackColor = true;
            // 
            // buttonGoElection
            // 
            this.buttonGoElection.Location = new System.Drawing.Point(43, 79);
            this.buttonGoElection.Name = "buttonGoElection";
            this.buttonGoElection.Size = new System.Drawing.Size(75, 23);
            this.buttonGoElection.TabIndex = 0;
            this.buttonGoElection.Text = "Démarrer le système de vote pour cette élection";
            this.buttonGoElection.UseVisualStyleBackColor = true;
            this.panelElection.ResumeLayout(false);
            this.panelElection.PerformLayout();
        }
    }
}
