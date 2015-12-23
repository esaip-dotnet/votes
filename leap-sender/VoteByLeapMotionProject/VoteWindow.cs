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

namespace VoteByLeapMotionProject
{
    public partial class VoteWindow : Form, ILeapEventDelegate
    {
        private Election election;
        private Controller controller;
        private LeapEventListener listener;
        private Choix actualChoice;
        private int countDown;
        private int DELAY = int.Parse(ConfigurationManager.AppSettings["Delay"]);
        public VoteWindow()
        {
            InitializeComponent();
        }
        public VoteWindow(Election election)
        {
            this.election = election;
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

        void newFrameHandler(Frame frame)
        {
            //The following are Label controls added in design view for the form
            int handCount = frame.Hands.Count;
            if(actualChoice==null){
                if (election.choix.ContainsKey(handCount))
                {
                    actualChoice = election.choix[handCount];
                    this.labelAction.Text="Vous allez voter " + actualChoice.nom +" , laissez vos mains en position pendant "+DELAY+" secondes pour confirmer!";
                }
                else
                {
                    

                }
            }
            else {
                if (handCount == actualChoice.id && countDown < DELAY)
                {
                    this.labelAction.Text=DELAY - countDown +" ";
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    countDown++;
                }
                else if (handCount != actualChoice.id)
                {
                    this.labelAction.Text="Vous avez changé la position de vo(s)tre main(s), Votre vote n'est pas comptabilisé, vous pouvez recommencer.\n";
                    actualChoice = null;
                    countDown = 0;
                }
                else
                {
                    this.labelAction.Text="Votre vote a bien été pris en compte, merci d'avoir voté!";
                    election.votes.Add(new Vote(renom, choixEnCours));
                    envoiInformation(generateJSon(election));
                    choixEnCours = null;
                     afficheInfo(election);
                }
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
    }
    public interface ILeapEventDelegate
    {
        void LeapEventNotification(string EventName);
    }

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