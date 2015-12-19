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
    {   //Add of the List ListePrenomOui witch take all the name where the vote is yes
        private List<String> ListePrenomOui = new List<string>();
        //It is here that we declare the URL to get all Elections with vote and prenom
        const string apiUrl = @"http://coreosjpg.cloudapp.net/api/Votes/Elections";
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
        private string getlisteprenom(List<string> listeprenom, int nb)
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
                    int id = 0;
                    foreach (ElectionDetails election in elections)
                    {
                        List<VoteDetails> LV1 = new List<VoteDetails>();
                        int Nb_Oui = 0;
                        int Nb_Non = 0;

                        List<String> ListePrenomNon = new List<string>();

                        foreach (var votes in election.VoteDetails)
                        {
                            if (votes.choix == 1)
                            {
                                Nb_Oui++;
                                ListePrenomOui.Add(votes.prenom);
                            }
                            else
                            {
                                Nb_Non++;
                                ListePrenomNon.Add(votes.prenom);
                            }

                        }
                        this.Items.Add(new ItemViewModel()
                        {
                            ID = (id++).ToString(),

                            LineOne = election.id,
                            LineTwo = "Votes: Oui = " + Nb_Oui + " Non = " + Nb_Non,
                            LineThree = "Total = " + election.VoteDetails.Count.ToString() + " votes",
                            // LineFour = "Coucou \n Kikou",
                            // LineFive = "Michel",
                            //LineFour = this.getlisteprenom(ListePrenomOui, Nb_Oui),

                        });
                    } this.IsDataLoaded = true;
                }
            }
            catch (Exception ex)
            {
                this.Items.Add(new ItemViewModel()
                {
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