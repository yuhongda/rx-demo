using JDH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

/// <summary>
/// Summary description for XmlDAL
/// </summary>
public class XmlDAL
{
	public XmlDAL()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    private const string dataXml = "~/App_Data/data.xml";


    private XDocument _comment;
    public XDocument Comment
    {
        get
        {
            if (this._comment == null)
                this._comment = XDocument.Load(HttpContext.Current.Server.MapPath(dataXml));
            return this._comment;
        }
    }



    public void AddComment(Comment comment)
    {
        this.Comment.Root.Add(new XElement("comment",
            new XElement("id", new XText(comment.ID)),
            new XElement("content", new XText(comment.Content)),
            new XElement("date", new XText(comment.Date))
            ));
        this.Comment.Save(HttpContext.Current.Server.MapPath(dataXml));
    }

    public List<Comment> GetComments()
    {
        var comments = from s in this.Comment.Descendants("comment")
                   select new Comment
                   {
                       ID = s.Element("id").Value,
                       Content = s.Element("content").Value,
                       Date = s.Element("date").Value,
                   };
        return comments.ToList<Comment>();
    }

    public Comment GetCommentByID(string id)
    {
        var comments = from s in this.Comment.Descendants("comment")
                   select new Comment
                   {
                       ID = s.Element("id").Value,
                       Content = s.Element("content").Value,
                       Date = s.Element("date").Value
                   };
        return comments.ToList<Comment>().SingleOrDefault(s => s.ID == id);
    }

    public void UpdateComment(Comment comment)
    {
        XElement el = this.Comment.Descendants("sub").SingleOrDefault(s => s.Element("id").Value == comment.ID);
        el.Element("content").SetValue(comment.Content);
        el.Element("date").SetValue(comment.Date);
        this.Comment.Save(HttpContext.Current.Server.MapPath(dataXml));
    }

    public int GetCommentMaxID()
    {
        var ids = from s in this.Comment.Descendants("comment")
                  select new
                    {
                        id = int.Parse(s.Element("id").Value)
                    };
        return ids.Max(i => i.id) > 0 ? ids.Max(i => i.id) : 0;
    }

}