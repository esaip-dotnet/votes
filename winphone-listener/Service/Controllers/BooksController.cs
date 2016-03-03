using Service.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Xml.Linq;
namespace Service.Controllers
{
    public class BooksController : ApiController
    {
        private XDocument dom = null;
        public BooksController() { dom = XDocument.Load(HttpContext.Current.Server.MapPath("~/app_data/Books.xml")); }
        public IEnumerable<BookDetails> ReadAllBooks()
        {
            CultureInfo culture = new CultureInfo("en-US");
            List<BookDetails> Resultat = new List<BookDetails>();
            foreach (var Election in dom.Elements("Election"))
            {
                BookDetails BD1 = new BookDetails();
                BD1.id = Election.Attribute("id").Value;
                List<votes> LV1 = new List<votes>();

                foreach (var Vote in Election.Elements("votes"))
	            {
                    votes V1 = new votes();
                    V1.choix = int.Parse(Vote.Attribute("choix").Value);
                    V1.prenom = Vote.Attribute("prenom").Value;
                    LV1.Add(V1);
	            }
                BD1.votes = LV1;
                Resultat.Add(BD1);                 
            }
            return Resultat;
        }
        [HttpGet]
        public HttpResponseMessage Get() { IEnumerable<BookDetails> books = ReadAllBooks(); if (books != null) { return Request.CreateResponse<IEnumerable<BookDetails>>(HttpStatusCode.OK, books); } else { return Request.CreateResponse(HttpStatusCode.NotFound); } }
    }
}