using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ElectionsMobile.Resources;
using ElectionsMobile.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;

using Newtonsoft.Json;



namespace ElectionsMobile.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {   //Add of the List ListePrenomOui/Non witch take all the name where the vote is yes/no
        private List<String> ListePrenomOui = new List<string>();
        private List<String> ListePrenomNon = new List<string>();
        //It is here that we declare the URL to get all Elections with vote and prenom
        const string apiUrl = @"http://coreosjpg.cloudapp.net/api/Votes/Elections";
        //constructor
        public MainViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
        }
        public ObservableCollection<ItemViewModel> Items { get; private set; }
        public bool IsDataLoaded { get; set; }
        public void LoadData()
        {
            if (this.IsDataLoaded == false)
            {
                this.Items.Clear();
                this.Items.Add(new ItemViewModel()
                {
                    //Declaration of sentence we will put on the screen until the application finish to download Elections
                    ID = "0",
                    LineOne = "Please Wait...",
                    LineTwo = "Please wait while Elections are downloaded from the server.",
                    LineThree = null
                });
                WebClient webClient = new WebClient();

                //Here we delete the cache
                webClient.Headers["Cache-Control"] = "no-cache";
                webClient.Headers["Accept"] = "application/json";
                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadElectionCompleted);
                webClient.DownloadStringAsync(new Uri(apiUrl));
            }
        }
        //It's the fonction to put firstname (yes or no) in a list.
        public string getlisteprenom(List<string> listeprenom, int nb)
        {
            for (int i = 0; i < nb; i++)
            {
                return listeprenom[i] + Environment.NewLine;
            }
            return string.Empty;
        }

        private void webClient_DownloadElectionCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                this.Items.Clear();
                if (e.Result != null)
                {


                    var elections = JsonConvert.DeserializeObject<ElectionDetails[]>(e.Result);
                    
                    //here we will do the treatment for each elections
                    foreach (ElectionDetails election in elections)
                    {
                        //We create a new VoteDetails's list
                        List<VoteDetails> LV1 = new List<VoteDetails>();
                        //initialisation of some variables
                        int Nb_Oui = 0;
                        int Nb_Non = 0;
                        //treatment for each vote
                        foreach (var votes in election.VoteDetails)
                        {
                            //if the choice is 1 it's yes
                            if (votes.choix == 1)
                            {
                                Nb_Oui++;
                                ListePrenomOui.Add(votes.prenom);
                            }
                                //else it's no
                            else
                            {
                                Nb_Non++;
                                ListePrenomNon.Add(votes.prenom);
                            }

                        }
                        this.Items.Add(new ItemViewModel()
                        {
                            LineOne = election.id,//we put the id of the election
                            LineTwo = "Votes: Oui = " + Nb_Oui + " Non = " + Nb_Non,//we put the number of yes and no
                            LineThree = "Total = " + election.VoteDetails.Count.ToString() + " votes",//we put the number of person who have vote
                            

                        });
                    } this.IsDataLoaded = true;
                }
            }
            catch (Exception ex)
            {
                this.Items.Add(new ItemViewModel()
                {//Declaration of sentence we will put on the screen if there is a problem with the application
                    ID = "0",
                    LineOne = "An Error Occurred",
                    LineTwo = String.Format("The following exception occured: {0}", ex.Message),
                    LineThree = String.Format("Additional inner exception information: {0}", ex.InnerException.Message)
                });
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}