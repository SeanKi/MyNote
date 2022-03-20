using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NSXNote
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                //if (webBrowser1.Document != null) {
                //    object html = webBrowser1.Document.InvokeScript

                webBrowser1.Navigate("");
                Application.DoEvents();
                webBrowser1.Document.OpenNew(false).Write("<html><body><div><span>Date</span>2022-03-20</div><div id=\"editable\">Edit this text</div></body></html>");

                foreach (HtmlElement el in webBrowser1.Document.All)
                {
                    el.SetAttribute("unselectable", "on");
                    el.SetAttribute("contenteditable", "false");
                }

                webBrowser1.Document.Body.SetAttribute("width", this.Width.ToString() + "px");
                webBrowser1.Document.Body.SetAttribute("height", "100%");
                webBrowser1.Document.Body.SetAttribute("contenteditable", "true");
                webBrowser1.Document.DomDocument.GetType().GetProperty("designMode").SetValue(webBrowser1.Document.DomDocument, "On", null);
                webBrowser1.IsWebBrowserContextMenuEnabled = false;

                JObject o1 = JObject.Parse(File.ReadAllText("config.json"));
                var a = o1;
                foreach (JProperty property in o1.Properties())
                {
                    string name = property.Name;
                    var n = treeView1.Nodes.Add(name);
                    foreach (var item in property.Value)
                    {
                        Console.WriteLine($"Value: {item}");
                        n.Nodes.Add(item.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Text != "")
            {
                try
                {
                    JObject o1 = JObject.Parse(File.ReadAllText(e.Node.Text));
                    // title
                    // brief
                    // content
                    if (o1["category"] != null)
                    {
                        var category = o1["category"].ToString();
                        var title = o1["title"].ToString();
                        Console.WriteLine($"{category} {title}");
                    }
                    if (o1["content"] != null)
                    {
                        var content = o1["content"].ToString();
                        var brief = o1["brief"].ToString();
                        webBrowser1.Document.GetElementById("editable").InnerHtml = content;
                    }
                    //htmlEditControl1.DocumentHTML = content;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
