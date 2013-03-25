using JDH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

/// <summary>
/// Summary description for SampleWebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class SampleWebService : System.Web.Services.WebService {

    public SampleWebService () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string GetData(string postdata)
    {
        JSONData obj = postdata.FromJsonTo<JSONData>();
        XmlDAL xml = new XmlDAL();
        List<Comment> comments = xml.GetComments().Where(c => c.ID.ToInt() > obj.LastId.ToInt()).ToList<Comment>();
        return comments.ToJSON();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string AddComment(string postdata)
    {
        JSONDataComment obj = postdata.FromJsonTo<JSONDataComment>();
        XmlDAL xml = new XmlDAL();
        xml.AddComment(new Comment() { ID = (xml.GetCommentMaxID()+1).ToString(), Content = obj.Content, Date = DateTime.Now.ToString("yyyy-MM-dd hh:mm") });
        return new { Error = 0 }.ToJSON();
    }
    
}
