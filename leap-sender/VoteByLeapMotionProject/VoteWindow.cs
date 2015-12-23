using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Leap;
using System.Diagnostics;
using System.Configuration;
using System.Net;
using System.IO;

namespace VoteByLeapMotionProject
{
    public partial class VoteWindow : Form, ILeapEventDelegate
    {
        private Election election;
        private Controller controller;
        private LeapEventListener listener;
        private Choix actualChoice;
        private String nom;
        private int countDown;
        private int DELAY = int.Parse(ConfigurationManager.AppSettings["Delay"]);

        //part needed for the leapMotion 
        public VoteWindow()
        {
            InitializeComponent();
        }
        public VoteWindow(Election election, String nom)
        {
            this.election = election;
            this.nom = nom;
            InitializeComponent();
            this.controller = new Controller();
            this.listener = new LeapEventListener(this);
            controller.AddListener(listener);
        }

        private void buttonVote_Click(object sender, EventArgs e)
        {
           
        }
        delegate void LeapEventDelegate(string EventName);
        public void LeapEventNotification(string EventName)
        {
            if (!this.InvokeRequired)
            {
                switch (EventName)
                {
                    case "onInit":
                        Debug.WriteLine("Init");
                        break;
                    case "onConnect":
                        this.connectHandler();
                        break;
                    case "onFrame":
                        if (!this.Disposing)
                            this.newFrameHandler(this.controller.Frame());
                        break;
                }
            }
            else
            {
                BeginInvoke(new LeapEventDelegate(LeapEventNotification), new object[] { EventName });
            }
        }
        void connectHandler()
        {
        }
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (components != null)
                    {
                        components.Dispose();
                    }
                    this.controller.RemoveListener(this.listener);
                    this.controller.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
        /// <summary>
        /// part of treatment for the leap motion.
        /// </summary>
        /// <param name="frame">frame of the system</param>
         void newFrameHandler(Frame frame)
        {
            
            int handCount = frame.Hands.Count;

             // part working when the system is waiting a choice from the user
            if (actualChoice == null)
            {
                if (election.choix.ContainsKey(handCount))
                {
                    actualChoice = election.choix[handCount];
                    this.labelAction.Text = "Vous allez voter " + actualChoice.nom + " , laissez vos mains en position pendant " + DELAY + " secondes pour confirmer!";
                }
                else
                {
                    this.labelAction.Text = "";

                }
            }
            //part working when the user already placed hand to make a choice 
            else
            {
                //we are checking that between to loop of the system, the same number of hands are detected by the leapMotion
                //then we show the countdown to the user
                if (handCount == actualChoice.id && countDown < DELAY)
                {
                    this.labelAction.Text = DELAY - countDown + " ";
                    countDown++;
                }
                //if the count of hand is different, then we notify the user, and we start with a choice to void
                else if (handCount != actualChoice.id)
                {
                    this.labelAction.Text = "Vous avez changé la position de vo(s)tre main(s), Votre vote n'est pas comptabilisé, vous pouvez recommencer.\n";
                    actualChoice = null;
                    countDown = 0;
                }
                // if the countdown is over, then we send the vote to the server
                else
                {
                    try
                    {
                        election.votes.Add(new Vote(nom, actualChoice.id));
                        sendDatatoServer(generateJSon(election));
                        this.labelAction.Text = "Votre vote a bien été pris en compte, merci d'avoir voté! Cette fenetre se fermera automatiquement dans 8 secondes";
                    }
                    catch
                    {
                        this.labelAction.Text = "Erreur lors de l'envoi de votre vote au serveur!";
                    }
                    //  we free the leap motion, we wait 8 seconds, and we close the windows
                    Dispose(true);
                    System.Threading.Thread.Sleep(8000);
                    
                    this.Close();
                }
            }
            System.Threading.Thread.Sleep(1000);
         }
         /// <summary>
         /// TODO Fonction permettant la transformation des objets liés à l'élection en chaine de caractère au format JSON.
         /// </summary>
         /// <param name="election"> Objet Election contenant les autres objets Vote et Choix</param>
         /// <returns>Retourne la chaine de caractère au format JSON</returns>
         public String generateJSon(Election election)
         {
             String json = "";

             for (int i = 0; i < election.votes.Count; i++)
             {
                 Vote vote = election.votes[i];
                 json += "{'choix':" + vote.choix + ", 'prenom':'" + vote.prenom + "'}";
             }

             return json;
         }

         /// <summary>
         /// Fonction permettant d'envoyer les informations vers le serveur
         /// TODO
         /// </summary>
         /// <param name="fichier">demande les données à envoyer (Format JSON ou XML, à appeler avec les fonctions generateXML et generateJSon</param>
         public void sendDatatoServer(String fichier)
         {

             HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("/BDE");
             request.ContentType = "application/json";
             request.Method = "POST";
             StreamWriter streamWriter = new StreamWriter(request.GetRequestStream());
             Console.WriteLine(fichier);
             streamWriter.Write(fichier);
             streamWriter.Close();
             HttpWebResponse response = (HttpWebResponse)request.GetResponse();


         }
    }
    // interface needed for the leapMotion
    public interface ILeapEventDelegate
    {
        void LeapEventNotification(string EventName);
    }

     // listener needed for the leapMotion
    public class LeapEventListener : Listener
    {
        ILeapEventDelegate eventDelegate;

        public LeapEventListener(ILeapEventDelegate delegateObject)
        {
            this.eventDelegate = delegateObject;
        }
        public override void OnInit(Controller controller)
        {
            this.eventDelegate.LeapEventNotification("onInit");
        }
        public override void OnConnect(Controller controller)
        {
            this.eventDelegate.LeapEventNotification("onConnect");
        }
        public override void OnFrame(Controller controller)
        {
            this.eventDelegate.LeapEventNotification("onFrame");
        }
        public override void OnExit(Controller controller)
        {
            this.eventDelegate.LeapEventNotification("onExit");
        }
        public override void OnDisconnect(Controller controller)
        {
            this.eventDelegate.LeapEventNotification("onDisconnect");
        }
    }
}