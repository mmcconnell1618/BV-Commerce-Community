using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace BVSoftware.Web
{
    public class SiteMapNode
    {
        public string DisplayName { get; set; }
        public string Url { get; set; }
        public DateTime? LastModified { get; set; }
        public SiteMapPriority Priority { get; set; }
        public SiteMapChangeFrequency ChangeFrequency { get; set; }
        public List<SiteMapNode> Children { get; set; }

        public SiteMapNode()
        {
            DisplayName = string.Empty;
            LastModified = null;
            Priority = SiteMapPriority.NotSet;
            ChangeFrequency = SiteMapChangeFrequency.NotSet;
            Url = string.Empty;
            Children = new List<SiteMapNode>();
        }

        public void AddUrl(string url)
        {
            AddUrl(url, SiteMapPriority.p5);
        }
        public void AddUrl(string url, SiteMapPriority priority)
        {
            SiteMapNode n = new SiteMapNode() { Url = url, Priority = priority };

            // Site Map can't exceed 50,000 according to specs
            if (this.Children.Count < 49999)
            {
                this.Children.Add(n);
            }
        }
        public string RenderAsXmlSiteMap()
        {
            StringBuilder sb = new StringBuilder();
            string result = string.Empty;

            sb.Append("<?xml version='1.0' encoding='UTF-8'?>");
            sb.Append("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" ");
            sb.Append("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" ");
            sb.Append("xsi:schemaLocation=\"http://www.sitemaps.org/schemas/sitemap/0.9 ");
            sb.Append(" http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd\">" + System.Environment.NewLine);


            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = System.Text.Encoding.UTF8;
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;
            settings.ConformanceLevel = ConformanceLevel.Fragment;

            XmlWriter xw = XmlWriter.Create(sb, settings);

            RenderAsXmlSiteMapToStringBuilder(xw);

            xw.Flush();
            xw.Close();

            sb.Append(System.Environment.NewLine + "</urlset>");

            result = sb.ToString();
            return result;
        }

        public void RenderAsXmlSiteMapToStringBuilder(XmlWriter xw)
        {

            RenderThis(xw);

            if (this.Children != null)
            {
                foreach (SiteMapNode node in Children)
                {
                    node.RenderAsXmlSiteMapToStringBuilder(xw);
                }
            }
        }

        private void RenderThis(XmlWriter xw)
        {
            if (xw == null) return;

            if (this.Url.Trim().Length > 4)
            {
                xw.WriteStartElement("url");
                xw.WriteElementString("loc", this.Url);
                if (this.LastModified.HasValue)
                {
                    // write date in ISO 8601 Format
                    xw.WriteElementString("lastmod", this.LastModified.Value.ToString("s"));
                }
                if (this.Priority != SiteMapPriority.NotSet)
                {
                    switch (this.Priority)
                    {
                        case SiteMapPriority.p1:
                            xw.WriteElementString("priority", "0.1");
                            break;
                        case SiteMapPriority.p2:
                            xw.WriteElementString("priority", "0.2");
                            break;
                        case SiteMapPriority.p3:
                            xw.WriteElementString("priority", "0.3");
                            break;
                        case SiteMapPriority.p4:
                            xw.WriteElementString("priority", "0.4");
                            break;
                        case SiteMapPriority.p5:
                            xw.WriteElementString("priority", "0.5");
                            break;
                        case SiteMapPriority.p6:
                            xw.WriteElementString("priority", "0.6");
                            break;
                        case SiteMapPriority.p7:
                            xw.WriteElementString("priority", "0.7");
                            break;
                        case SiteMapPriority.p8:
                            xw.WriteElementString("priority", "0.8");
                            break;
                        case SiteMapPriority.p9:
                            xw.WriteElementString("priority", "0.9");
                            break;
                        case SiteMapPriority.p10:
                            xw.WriteElementString("priority", "1.0");
                            break;
                    }
                }
                if (this.ChangeFrequency != SiteMapChangeFrequency.NotSet)
                {
                    switch (this.ChangeFrequency)
                    {
                        case SiteMapChangeFrequency.always:
                            xw.WriteElementString("changefreq", "always");
                            break;
                        case SiteMapChangeFrequency.daily:
                            xw.WriteElementString("changefreq", "daily");
                            break;
                        case SiteMapChangeFrequency.hourly:
                            xw.WriteElementString("changefreq", "hourly");
                            break;
                        case SiteMapChangeFrequency.monthly:
                            xw.WriteElementString("changefreq", "monthly");
                            break;
                        case SiteMapChangeFrequency.never:
                            xw.WriteElementString("changefreq", "never");
                            break;
                        case SiteMapChangeFrequency.weekly:
                            xw.WriteElementString("changefreq", "weekly");
                            break;
                        case SiteMapChangeFrequency.yearly:
                            xw.WriteElementString("changefreq", "yearly");
                            break;
                    }
                }
                xw.WriteEndElement();

            }
        }
    }
}
