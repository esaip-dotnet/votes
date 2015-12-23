using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Net.NetworkInformation;
using BiblioMobile.Resources;
using System.Collections.Generic;
using Newtonsoft.Json;
using BiblioMobile.Models;
namespace BiblioMobile.ViewModels
{
  public class MainViewModel : INotifyPropertyChanged
  {
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
          ID = "0",
          LineOne = "Please Wait...",
          LineTwo = "Please wait while the catalog is downloaded from the server.",
          LineThree = null
        });
        WebClient webClient = new WebClient();

        // Delete cach memory
        webClient.Headers["Cache-Control"] = "no-cache";
        webClient.Headers["Accept"] = "application/json";
        webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadCatalogCompleted);
        webClient.DownloadStringAsync(new Uri(apiUrl));
      }
    }
    private void webClient_DownloadCatalogCompleted(object sender, DownloadStringCompletedEventArgs e)
    {
      try
      {
        this.Items.Clear();
        if (e.Result != null)
        {
          var books = JsonConvert.DeserializeObject<BookDetails[]>(e.Result);
          int id = 0;
          foreach (BookDetails book in books)
          {
            List<votes> LV1 = new List<votes>();
            int Nb_Oui = 0;
            int Nb_Non = 0;
            List<String> ListePrenomOui = new List<string>();
            List<String> ListePrenomNon = new List<string>();

            foreach (var votes in book.votes)
            {
              if (votes.choix == 1)
              {
                Nb_Oui++;
                ListePrenomOui.Add(votes.prenom);
              }
              else
              {
                Nb_Non++;
                ListePrenomOui.Add(votes.prenom);
              }          
            }
            this.Items.Add(new ItemViewModel()
            {
              ID = (id++).ToString(),            
              LineOne = book.id,
              LineTwo = "Votes: Oui = "+ Nb_Oui + " Non = " + Nb_Non,
              LineThree = "Total = "+ book.votes.Count.ToString() + " votes",
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