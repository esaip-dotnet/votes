﻿/******************************************************************************\
* Copyright (C) 2012-2014 Leap Motion, Inc. All rights reserved.               *
* Leap Motion proprietary and confidential. Not for distribution.              *
* Use subject to the terms of the Leap Motion SDK Agreement available at       *
* https://developer.leapmotion.com/sdk_agreement, or another agreement         *
* between Leap Motion and you, your company or other organization.             *
* @Authors : Esaip's students                                                  *
* @Promotion : IR2016                                                          *
* @Tutor : Jean-Philippe Gouigoux                                              *
* @Date 22/12/2015 (DD/MM/YY)                                                  *
* @Brief : Leap Motion vote application                                       *
\******************************************************************************/
using System;
using System.Threading;
using Leap;
using System.Collections.Generic;
///////////
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Configuration;
using System.String;
using System.Runtime.Serialization;
using System.ServiceModel.Web;
using System.Runtime.Serialization.Json;


namespace VoteByLeapMotionProject
{

    class SampleListener : Listener
    {
        private Object thisLock = new Object();
        private List<Election> elections;
        private Choix choixEnCours = null;
        private int compteur = 0;
        private int DELAIS = 3;
        private String messageAttente = ".";

        /// <summary>
        /// To initialize the basic elements of the election
        /// </summary>
        public void initSampleListener() {
            string urlToCall = ConfigurationManager.AppSettings["urlServer"];
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(urlToCall);
                request.ContentType = "application/json";
                request.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                SafeWriteLine(response.ContentType);
                Stream streamReader = response.GetResponseStream();

                using (Stream stream = response.GetResponseStream())
                {
                    Type serializationTargetType = typeof(List<Election>);
                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(serializationTargetType);

                    elections = (List<Election>)jsonSerializer.ReadObject(stream);
                }
                foreach (Election election in elections)
                {
                    string[] varSplit = { "'" };
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
                            SafeWriteLine(e.Message);
                        }

                    }
                }
            }
            catch (WebException we)
            {
               
            }
            catch (Exception e)
            {
            }
            afficheInfo(elections[0]);

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
        /// This function is continuously called by the controller , one pause per frame is made in this function.
        /// </summary>
        /// <param name="controller"></param>
        public override void OnFrame(Controller controller)
        {
            Election election = elections[0];
            Frame frame = controller.Frame();
            int nombreDeMains = frame.Hands.Count;
            if(choixEnCours==null){
                if (election.choix.ContainsKey(nombreDeMains))
                {
                    choixEnCours = election.choix[nombreDeMains];
                    SafeWriteLine("Vous allez voter " + choixEnCours.nom +" , laissez vos mains en position pendant "+DELAIS+" secondes pour confirmer!");
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
                if (nombreDeMains == choixEnCours.id && compteur < DELAIS)
                {
                    SafeWriteLine(DELAIS - compteur + "...");
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    compteur++;
                }
                else if (nombreDeMains != choixEnCours.id)
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
                    election.votes.Add(new Vote(prenom, choixEnCours));
                    envoiInformation(generateJSon(election));
                    choixEnCours = null;
                    compteur = 0;
                    afficheInfo(election);
                }
            }
            System.Threading.Thread.Sleep(1000);
        }
        /// <summary>
        /// Displays information for each new vote.
        /// </summary>
        /// <param name="election">Request the election object to display information choices , etc ...</param>

        private void afficheInfo(Election election)
        {
            SafeWriteLine("En attente de positionnement des mains.");
            foreach (KeyValuePair<int, Choix> entreeDictionnaire in election.choix)
            {
                SafeWriteLine("Pour voter " + entreeDictionnaire.Value.nom + ", positionnez " + entreeDictionnaire.Key + " main(s)");
            }
            Console.Write("\n");
        }
        
        /// <summary>
        /// TODO function for the transformation of objects related to the election in string in JSON format .
        /// </summary>
        /// <param name="election"> Election object containing other objects and Vote Choice</param>
        /// <returns>Returns the string in XML format</returns>
        public String generateJSon(Election election)
        {
            String json = "";

            for (int i = 0; i < election.votes.Count; i++)
            {
                Vote vote = election.votes[i];
                json += "{'choix':" + vote.choix.id + ", 'prenom':'" + vote.prenom + "'}";
                //Pensez à ajouter les virgules en cas d'envoi de plusieurs lignes.
            }
        
            return json;
        }
        /// <summary>
        /// Function for the transformation of objects related to the election in string in XML format.
        /// </summary>
        /// <param name="election"> Election object containing other objects and Vote Choice</param>
        /// <returns>Returns the string in XML format</returns>
        public String generateXML(Election election)
        {
            String xml = "<Election id:\"" + election.nom + "\">";
            foreach(Vote vote in election.votes){
                xml += "<Vote choix:\"" + vote.choix.id + "\" prenom:\"" + vote.prenom + "\"/>";
            }
            xml += "</Election>";
            return xml;
        }


        /// <summary>
        /// A function to send the information to the server.
        /// TODO
        /// </summary>
        /// <param name="fichier">requests the data to send ( JSON or XML format , to call with generateXML generateJSon and functions</param>
        public void envoiInformation(String fichier)
        {
            
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://coreosjpg.cloudapp.net/api/votes/Elections/BDE/Votes");
            request.ContentType = "application/json";
            request.Method = "POST";
            StreamWriter streamWriter = new StreamWriter(request.GetRequestStream());
            Console.WriteLine(fichier);
            streamWriter.Write(fichier);
            streamWriter.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            
   
       }

        private string generateXML(string fichier)
        {
            throw new NotImplementedException();
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
         //   Console.WriteLine("Appuyez sur la touche échape pour quitter: \n");
            listener.initSampleListener();
            while (true) ;

        }
    }
}