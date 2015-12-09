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
        /// Permet d'initialiser les éléments de base de l'élection. 
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
        /// Cette fonction est appelée en continue par le controller, une pause de 1 seconde par frame est faite dans cette fonction.
        /// </summary>
        /// <param name="controller"></param>
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
                    SafeWriteLine("Saissisez votre prénom (Commencez par un espace):");
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
        /// Permet d'afficher les informations pour chaque nouveau vote.
        /// </summary>
        /// <param name="election">Demande l'objet election afin d'afficher les informations des choix, etc...</param>

        private void afficheInfo(Election election)
        {
            
            SafeWriteLine("En attente de positionnement des mains.");
      
            election.getListeChoix().ContainsKey
            foreach (KeyValuePair<int, Choix> entreeDictionnaire in election.getListeChoix())
            {
                SafeWriteLine("Pour voter " + entreeDictionnaire.Value.getNom() + ", positionnez " + entreeDictionnaire.Key + " main(s)");
            }
            Console.Write("\n");
        }
        
        /// <summary>
        /// TODO Fonction permettant la transformation des objets liés à l'élection en chaine de caractère au format JSON.
        /// </summary>
        /// <param name="election"> Objet Election contenant les autres objets Vote et Choix</param>
        /// <returns>Retourne la chaine de caractère au format JSON</returns>
        public String generateJSon(Election election)
        {
            String json = "{'id':'"+election.getNom()+"','votes':[";

            for (int i = 0; i < election.getListeVote().Count; i++)
            {
                Vote vote = election.getListeVote()[i];
                json += "{'choix':'" + vote.getChoixFait().getId() + "', 'prenom':'" + vote.getPrenom() + "'}";
                //Pensez à ajouter les virgules en cas d'envoi de plusieurs lignes.
            }
            json += "]}";
            return json;
        }
        /// <summary>
        /// Fonction permettant la transformation des objets liés à l'élection en chaine de caractère au format XML.
        /// </summary>
        /// <param name="election"> Objet Election contenant les autres objets Vote et Choix</param>
        /// <returns>Retourne la chaine de caractère au format XML</returns>
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
        /// Fonction permettant d'envoyer les informations vers le serveur
        /// TODO
        /// </summary>
        /// <param name="fichier">demande les données à envoyer (Format JSON ou XML, à appeler avec les fonctions generateXML et generateJSon</param>
        public void envoiInformation(String fichier)
        {
            String url = "coreosjpg.cloudapp.net/votes";
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
            // Keep this process running until Enter is pressed
            controller.AddListener(listener);
            Console.WriteLine("Appuyez sur la touche échape pour quitter: \n");
            listener.initSampleListener();
            while (true) ;

        }
    }
}