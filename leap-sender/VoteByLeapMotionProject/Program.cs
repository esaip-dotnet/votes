/******************************************************************************\
* Copyright (C) 2012-2014 Leap Motion, Inc. All rights reserved.               *
* Leap Motion proprietary and confidential. Not for distribution.              *
* Use subject to the terms of the Leap Motion SDK Agreement available at       *
* https://developer.leapmotion.com/sdk_agreement, or another agreement         *
* between Leap Motion and you, your company or other organization.             *
\******************************************************************************/
using System;
using System.Threading;
using Leap;
using System.Collections.Generic;

namespace VoteByLeapMotionProject
{
    class SampleListener : Listener
    {
        private Object thisLock = new Object();
        private Election election;
        private Choix choixEnCours = null;
        private int compteur = 0;
        private int DELAIS = 3;
        private String messageAttente = ".";

        /// <summary>
        /// Create the listener for the basic elements of the election and choices,
        /// </summary>
        public void initSampleListener() {
            election = new Election("Referendum BDE");
            Choix choixOui = new Choix(1, "Oui", 1);
            election.getListeChoix().Add(choixOui.getNombreDeMainPourChoix(), choixOui);
            Choix choixNon = new Choix(2, "Non", 2);
            election.getListeChoix().Add(choixNon.getNombreDeMainPourChoix(), choixNon);
            afficheInfo(election);

        }
        private void SafeWriteLine(String line)
        {
            lock (thisLock)
            {
                Console.WriteLine(line);
            }
        }

        public override void OnInit(Controller controller)
        {
            SafeWriteLine("Initialized\n");

        }

        public override void OnConnect(Controller controller)
        {
            SafeWriteLine("Connected\n");
            controller.EnableGesture(Gesture.GestureType.TYPE_CIRCLE);
            controller.EnableGesture(Gesture.GestureType.TYPE_KEY_TAP);
            controller.EnableGesture(Gesture.GestureType.TYPE_SCREEN_TAP);
            controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE);
        }

        public override void OnDisconnect(Controller controller)
        {
            //Note: not dispatched when running in a debugger.
            SafeWriteLine("Disconnected\n");
        }

        public override void OnExit(Controller controller)
        {
            SafeWriteLine("Exited\n");
        }
        /// <summary>
        /// This function is continuously called by the controller, there is a 1s pause for each frame.
        /// </summary>
        public override void OnFrame(Controller controller)
        {
            Frame frame = controller.Frame();
            int nombreDeMains = frame.Hands.Count;
            if(choixEnCours==null){
                if (election.getListeChoix().ContainsKey(nombreDeMains))
                {
                    choixEnCours = election.getListeChoix()[nombreDeMains];
                    SafeWriteLine("Vous allez voter " + choixEnCours.getNom() +" , laissez vos mains en position pendant "+DELAIS+" secondes pour confirmer!");
                }
                else
                {
                    Console.SetCursorPosition(0, Console.CursorTop-1);
                    SafeWriteLine(messageAttente);
                    switch (messageAttente.LastIndexOf(".")){
                        case 0 :
                            messageAttente =".. ";
                            break;
                        case 1:
                            messageAttente = "...";
                            break;
                        case 2:
                            messageAttente = "   ";
                            break;
                        case -1:
                            messageAttente = ".  ";
                            break;
                    }
                }
            }
            else {
                if (nombreDeMains == choixEnCours.getNombreDeMainPourChoix() && compteur < DELAIS)
                {
                    SafeWriteLine(DELAIS - compteur + "...");
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    compteur++;
                }
                else if (nombreDeMains != choixEnCours.getNombreDeMainPourChoix())
                {
                    SafeWriteLine("Vous avez changé la position de vo(s)tre main(s), Votre vote n'est pas comptabilisé, vous pouvez recommencer.\n");
                    choixEnCours = null;
                    compteur = 0;
                }
                else
                {
                    SafeWriteLine("Saisissez votre prénom (Commencez par un espace):");
                    String prenom = "";
                    prenom = Console.ReadLine();
                    while (prenom.Length == 0)
                    {
                        SafeWriteLine("Aucun nom saisie, merci de resaisir votre prénom");
                        prenom = Console.ReadLine();
                    }
                    SafeWriteLine(prenom);
                    SafeWriteLine("Votre vote a bien été pris en compte, merci d'avoir voté!");
                    election.getListeVote().Add(new Vote(prenom, choixEnCours));
                    envoiInformation(generateXML(election));
                    choixEnCours = null;
                    compteur = 0;
                    afficheInfo(election);
                }
            }
            System.Threading.Thread.Sleep(1000);
        }
        /// <summary>
        /// Display informations for each new vote.
        /// </summary>
        private void afficheInfo(Election election)
        {

            SafeWriteLine("En attente du positionnement des mains.");

            election.getListeChoix().ContainsKey
            foreach (KeyValuePair<int, Choix> entreeDictionnaire in election.getListeChoix())
            {
                SafeWriteLine("Pour voter " + entreeDictionnaire.Value.getNom() + ", positionnez " + entreeDictionnaire.Key + " main(s)");
            }
            Console.Write("\n");
        }

        /// <summary>
        /// Generate the JSON String with the election informations.
        /// </summary>
        /// <param name="election"> Election object containing Votes and Choices</param>
        /// <returns>JSON String</returns>
        public String generateJSon(Election election)
        {
            String json = "{'id':'"+election.getNom()+"','votes':[";

            for (int i = 0; i < election.getListeVote().Count; i++)
            {
                Vote vote = election.getListeVote()[i];
                json += "{'choix':'" + vote.getChoixFait().getId() + "', 'prenom':'" + vote.getPrenom() + "'}";
            }
            json += "]}";
            return json;
        }
        /// <summary>
        /// Generate the XML with the election informations.
        /// </summary>
        /// <param name="election"> Election object containing Votes and Choices</param>
        /// <returns>XML String</returns>
        public String generateXML(Election election)
        {
            String xml = "<Election id:\"" + election.getNom() + "\">";
            foreach(Vote vote in election.getListeVote()){
                xml += "<Vote choix:\"" + vote.getChoixFait().getId() + "\" prenom:\"" + vote.getPrenom() + "\"/>";
            }
            xml += "</Election>";
            return xml;
        }
        /// <summary>
        /// Function to send the informations to the API
        /// </summary>
        /// <param name="fichier">JSON or XML file to send</param>
        public void envoiInformation(String fichier)
        {
            String url = "localhost:5004/api/votes/elections";
            String method = "POST";
            String data = fichier;
            ////TODO Create and call the method that send the votes
            SafeWriteLine("url = " +url + " xml "+fichier);
        }
    }

    class Sample
    {
        public static void Main()
        {
            // Create a sample listener and controller
            SampleListener listener = new SampleListener();
            Controller controller = new Controller();
            // Keep the process running until Enter is pressed
            controller.AddListener(listener);
            Console.WriteLine("Appuyez sur la touche échape pour quitter: \n");
            listener.initSampleListener();
            while (true) ;

        }
    }
}
