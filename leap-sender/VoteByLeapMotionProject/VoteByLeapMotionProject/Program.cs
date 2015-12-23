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
///////////
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

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

    StringBuilder SB = new StringBuilder();

    /// <summary>
    ///Initialisation of basics elements  
    /// </summary>
    public void initSampleListener() 
    {
      election = new Election("BDE");
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
    /// This function is called by a controller , One second per second of break is done by frame
    /// </summary>
    /// <param name="controller"></param>
    public override void OnFrame(Controller controller)
    {
      Frame frame = controller.Frame();
      int nombreDeMains = frame.Hands.Count;
      if(choixEnCours==null)
      {
        if (election.getListeChoix().ContainsKey(nombreDeMains))
        {
          choixEnCours = election.getListeChoix()[nombreDeMains];
          SafeWriteLine("Vous allez voter " + choixEnCours.getNom() +" , laissez vos mains en position pendant "+DELAIS+" secondes pour confirmer!");
        }
        else
        {
          Console.SetCursorPosition(0, Console.CursorTop-1);
          SafeWriteLine(messageAttente);
          switch (messageAttente.LastIndexOf("."))
          {
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
      else 
      {
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
          envoiInformation(generateJSon(election));
          choixEnCours = null;
          compteur = 0;
          afficheInfo(election);
        }
      }
      System.Threading.Thread.Sleep(1000);
    }

    /// <summary>
    /// Permit the display of information for each vote.
    /// </summary>
    /// <param name="election"> request election object for the display of information like choix etc...</param>
    private void afficheInfo(Election election)
    {
      SafeWriteLine("En attente de positionnement des mains.");
      foreach (KeyValuePair<int, Choix> entreeDictionnaire in election.getListeChoix())
      {
        SafeWriteLine("Pour voter " + entreeDictionnaire.Value.getNom() + ", positionnez " + entreeDictionnaire.Key + " main(s)");
      }
      Console.Write("\n");
    }
        
    /// <summary>
    /// This function permit the transofrmation of  objects in link of election to character chain in JSON format. 
    /// </summary>
    /// <param name="election"> Election object contain others objects like Vote and Choix</param>
    /// <returns>character chain in JSON format</returns>
    public StringBuilder generateJSon(Election election)
    {
        StringBuilder json = SB.Append("");
      for (int i = 0; i < election.getListeVote().Count; i++)
      {
        Vote vote = election.getListeVote()[i];
        json = SB.Append("{'choix':" + vote.getChoixFait().getNombreDeMainPourChoix() + ", 'prenom':'" + vote.getPrenom() + "'}");
      }
      return json;
    }  
  
    /// <summary>
    /// This function permit the transofrmation of  objects in link of election to character chain in XML format. 
    /// </summary>
    /// <param name="election"> Election object contain others objects like Vote and Choix</param>
    /// <returns> character chain in XML format</returns>
    public StringBuilder generateXML(Election election)
    {
        StringBuilder xml = SB.Append("<Election id:\"" + election.getNom() + "\">");
      foreach(Vote vote in election.getListeVote())
      {
        xml = SB.Append("<Vote choix:\"" + vote.getChoixFait().getNombreDeMainPourChoix() + "\" prenom:\"" + vote.getPrenom() + "\"/>");
      }
      xml = SB.Append("</Election>");
      return xml;
    }  

    /// <summary>
    /// This function send informations to the server
    /// </summary>
    /// <param name="fichier">Data to send (JSON or XML format, for find data, use generateXML or generateJSon functions)</param>
    public void envoiInformation(StringBuilder fichier)
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
    /// <summary>
    /// function how permit to lunch the application. 
    /// One process is running permanently
    /// </summary>
    public static void Main()
    { 
      // Create a sample listener and controller
      SampleListener listener = new SampleListener();
      Controller controller = new Controller();
      // Keep this process running until Enter is pressed
      controller.AddListener(listener);
      listener.initSampleListener();
      while (true) ;
    }
  }
}