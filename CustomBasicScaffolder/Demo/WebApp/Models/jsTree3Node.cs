using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jsTree3.Models
{
    public class JsTree3Node
    {
        public string id;
        public string text;
        public string icon;
        public State state;
        public List<JsTree3Node> children;

        public static JsTree3Node NewNode(string id)
        {
            return new JsTree3Node()
            {
                id = id,
                text = string.Format("Node {0}", id),
                children = new List<JsTree3Node>()
            };
        }
    }
    public class State
    {
        public bool opened = false;
        public bool disabled = false;
        public bool selected = false;

        public State(bool Opened, bool Disabled, bool Selected)
        {
            opened = Opened;
            disabled = Disabled;
            selected = Selected;
        }
    }
   
}